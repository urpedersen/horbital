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

    // Enables the programmer to se the value of the wave-function in Unity
    public float Output_wavefunction;

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
        float r = Mathf.Sqrt(x * x + y * y + z * z);
        transform.position = new Vector3(x+Random.Range(-m_step,m_step), y+Random.Range(-m_step, m_step), z+Random.Range(-m_step, m_step));

        // Compute wave function
        float waveFunction = RadialWaveFunction() * AngularWaveFunctionPrefactor();

        // Change alpha
        float alpha = numberToAlpha(waveFunction);
        if (alpha < 0.001f) {
            while (alpha < 0.001f)  // Find a location where the particle is not invisible
            {
                transform.position = new Vector3(Random.Range(-m_maxRadius, m_maxRadius), Random.Range(-m_maxRadius, m_maxRadius), Random.Range(-m_maxRadius, m_maxRadius));
                waveFunction = RadialWaveFunction()*AngularWaveFunctionPrefactor();
                alpha = numberToAlpha(waveFunction);
            }
        };


        m_renderer.material.color = new Color(1.0f,0.0f,0.0f, alpha);
        //Debug.Log(waveFunction);

        Output_wavefunction = alpha;

        // Swop particle back if it move to far away
        if (r > m_maxRadius)
            transform.position = new Vector3(Random.Range(-m_maxRadius, m_maxRadius), Random.Range(-m_maxRadius, m_maxRadius), Random.Range(-m_maxRadius, m_maxRadius));

    }

    private float numberToAlpha(float input)
    {
        float alpha = (input*input - 0.5f) / 2.0f;
        if (alpha < 0f)
        {
            alpha = 0f;
        } else if (alpha > 1f)
        {
            alpha = 1.0f;
        }
        return alpha;
    }

    private float RadialWaveFunction()
    {
        int n = m_QuantumNumberN;
        int l = m_QuantumNumberL;
        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;
        float r = Mathf.Sqrt(x * x + y * y + z * z);


        // TODO ensure that input is an allowed wave function
        float result = Mathf.Pow(2f/n,3);
        result *= (float)Factorial(n - l - 1)/2*n;
        float tmp = (float)Factorial(n + l);
        result /= tmp * tmp * tmp;
        result = Mathf.Sqrt(result);
        result *= Mathf.Exp(-r / (float)n);
        result *= Mathf.Pow(2 * r / n, l);
        result *= LaguerrePolynomials(2*l+1,n-l-1,2*r/n);

        return result;
    }

    private float AngularWaveFunctionReal()
    {
        int m = m_QuantumNumberM;

        float x = this.transform.position.x;
        float y = this.transform.position.y;

        // Compute spherical coordinates
        float phi = Mathf.Atan2(y, x);

        return AngularWaveFunctionPrefactor() * Mathf.Cos(m * phi);
    }

    private float AngularWaveFunctionImag()
    {
        int m = m_QuantumNumberM;

        float x = this.transform.position.x;
        float y = this.transform.position.y;

        // Compute spherical coordinates
        float phi = Mathf.Atan2(y, x);

        return AngularWaveFunctionPrefactor() * Mathf.Sin(m * phi);
    }

    // The anglar 
    private float AngularWaveFunctionPrefactor()
    {
        int l = m_QuantumNumberL;
        int m = m_QuantumNumberM;

        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;
        float r = Mathf.Sqrt(x * x + y * y + z * z);

        // Compute spherical coordinates
        //float theta = Mathf.Acos(z / r);
        //float phi = Mathf.Atan2(y, x);

        float output =1/(4*Mathf.PI);
        output *= (float)Factorial(l + m);
        output /= (2 * l + 1) * (float)Factorial(l - m);
        output = Mathf.Pow(-1, m) / Mathf.Sqrt( output );
        output *= AssociatedLegendrePolynomials() ;

        return output;
    }

    private float AssociatedLegendrePolynomials()
    {
        int l = m_QuantumNumberL;
        int m = m_QuantumNumberM;

        float x = this.transform.position.x;
        float y = this.transform.position.y;
        float z = this.transform.position.z;
        float r = Mathf.Sqrt(x * x + y * y + z * z);

        // Compute spherical coordinates
        float theta = Mathf.Acos(z / r);

        float arg = Mathf.Cos(theta);   // The argument to the polynomial
        float result = 1.0f;    // l=0 gives 1

        if (l == 1)
        {
            if (m == 1)
            {
                result = -Mathf.Sqrt(1 - arg * arg);
            }
            else if (m == 0)
            {
                result = arg;
            }
        }
        else if (l == 2)
        {
            if (m == 2)
            {
                result = 3f * (1f - arg * arg);
            }
            else if (m == 1)
            {
                result = -3f * arg * Mathf.Sqrt(1f - arg * arg);
            }
            else if(m == 0)
            {
                result = 0.5f * (3f * arg * arg - 1f);
            }
        }
        else if (l == 3)
        {
            if (m == 3)
            {
                result = -15f * Mathf.Pow(Mathf.Sqrt(1f - arg * arg), 3.0f);
            }
            else if (m == 2)
            {
                result = 15f * arg * (1f - arg * arg);
            } else if (m == 1)
            {
                result = -3f / 2f * (5f * arg * arg - 1) * Mathf.Sqrt(1f - arg * arg);
            } else if (m == 0)
            {
                result = 0.5f * (5f * arg * arg * arg - 3f * arg);
            }
        }

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
    }
}
