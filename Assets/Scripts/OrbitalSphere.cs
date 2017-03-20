using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitalSphere : MonoBehaviour {

    private Renderer m_renderer;

    [SerializeField]
    private float m_maxRadius = 3.0f;
    [SerializeField]
    private float m_step = 0.05f;

    [SerializeField]
    private int m_QuantumNumberN = 2;
    [SerializeField]
    private int m_QuantumNumberL = 1;
    [SerializeField]
    private int m_QuantumNumberM = 0;

    [SerializeField]
    private float m_maxWavefunction = 3f;
    [SerializeField]
    private float m_minWavefunction = 0.5f;


    // Enables the programmer to se the value of the wave-function in Unity
    public float Output_wavefunction;
    public float Output_wavefunctionAngle;

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
        float waveFunctionNorm = wavefunctionNorm();
        float waveFunctionAngle = wavefunctionAngle();
        Output_wavefunction = waveFunctionNorm;
        Output_wavefunctionAngle = waveFunctionAngle;

        // Change alpha
        float alpha = numberToAlpha(waveFunctionNorm, m_minWavefunction, m_maxWavefunction);
        while (alpha < 0.001f)  // Find a location where the particle is not invisible
        {
            transform.position = new Vector3(Random.Range(-m_maxRadius, m_maxRadius), Random.Range(-m_maxRadius, m_maxRadius), Random.Range(-m_maxRadius, m_maxRadius));
            waveFunctionNorm = RadialWaveFunction()* AngularWaveFunctionReal();
            alpha = numberToAlpha(waveFunctionNorm, m_minWavefunction, m_maxWavefunction);
        };

        // Change color of wavefunction
        if (waveFunctionNorm > 0f)
        {
            m_renderer.material.color = new Color(1.0f, 0.0f, 0.0f, alpha);
        } else
        {
            m_renderer.material.color = new Color(0.0f, 0.0f, 1.0f, alpha);
        }
        //Debug.Log(waveFunction);



        // Swop particle back if it move to far away
        if (r > m_maxRadius)
            transform.position = new Vector3(
                Random.Range(-m_maxRadius, m_maxRadius), 
                Random.Range(-m_maxRadius, m_maxRadius), 
                Random.Range(-m_maxRadius, m_maxRadius)
            );
    }

    private float numberToAlpha(float input,float min,float max)
    {
        float alpha = (input*input - min) / (max-min);
        if (alpha < 0f)
        {
            alpha = 0f;
        } else if (alpha > 1f)
        {
            alpha = 1.0f;
        }
        return alpha;
    }

    private float wavefunctionNorm()
    {
        return RadialWaveFunction()*AngularWaveFunctionPrefactor();
    }

    private float wavefunctionAngle()
    {
        float angle = (float)m_QuantumNumberM * Mathf.Atan2(this.transform.position.y, this.transform.position.x);
        float pre = wavefunctionNorm();
        if (wavefunctionNorm() < 0f)
            angle -= Mathf.PI;
        return angle;
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
        float result = Mathf.Pow(2f/n,3f);
        result *= (float)Factorial(n - l - 1)/2f*n;
        float tmp = (float)Factorial(n + l);
        result /= tmp * tmp * tmp;
        result = Mathf.Sqrt(result);
        result *= Mathf.Exp(-r / (float)n);
        result *= Mathf.Pow(2f * r / n, l);
        result *= AssociatedLaguerrePolynomials(2*l+1,n-l-1,2f*r/n);

        return result;
    }

    private float AngularWaveFunctionReal()
    {
        int m = m_QuantumNumberM;

        float x = this.transform.position.x;
        float y = this.transform.position.y;

        // Compute spherical coordinates
        float phi = Mathf.Atan2(y, x);

        return AngularWaveFunctionPrefactor() * Mathf.Cos((float)m * phi);
    }

    private float AngularWaveFunctionImag()
    {
        int m = m_QuantumNumberM;

        float x = this.transform.position.x;
        float y = this.transform.position.y;

        // Compute spherical coordinates
        float phi = Mathf.Atan2(y, x);

        return AngularWaveFunctionPrefactor() * Mathf.Sin((float)m * phi);
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

        float output =1f/(4f*Mathf.PI);
        output *= (float)Factorial(l + m);
        output /= (2f * l + 1f) * (float)Factorial(l - m);
        output = Mathf.Pow(-1f, m) / Mathf.Sqrt( output );
        output *= AssociatedLegendrePolynomials() ;

        return output;
    }

    // TODO Negative m's are missing
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
                result = -Mathf.Sqrt(1f - arg * arg);
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
                result = -3f / 2f * (5f * arg * arg - 1f) * Mathf.Sqrt(1f - arg * arg);
            } else if (m == 0)
            {
                result = 0.5f * (5f * arg * arg * arg - 3f * arg);
            }
        } else
        {
            Debug.LogError("AssociatedLegendrePolynomials() does not take so large aguments");
        }

        return result;
    }


    private float AssociatedLaguerrePolynomials(int up,int low,float x)
    {
        float result = 0f;
        if (up == 0)
        {
            if(low == 0)
            {
                result = 1f;
            }else if (low == 1)
            {
                result = -x + 1f;
            }
            else if (low == 2)
            {
                result = x*x-4f*x+2f;
            }
        }else if (up == 1)
        {
            if(low == 0)
            {
                result = 1f;
            } else if (low == 1)
            {
                result = -2f * x + 4f;
            } else if (low == 2)
            {
                result = 3f * x * x - 18f * x + 18f;
            }
        } else if (up == 2)
        {
            if(low == 0)
            {
                result = 2f;
            } else if (low == 1)
            {
                result = -6f * x + 18f;
            } else if (low == 2)
            {
                result = 12f * x * x - 96f * x + 144f;
            } 
        } else if (up == 3)
        {
            if ( low == 0)
            {
                result = 6f;
            } else if (low == 1)
            {
                result = -24f * x + 96f;
            } else if (low == 2)
            {
                result = 60f * x * x - 600f * x + 1200f;
            }
        }
        else
        {
            Debug.LogError("AssociatedLaguerrePolynomials() does not take so large aguments");
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
