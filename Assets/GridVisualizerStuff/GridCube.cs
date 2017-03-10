using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCube : MonoBehaviour
{

    public int xPos;
    public int yPos;
    public int zPos;

    public float alpha;
    private Renderer rend;

    public Color cubeColor = Color.blue;

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();


    }

    // Update is called once per frame
    void Update()
    {
        cubeColor.a = alpha;
        rend.material.color = cubeColor;

    }
}
