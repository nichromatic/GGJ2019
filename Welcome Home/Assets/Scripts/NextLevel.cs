using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevel : MonoBehaviour
{
    public float time;
    public int sceneNumber;
    Camera cam;
    BallController ball;

    public void Start()
    {
        cam = FindObjectOfType<Camera>();
        ball = FindObjectOfType<BallController>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            cam.transform.parent.GetComponent<Animator>().SetTrigger("End");
            ball.loading = true;
            StartCoroutine(NextScene());
            AudioManager.Instance.callFadeOut();
        }
    }
    
    public IEnumerator NextScene()
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
