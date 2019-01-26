using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToSpawn : MonoBehaviour
{
    Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = GameObject.FindGameObjectWithTag("Player").transform.position;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.transform.position = spawnPoint;
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
        }
    }
}
