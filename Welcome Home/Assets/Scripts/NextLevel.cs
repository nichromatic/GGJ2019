using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public float time;
    public int sceneNumber;
    Camera cam;
    public string song;

    public void Start()
    {
        cam = FindObjectOfType<Camera>();
        AudioManager.Instance.callFadeIn(song);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cam.GetComponent<Animator>().SetTrigger("End");
            StartCoroutine(NextScene());
            AudioManager.Instance.callFadeOut();
        }
    }
    
    public IEnumerator NextScene()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(sceneNumber);
    }
}
