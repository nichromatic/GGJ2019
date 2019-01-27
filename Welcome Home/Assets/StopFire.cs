using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFire : MonoBehaviour
{
    public void Start()
    {
        AudioManager.Instance.stopSound("fuego");
    }
}
