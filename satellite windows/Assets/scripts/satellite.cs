using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using MathNet.Numerics.Integration;


public class satellite : MonoBehaviour
{
    
    public float a, b;
    public float minHeight, maxHeight;
    public float phi;

    public float orbitAngle; 
    public float orbitalSpeed;
    //public int id;


    float scale = 6371; 
    float c;



    public float speed;
    public float T;
    public float Ttest;

    public float GM;
    public float midleV=0;
    public float L;

     
    public float pow; 
    Vector3 deltaPos;

    LineRenderer lr;
    
    int counter;
    

    private void Start()
    {
         
        lr = GetComponent<LineRenderer>();
         

    }


 

    private void Update()
    {
        transform.LookAt(Vector3.zero);

        CalculateMovement(); 

    }

    void CalculateMovement()
    {
        L = Mathf.PI * (a / scale + b / scale);
        for (int i = 0; i < 360; i++)
        {
            float c = Mathf.Sqrt((a / scale) * (a / scale) - (b / scale) * (b / scale));
            float x = (b / scale) * Mathf.Sin(i * Mathf.Deg2Rad);
            float z = (a / scale) * Mathf.Cos(i * Mathf.Deg2Rad) - c;

            float r_ = new Vector3(x, 0, z).magnitude;
            midleV += (Mathf.Pow(r_, pow));

        }
        midleV /= 360;
        T = (360 / (360 * (speed / midleV))) * (360 * Time.fixedDeltaTime);



        orbitalSpeed = (Mathf.Sqrt(GM / (transform.position.magnitude*scale*1000)));

        if (phi <= 360)
        {
            Ttest += Time.fixedDeltaTime;
        }
        Debug.DrawLine(transform.position, Vector3.zero);

        float r = transform.position.magnitude;
        phi += speed *( orbitalSpeed );
        if (phi > 360) phi = 0;

        calculateOrbit();
    }

    void calculateOrbit()
    {
        if (!Application.isPlaying)
            lr = GetComponent<LineRenderer>();

        if (b > a)
        {
            a = b;
        }

        minHeight = Mathf.Abs((a - c) - scale);
        maxHeight = Mathf.Abs((-a - c) + scale);

        Vector3 lastDotPose = Vector3.zero;
        Vector3 dotPose = new Vector3(0, 0, 1);
        c = Mathf.Sqrt(a * a - b * b);  

        int counter = 0;
        for (float i = 0; i < Mathf.PI * 2; i += Mathf.PI / 36)
        {
            float x = b * Mathf.Sin(i);
            float z = (a * Mathf.Cos(i)) - c;
            //float y = 0;

            float x0 = x * Mathf.Cos(orbitAngle / Mathf.Rad2Deg - 23.5f / Mathf.Rad2Deg);
            float y0 = -x * Mathf.Sin(orbitAngle / Mathf.Rad2Deg - 23.5f / Mathf.Rad2Deg);
            float z0 = z;




            calculateSatellinePos();
            dotPose = new Vector3(x0 / scale, y0 / scale, z0 / scale);
            lr.SetPosition(counter, dotPose); 

            lastDotPose = dotPose;

            counter++;

        }
    }

    private void OnDrawGizmos()
    {/*
        if(!Application.isPlaying)
        lr = GetComponent<LineRenderer>();

        if (b > a)
        { 
            a = b;
        }

        minHeight = Mathf.Abs((a - c)-scale);
        maxHeight = Mathf.Abs((-a - c)+scale);

        Vector3 lastDotPose=Vector3.zero;
        Vector3 dotPose=new Vector3(0,0,1);
        c = Mathf.Sqrt(a * a - b * b) ;

        Gizmos.DrawSphere(new Vector3(0, 0, -2*c ), 0.1f);
        Gizmos.DrawSphere(new Vector3(0, 0, 0), 0.1f);


        int counter = 0;
        for (float i = 0; i < Mathf.PI * 2; i += Mathf.PI / 36)
        {
            float x =b* Mathf.Sin(i);
            float z = (a * Mathf.Cos(i))-c ;
            //float y = 0;

           float x0 = x * Mathf.Cos(orbitAngle/Mathf.Rad2Deg- 23.5f / Mathf.Rad2Deg)  ;
           float y0= -x * Mathf.Sin(orbitAngle / Mathf.Rad2Deg- 23.5f / Mathf.Rad2Deg)  ;
            float z0 = z;




            calculateSatellinePos();
            dotPose = new Vector3(x0/scale, y0/scale,z0/scale );
            lr.SetPosition(counter, dotPose);

            Gizmos.DrawLine(lastDotPose, dotPose);
           

            lastDotPose = dotPose;

            counter++;
            */
        //}
    }


    void calculateSatellinePos()
    {
        float x = (b / scale) * Mathf.Sin(phi/Mathf.Rad2Deg);
        //float y = 0;
        float z = (a / scale) * Mathf.Cos(phi/Mathf.Rad2Deg)-c/scale;


        float x0 = x * Mathf.Cos(orbitAngle / Mathf.Rad2Deg - 23.5f / Mathf.Rad2Deg);
        float y0 = -x * Mathf.Sin(orbitAngle / Mathf.Rad2Deg - 23.5f / Mathf.Rad2Deg);
        float z0 = z;
        transform.position = new Vector3(x0, y0, z0);
    }
}
