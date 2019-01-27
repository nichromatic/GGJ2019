using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayDelay : MonoBehaviour
{
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        StartCoroutine(playDelay());
    }

    public IEnumerator playDelay()
    {
        yield return new WaitForSeconds(2f);
        audio.Play();
        yield return new WaitForSeconds(2f);
        AudioManager.Instance.callFadeIn("nivel1");
        AudioManager.Instance.playSound("fuego");
    }

}
