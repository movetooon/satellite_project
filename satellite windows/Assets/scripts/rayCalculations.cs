using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class rayCalculations : MonoBehaviour
{

    public float height; 
    List<Vector3> earthGirdPoints = new List<Vector3>();
    public float dotScale=0.05f;
    public float rayArea;
    public int pointsScaler;
    public Collider virtualRay;

    public float min;
    public LayerMask rayMask;


    private void Start()
    {
        GeneratePoints();
    }

    void GeneratePoints()
    {
        for (float o = 0; o <= 180; o += pointsScaler)
        {
            for (float phi = 0; phi <= 180; phi += pointsScaler)
            {
                Vector3 dotPos = new Vector3(Mathf.Sin(o * Mathf.Deg2Rad) * Mathf.Cos(phi * Mathf.Deg2Rad),
                    (Mathf.Sin(o * Mathf.Deg2Rad) * Mathf.Sin(phi * Mathf.Deg2Rad)/1.00326f), Mathf.Cos(o * Mathf.Deg2Rad));
                 
                if(dotPos!=Vector3.zero) earthGirdPoints.Add(dotPos);

            }
        }
    }


    private void OnDrawGizmos()
    {
        /*
        for (int i=0;i<earthGirdPoints.Count;i++)
        { 
            DrawEarthGird(earthGirdPoints[i]);
            
        }
        */
    }


    private void FixedUpdate()
    {
        //rayArea = CalculateRayArea(rayColl);
    }

    public float CalculateRayArea(Collider[] rayCol)
    {
         
        float sum=0;
        Vector3[] areaPoints = new Vector3[earthGirdPoints.Count+1];
        

        for (int i = 0; i < earthGirdPoints.Count; i++)
        {
            for (int j = 0; j < rayCol.Length; j++)
            {
                virtualRay.transform.localScale = rayCol[j].transform.parent.transform.localScale;
                virtualRay.transform.position =  new Vector3(0, rayCol[j].transform.parent.transform.position.magnitude,0);
                virtualRay.transform.rotation = rayCol[j].transform.localRotation;

                height = rayCol[j].transform.parent.transform.position.magnitude+1;

                if (checkIfInsideCollider(earthGirdPoints[i], virtualRay))
                {
                    areaPoints[i] = earthGirdPoints[i];
                }
                else
                {
                    areaPoints[i] = Vector3.zero;

                }
            }
        }
         

          for (int i = 1; i <  earthGirdPoints.Count-180/ pointsScaler; i++)
          {
            bool yZero = areaPoints[i].y <min && areaPoints[i + 1].y < min && areaPoints[i + 180 / pointsScaler].y < min;


            if (!yZero&&areaPoints[i]!=Vector3.zero&&areaPoints[i+1]!=Vector3.zero && areaPoints[i + 180 / pointsScaler ]!=Vector3.zero)
                sum += 2* MAthfutilits.GeronArea(areaPoints[i ], areaPoints[i+1], areaPoints[i + 180 / pointsScaler +1]);  
          }


        return sum;
        
         
    }

   

    void DrawEarthGird(Vector3 point,Collider col)
    { 
        if (checkIfInsideCollider(point, col))
        {
            Gizmos.color = Color.green;
            Gizmos.DrawCube(point, new Vector3(dotScale, dotScale, dotScale));
        }
        else
        {
            Gizmos.color = new Color(1,0,0,0.01f);
            Gizmos.DrawCube(point, new Vector3(dotScale, dotScale, dotScale));
        }
    }

    bool checkIfInsideCollider(Vector3 point,Collider col)
    {
        float minHeight = Mathf.Sqrt(1/((height-1)*(height-1)));  
        if ((col.bounds.Contains(point)&& Physics.CheckSphere(point, 0.01f,rayMask)) &&point.y>minHeight)
        { 
            return true;
        } 
        else return false;
    }
}
