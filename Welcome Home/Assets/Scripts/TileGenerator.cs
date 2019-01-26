using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int width;
    public int height;
    public float tileWidth = 1.0f;
    public float tileHeight = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [ContextMenu("Generate")]
    public void Generate()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject go = Instantiate(tilePrefab, this.transform);
                go.transform.localPosition = new Vector3(i, 0, j);
            }
        }
    }
}
