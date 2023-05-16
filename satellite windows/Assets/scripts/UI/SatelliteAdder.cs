using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SatelliteAdder : MonoBehaviour
{
    public TMP_InputField satName;
    public GameObject rayEditor;

    public Slider bigSemiAxis, smallSemiAxis,orbitRotation;
    public TMP_Text bigsemText, smallTemText, orbitText;

    public Slider rayRingCount,rayAngle;
    public TMP_Text rayRingValueText, rayAngleValueText;

    public GameObject hexagon;
    public GameObject satelliteObj;

    GameObject[] rays;
    bool checkEditingRays;
    GameManager gm;
    satellite editingSattelite;
    float t;

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void  Update()
    {
        if (editingSattelite != null)
            EditingSatellite();
    }

    void EditingSatellite()
    {
        if(!checkEditingRays)
        editOrbit();
        else
        {
            editRays();
        }
    }

    public void backToSat()
    {
        rayEditor.SetActive(false);
        checkEditingRays = false;
        t = 0;
    }

    public void ApplySat()
    {
       
        gm.satellitesCount++;
        rayEditor.SetActive(false);
        checkEditingRays = false;

        gm.matMan.matrixObject.Add(editingSattelite.gameObject);
        gm.cr.transform.position = Vector3.zero;
        gm.cr.isEditing = false;
        editingSattelite.speed = 0.00001f;
        editingSattelite = null;
       TMPro.TMP_Text newName=  Instantiate(gm.matMan.names[0].gameObject).GetComponent<TMP_Text>();
        newName.transform.parent = gm.matMan.names[0].transform.parent;
        gm.matMan.names.Add(newName);

        gameObject.SetActive(false);

    }

    void editRays()
    {
        float SatHeight = editingSattelite.transform.position.magnitude;
         
        float radius= SatHeight * Mathf.Tan(rayAngle.value / 2 * Mathf.Deg2Rad);
        float heightCone = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(radius / 2f, 2));
        int raysCount = 1;

        rayAngleValueText.text = rayAngle.value.ToString();

        rayRingValueText.text = rayRingCount.value.ToString();

        for(int i = 0; i < editingSattelite.transform.GetChild(0).childCount;i++)
        {
            editingSattelite.transform.GetChild(0).GetChild(i).localScale = new Vector3(radius,SatHeight,radius);
        }

        int res = 0;
        for (int i = 0; i < rayRingCount.value+1; i++)
        {
            
            res += 6 * i;

             
        }
        raysCount = res+1;
        Debug.Log(raysCount);

        if (rayRingCount.value != 0 && editingSattelite.transform.GetChild(0).childCount < raysCount)
        { 

            int vertex = (int)rayRingCount.value ;


            for (int j = 0; j < 360; j += (int)(60 / rayRingCount.value))
            {
                 
                GameObject newRay = Instantiate(editingSattelite.transform.GetChild(0).GetChild(0).gameObject);
                newRay.transform.parent = editingSattelite.transform.GetChild(0);
                newRay.transform.localPosition = Vector3.zero;


                newRay.transform.localScale = new Vector3(radius, SatHeight, radius);

                float alfa0 = alfa(Mathf.Sqrt(radius * radius - (radius / 2f) * (radius / 2f)), SatHeight, vertex);
                

                newRay.transform.localRotation = Quaternion.Euler(0, j, alfa0 * (rayRingCount.value));

                if (vertex <= 0) vertex = ((int)rayRingCount.value );
                if(rayRingCount.value!=1)
                vertex--;
            }

        } else if (editingSattelite.transform.GetChild(0).childCount > raysCount)
        {

            int chC = editingSattelite.transform.GetChild(0).childCount;

            for (int i= raysCount; i< chC; i++)
            {
                Destroy(editingSattelite.transform.GetChild(0).GetChild(i).gameObject);
            }
        }

        gm.cr.transform.position = Vector3.Lerp(gm.cr.transform.position, new Vector3(0,
         0, editingSattelite.transform.position.z), 0.05f);

        gm.cr.transform.localRotation = Quaternion.Lerp(gm.cr.transform.localRotation, Quaternion.Euler(270, 87.5f, 66.5f), 0.05f);

        t += Time.deltaTime;
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 2,t);

    }

    float alfa(float radius,float height,int edge)
    {
      

        if (edge <= 0)
        {
            
            return  Mathf.Asin((radius ) / Mathf.Sqrt(height * height + (radius ) * (radius ))) * Mathf.Rad2Deg*2;
        }
        else
        {
            float hexagonHeight = radius * 0.866025f;
            
            return   Mathf.Asin((hexagonHeight ) / Mathf.Sqrt(height * height + (hexagonHeight ) * (hexagonHeight ))) * Mathf.Rad2Deg*2;
            
        }
    }

    public void StartEditRays()
    {
        checkEditingRays = true;
        rayEditor.SetActive(true);

    }

    void editOrbit()
    {
        if (smallSemiAxis.value + 1 > bigSemiAxis.value)
        {
            bigSemiAxis.value = smallSemiAxis.value;
        }

        gm.cr.isEditing = true;

        gm.cr.transform.position = Vector3.Lerp(gm.cr.transform.position, new Vector3(editingSattelite.transform.position.z / 6,
            editingSattelite.transform.position.z + 1, editingSattelite.transform.position.z / 4), 0.05f);
        gm.cr.transform.localRotation = Quaternion.Lerp(gm.cr.transform.localRotation, Quaternion.Euler(90, 0, 0), 0.05f);

        if (satName.text.Length > 0)
            editingSattelite.name = satName.text;
        else editingSattelite.name = "спутник " + gm.satellitesCount;
        editingSattelite.a = bigSemiAxis.value;
        bigsemText.text = bigSemiAxis.value.ToString();

        editingSattelite.b = smallSemiAxis.value;
        smallTemText.text = smallSemiAxis.value.ToString();

        editingSattelite.orbitAngle = orbitRotation.value;
        orbitText.text = orbitRotation.value.ToString();
    }

    public void addSatellite()
    {
        gameObject.SetActive(true);
       GameObject newSat= Instantiate(satelliteObj) ;
        newSat.transform.parent = gm.satelliteHolder.transform;
        editingSattelite = newSat.GetComponent<satellite>();

    }



}
