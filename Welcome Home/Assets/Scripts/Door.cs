using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        anim.SetTrigger(Constantes.SIMON_DOOR_OPEN);
    }
}
