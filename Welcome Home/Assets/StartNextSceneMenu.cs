using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartNextSceneMenu : MonoBehaviour
{
    ParticleSystem ps;
    bool clicked;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        ps = GetComponent<ParticleSystem>();
        ps.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {
        if (!clicked)
        {
            AudioManager.Instance.playSound("fuego");
            StartCoroutine(NextScene());
            clicked = true;
        }
    }

    public IEnumerator NextScene()
    {
        ps.Play();
        yield return new WaitForSeconds(3f);
        cam.GetComponent<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
