using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    SimonDice simon;
    // Start is called before the first frame update
    public void Init(SimonDice controller)
    {
        simon = controller;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            simon.Restart();
            Destroy(this.gameObject);
        }
    }
}
