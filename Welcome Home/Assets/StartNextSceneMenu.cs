using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartNextSceneMenu : MonoBehaviour
{
    ParticleSystem ps;
    bool clicked;
    Camera cam;
    public Image image;
    public Sprite newSprite;
    // Start is called before the first frame update
    void Start()
    {
        cam = FindObjectOfType<Camera>();
        ps = GetComponent<ParticleSystem>();
        ps.Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
    }


    public void OnMouseDown()
    {
        if (!clicked)
        {
            AudioManager.Instance.playSound("fuego");
            image.sprite = newSprite;
            StartCoroutine(NextScene());
            clicked = true;
        }
    }

    public void Update(){

        if (!clicked && Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            AudioManager.Instance.playSound("fuego");
            image.sprite = newSprite;
            StartCoroutine(NextScene());
            clicked = true;
        }
    }

    public IEnumerator NextScene()
    {
        ps.Play();
        yield return new WaitForSeconds(3f);
        cam.GetComponent<Animator>().SetTrigger("End");
        yield return new WaitForSeconds(0.75f);
        ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        yield return new WaitForSeconds(1.25f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
