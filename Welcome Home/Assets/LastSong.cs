using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LastSong : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.callFadeIn("nivel1");
    }


}
