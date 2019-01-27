using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene2D : MonoBehaviour
{
    public float minTime;
    float currentMinTime;
    public float maxTime;
    public float currentMaxTime;
    bool canClick;
    public int sceneToLoad;
    public string levelMusic;
    Coroutine loadWait;
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.callFadeIn(levelMusic);
        currentMinTime = minTime;
        currentMaxTime = maxTime;
        cam = FindObjectOfType<Camera>();
        loadWait = StartCoroutine(WaitIdle());
    }

    IEnumerator WaitIdle()
    {
        yield return new WaitForSeconds(minTime);
        canClick = true;
        yield return new WaitForSeconds(maxTime);
        canClick = false;
        cam.GetComponent<Animator>().SetTrigger("End");
        StartCoroutine(NextScene());
    }

    public IEnumerator NextScene()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(sceneToLoad);
    }

    // Update is called once per frame
    void Update()
    {
        if(canClick && Input.GetMouseButtonDown(0))
        {
            StopCoroutine(loadWait);
            cam.GetComponent<Animator>().SetTrigger("End");
            StartCoroutine(NextScene());
        }
    }
}
