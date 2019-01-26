using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    Rigidbody rb;
    Camera cam;

    public float maxForce;
    public float minForce;

    //public Vector3 velocity;
    //public float velocityMagnitude;

    public bool onFloor = false;

    public float maxVelocity;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
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

        //velocity = rb.velocity;
        //velocityMagnitude = rb.velocity.magnitude;
    }
}
