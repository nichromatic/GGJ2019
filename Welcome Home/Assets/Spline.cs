using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    public Transform[] path;

    // Start is called befzore the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255,0,0,1);
        iTween.DrawPathHandles(path);
        //iTween.DrawPathHandles(path);
    }


}
