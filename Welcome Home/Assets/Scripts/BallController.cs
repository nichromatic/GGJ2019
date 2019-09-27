using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{

    Rigidbody rb;
    Camera cam;
    Vector3 startPos;
    bool used;
    Vector3 joystickDir;
    [HideInInspector]
    public bool loading = false;

    [Header("References")]
    public List<Sound> sounds = new List<Sound>();
    bool isRollingSoundPlaying = false;

    [Header("Physics")]
    public float maxForce;
    public float minForce;
    public bool onFloor = false;
    public float maxVelocity;

    [Header("Mouse wheel zoom")]
    public float minZoom = 2f;
    public float maxZoom = 7f;
    public float sensitivity = 0.5f;
    public Vector3 initialCamPos;

    //public Vector3 velocity;
    //public float velocityMagnitude;

    void Start()
    {
        loading = false;
        used = false;
        startPos = transform.position;
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        initialCamPos = cam.transform.parent.position;
        for (int i = 0; i < sounds.Count; i++)
        {
            AudioSource audiosrc = this.gameObject.AddComponent<AudioSource>();
            audiosrc.clip = sounds[i].clip;
            audiosrc.volume = sounds[i].volume;
            audiosrc.pitch = sounds[i].pitch;
            audiosrc.loop = sounds[i].loop;
            sounds[i].source = audiosrc;
        }
    }

    public IEnumerator NextScene()
    {
        loading = true;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(used);
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton3))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if ((Input.GetKey(KeyCode.H) || Input.GetKey(KeyCode.JoystickButton6)) && !used)
        {
                used = true;
                cam.transform.parent.GetComponent<Animator>().SetTrigger("End");
                StartCoroutine(NextScene());
                AudioManager.Instance.callFadeOut();

        }

        // Comprobamos si la bola está en el suelo ahora mismo
        Ray floorRay = new Ray(this.transform.position, new Vector3(0, -1, 0));
        if (Physics.Raycast(floorRay, 0.35f))
        {
            onFloor = true;
            sounds[0].volume = (rb.velocity.magnitude) / 10;
            playSound("rolling");
            isRollingSoundPlaying = true;
        }
        else
        {
            if (isRollingSoundPlaying)
            {
                //playSound("rolling");
                //isRollingSoundPlaying = !sounds[0].Fade(false, Time.deltaTime);
                stopSound("rolling");
            }

            onFloor = false;
        }

        // Aplicamos fuerza a la bola si se hace clic y está en el suelo
        if (Input.GetMouseButton(0) && onFloor)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, LayerMask.GetMask("Escenario")))
            {
                Vector3 projectedClick = Vector3.ProjectOnPlane(hit.point, Vector3.up);
                Vector3 projectedBallPosition = Vector3.ProjectOnPlane(transform.position, Vector3.up);
                Vector3 dir = projectedClick - projectedBallPosition;
                float forceMagnitude = Mathf.Clamp(dir.magnitude, minForce, maxForce);
                if (rb.velocity.magnitude <= maxVelocity)
                {
                    rb.AddForce(dir * forceMagnitude * Time.deltaTime, ForceMode.Acceleration);
                }
                else
                {
                    rb.AddForce(dir * forceMagnitude * Time.deltaTime, ForceMode.Acceleration);
                    float velocityExtra = Mathf.Abs((maxVelocity - rb.velocity.magnitude));
                    velocityExtra = Mathf.Clamp(velocityExtra, 0, 1);
                    rb.AddForce(-dir * forceMagnitude * Time.deltaTime * velocityExtra, ForceMode.Acceleration);
                }
            }
        }

        float xAxisDir = Input.GetAxis("Horizontal");
        float yAxisDir = Input.GetAxis("Vertical");
        if((xAxisDir != 0 || yAxisDir != 0)  && onFloor){
            Vector3 camForwardProyected = Vector3.ProjectOnPlane(cam.transform.forward,Vector3.up).normalized;
            float angle = Vector3.Angle(Vector3.forward,camForwardProyected);
            joystickDir.Set(xAxisDir,0,yAxisDir);
            float joystickTilt = Mathf.Min(joystickDir.magnitude,1);
            joystickDir.Normalize();
            joystickDir = Quaternion.Euler(0,angle,0) * joystickDir;
            float forceMagnitude = Mathf.Lerp(minForce, maxForce, joystickTilt);
            if (rb.velocity.magnitude <= maxVelocity)
            {
                rb.AddForce(joystickDir * forceMagnitude * Time.deltaTime, ForceMode.Acceleration);
            }
            else
            {
                rb.AddForce(joystickDir * forceMagnitude * Time.deltaTime, ForceMode.Acceleration);
                float velocityExtra = Mathf.Abs((maxVelocity - rb.velocity.magnitude));
                velocityExtra = Mathf.Clamp(velocityExtra, 0, 1);
                rb.AddForce(-joystickDir * forceMagnitude * Time.deltaTime * velocityExtra, ForceMode.Acceleration);
            }
        }

        // Mouse wheel zoom
        float currentSize = cam.orthographicSize;
        float scrollAmount = Input.mouseScrollDelta.y;
        float controllerScrollAmount = Input.GetAxis("ScrollController");

        // Si se ha movido la rueda del ratón
        if (Mathf.Abs(scrollAmount) > 0 || Mathf.Abs(controllerScrollAmount) > 0)
        {
            if (scrollAmount >= 1 || controllerScrollAmount >= 0.6)
            {
                currentSize -= sensitivity;
                //Debug.Log("Zoomed In");
            }
            else if (scrollAmount <= -1 || controllerScrollAmount <= -0.6)
            {
                currentSize += sensitivity;
                //Debug.Log("Zoomed Out");
            }

            currentSize = Mathf.Clamp(currentSize, minZoom, maxZoom);
            cam.orthographicSize = currentSize;
        }

        // Hacer que la cámara siga a la bola cuando hace zoom
            float zoomPercentage = (currentSize - minZoom) / (maxZoom - minZoom);

        Vector3 ballCamPos = this.transform.position + new Vector3(-8.5f, 16f, 15f);

    if(!loading){
        Vector3 camPos = Vector3.Lerp(ballCamPos, initialCamPos, zoomPercentage);
        cam.transform.position = camPos;
    }else{
        cam.transform.localPosition = Vector3.zero;
        cam.orthographicSize = maxZoom;
    }


        //velocity = rb.velocity;
        //velocityMagnitude = rb.velocity.magnitude;
    }

    public void playSound(string name)
    {
        AudioSource audiosrc = sounds.Find(s => s.name == name).source;
        if (!audiosrc.loop)
        {
            if (audiosrc.isPlaying)
            {
                if (name == "golpe") return;
                audiosrc.Stop();
            }
            audiosrc.volume = sounds.Find(s => s.name == name).volume;
            audiosrc.pitch = sounds.Find(s => s.name == name).pitch;
            audiosrc.Play();
        }
        else
        {
            if (!audiosrc.isPlaying)
            {
                audiosrc.volume = sounds.Find(s => s.name == name).volume;
                audiosrc.pitch = sounds.Find(s => s.name == name).pitch;
                audiosrc.Play();
                sounds.Find(s => s.name == name).currentFade = sounds.Find(s => s.name == name).fade;
            }
            else
            {
                audiosrc.volume = sounds.Find(s => s.name == name).volume;
            }
        }
    }

    public void stopSound(string name)
    {
        AudioSource audiosrc = sounds.Find(s => s.name == name).source;
        audiosrc.Stop();
    }

    public void fadeOutSound(string name)
    {
        AudioSource audiosrc = sounds.Find(s => s.name == name).source;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log(collision.relativeVelocity.magnitude);
        float vel = collision.relativeVelocity.magnitude;
        if (collision.transform.gameObject.name == "SM_tile" || collision.transform.gameObject.name == "SM_Rampa")
        {
            if (vel > 6)
            {
                sounds[1].volume = vel;
                playSound("golpe");
            }
        }
        else
        {
            if (vel > 4)
            {
                sounds[1].volume = vel;
                playSound("golpe");
            }
        }
    }
}
