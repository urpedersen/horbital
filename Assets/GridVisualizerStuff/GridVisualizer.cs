using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridVisualizer : MonoBehaviour {

    [SerializeField]
    private GridCube gridCubePrefab;
    //public GameObject gridCubePrefab;

    private List<GridCube> gridCubes = new List<GridCube>();
    int numCubes;


    public int xResolution = 5;
    public int yResolution = 5;
    public int zResolution = 5;

    public float maxX = 1;
    public float maxY = 1;
    public float maxZ = 1;


    //public Vector3 resolution = new Vector3(10f, 10f, 10f);

    // Use this for initialization
    void Start()
    {
        numCubes = xResolution * yResolution * zResolution;

        


        Instantiate(gridCubePrefab, new Vector3(0,0,0), Quaternion.identity);
        //for (int x = 0; x < (int) resolution.x; x++)
        //{

        //    for (int y = 0; x < (int) resolution.y; y++)
        //    {

        //        for (int z = 0; x < (int) resolution.z; z++)
        //        {
        //            Instantiate(gridCubePrefab, new Vector3(x, y, z), Quaternion.identity);
        //        }
        //    }
        //}
        for (int x = -xResolution; x < xResolution; x++)
        {
            float xPos = ((float)x / xResolution);
            for (int y = -yResolution; y < yResolution; y++)
            {
                float yPos = ((float)y / yResolution);
                for (int z = -zResolution; z < zResolution; z++)
                {
                    float zPos = ((float)z / zResolution);

                    GridCube temp = (GridCube)  Instantiate(gridCubePrefab, new Vector3(xPos, yPos, zPos), Quaternion.identity);
                    temp.transform.localScale = new Vector3(1f / (float)xResolution, 1f / (float)yResolution, 1f / (float)zResolution);
                    temp.cubeColor = new Color(1f, 1f, 1f, Random.Range(0f, 1f));
                    temp.xPos = x;
                    temp.yPos = y;
                    temp.zPos = z;
                    //temp.alpha = Random.Range(0f, 1f);
                    //temp.alpha = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(xPos, yPos, zPos));
                    float r = Vector3.Distance(new Vector3(0, 0, 0), new Vector3(xPos, yPos, zPos));
                    float a = 0.5f;
                    //temp.alpha = 1-r;
                    //temp.alpha = 2 * Mathf.Pow(a, -3f / 2f) * Mathf.Exp(-r / a);
                    temp.alpha = Mathf.Abs(1/Mathf.Sqrt(2) * Mathf.Pow(a, -3f / 2f) *(1-0.5f*r/a)* Mathf.Exp(-r / 2*a));
                    //gridCubes.Add(temp);
                    


                }
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

    //float DistToOrigin(Vector3 vec)
    //{
    //    float distance = Mathf.Sqrt(Mathf.Pow(vec.x, 2) + Mathf.Pow(vec.y, 2) + Mathf.Pow(vec.z, 2));
    //    return distance;
    //}
}
