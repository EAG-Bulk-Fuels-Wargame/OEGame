using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LineScript : MonoBehaviour
{
    public float[] c;
    private Renderer rend;
    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
    }
    void Update()
    {
        rend.materials[0].mainTextureScale = new Vector3(c[0], c[1], c[2]);
    }
}