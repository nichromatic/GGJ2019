using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Spline sp;

    public float speed;
    public float current;

    public bool start;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        start = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            current = Mathf.Clamp(current += speed * Time.deltaTime, 0, 1);
            iTween.PutOnPath(this.transform, sp.path, current);
            transform.LookAt(iTween.PointOnPath(sp.path, current + 0.001f));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Domino"))
        {
            rb.constraints = RigidbodyConstraints.None;
            start = true;
        }
    }
}
