using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    [SerializeField]
    private GameObject particle;
    private GameObject[] particles;


    [SerializeField]
    private int numberOfParticles;

    // Use this for initialization
    void Start () {
		for(int i=0;i < numberOfParticles; i++)
        {
            GameObject copy = Instantiate<GameObject>(particle);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
