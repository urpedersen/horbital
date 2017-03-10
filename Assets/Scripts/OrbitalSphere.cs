using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalSphere : MonoBehaviour {

    private Renderer m_renderer;

    [SerializeField]
    private float m_maxRadius = 1.0f;
    [SerializeField]
    private float m_step = 1.0f;

    [SerializeField]
    private int m_QuantumNumberN = 1;
    [SerializeField]
    private int m_QuantumNumberL = 1;
    [SerializeField]
    private int m_QuantumNumberM = 1;

    // Use this for initialization
    void Start () {
        m_renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update () {

        // Get coordinates and move a bit
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;
        transform.position = new Vector3(x+Random.Range(-m_step,m_step), y+Random.Range(-m_step, m_step), z+Random.Range(-m_step, m_step));

        // Compute spherical coordinates
        float distance = Mathf.Sqrt(x * x + y * y + z * z);

        // Swop particle back if it move to far away
        if (distance > m_maxRadius)
            transform.position = new Vector3(0, 0, 0);

        // Change alpha based on wave function
        float waveFunction = RadialWaveFunction(m_QuantumNumberN, m_QuantumNumberL, distance);
        m_renderer.material.color = new Color(1.0f,0.1f,0.2f,0.5f);
        Debug.Log(waveFunction);
    }

    private float RadialWaveFunction(int n,int l,float r)
    {
        // TODO ensure that input is an allowed wave function
        float result = Mathf.Pow(2f/n,3);
        //result *= Factorial(n - l - 1)/2*n;
        //float tmp = Factorial(n + l);
        //result /= tmp * tmp * tmp;
        //result = Mathf.Sqrt(result);
        result *= Mathf.Exp(-r / (float)n);
        result *= Mathf.Pow(2 * r / n, l);
        //result *= LaguerrePolynomials(2*l+1,n-l-1,2*r/n);

        return result;
    }

    private float LaguerrePolynomials(int up,int low,float x)
    {
        float result = 0f;
        if (up == 0)
        {
            if(low == 0)
            {
                result = 1f;
            }else if (low == 1)
            {
                result = -x + 1;
            }
            else if (low == 2)
            {
                result = x*x-4f*x+2;
            }
        }else if (up == 1)
        {
            if(low == 0)
            {
                result = 1f;
            } else if (low == 1)
            {
                result = -2 * x + 4;
            } else if (low == 2)
            {
                result = 3 * x * x - 18 * x + 18;
            }
        } else if (up == 2)
        {
            if(low == 0)
            {
                result = 2f;
            } else if (low == 1)
            {
                result = -6 * x + 18;
            } else if (low == 2)
            {
                result = 12 * x * x - 96 * x + 144;
            } 
        } else if (up == 3)
        {
            if ( low == 0)
            {
                result = 6;
            } else if (low == 1)
            {
                result = -24 * x + 96;
            } else if (low == 2)
            {
                result = 60 * x * x - 600 * x + 1200;
            }
        }
        return result;
    }


    private int Factorial(int n)
    {
        int result = 1;
        for(int i = 1; i < n+1; i++)
        {
            result *= i;
        }
        return result;
        /*if (n >= 2) return n * Factorial(n - 1);
        return 1;*/
        
    }
}
