using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [Header("References")]
    Rigidbody rb;
    Camera cam;

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
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        initialCamPos = cam.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        // Comprobamos si la bola está en el suelo ahora mismo
        Ray floorRay = new Ray(this.transform.position, new Vector3(0, -1, 0));
        if (Physics.Raycast(floorRay, 0.35f)) {
            onFloor = true;
        } else
        {
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
                    rb.AddForce(-dir * forceMagnitude * Time.deltaTime * velocityExtra, ForceMode.Acceleration);
                }
            }
        }

        // Mouse wheel zoom
        float currentSize = cam.orthographicSize;
        float scrollAmount = Input.mouseScrollDelta.y;

        // Si se ha movido la rueda del ratón
        if (Mathf.Abs(scrollAmount) > 0)
        {
            if (scrollAmount >= 1)
            {
                currentSize -= sensitivity;
                //Debug.Log("Zoomed In");
            }
            else if (scrollAmount <= -1)
            {
                currentSize += sensitivity;
                //Debug.Log("Zoomed Out");
            }

            currentSize = Mathf.Clamp(currentSize, minZoom, maxZoom);
            cam.orthographicSize = currentSize; 
        }

        // Hacer que la cámara siga a la bola cuando hace zoom
        float zoomPercentage = (currentSize - minZoom) / (maxZoom - minZoom);
        //Debug.Log(zoomPercentage);

        Vector3 ballCamPos = this.transform.position + new Vector3(-8.5f, 16f, 15f); ;

        Vector3 camPos = Vector3.Lerp(ballCamPos, initialCamPos, zoomPercentage);
        cam.transform.position = camPos;

        //velocity = rb.velocity;
        //velocityMagnitude = rb.velocity.magnitude;
    }
}
