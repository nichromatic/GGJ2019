using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonButton : MonoBehaviour
{
    int timesActive = 0;
    WaitForSeconds wait;
    public float timeBetweenPresses;
    public bool canPress = false;
    public Animator anim;
    public SimonDice simon;
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        timesActive = 0;
        wait = new WaitForSeconds(timeBetweenPresses);
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    public void Init(SimonDice controller)
    {
        simon = controller;
        timesActive = 0;
        canPress = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canPress)
        {
            StartCoroutine(Press());
            audioSource.Play();
        }
    }

    public SimonButton UseOnce()
    {
        timesActive++;
        anim.SetTrigger(Constantes.SIMON_BUTTON_ACTIVE);
        audioSource.Play();
        return this;
    }

    public void Activate(bool state)
    {
        canPress = state;
        if (canPress)
        {
            anim.SetTrigger("IdleActive");
        }
        else
        {
            anim.SetTrigger("IdleInactive");
        }
    }

    public IEnumerator Press()
    {
        canPress = false;
        anim.SetTrigger(Constantes.SIMON_BUTTON_USE);
        timesActive--;
        simon.CompareButton(this);
        yield return wait;
        canPress = true;
    }
}
