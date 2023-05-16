using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Globalization;
using System.Xml;


public class GameManager : MonoBehaviour
{
    public int satellitesCount, stationsCount;
    [System.NonSerialized]
    public matrixManager matMan;
    public GameObject satelliteHolder, stationHolder;
    public cameraRotator cr;
    public LayerMask objMask;

    bool showSat,showStation; 
    satellite showingSat;
    UIManager uiMan;

    private void Awake()
    {
        uiMan = FindObjectOfType<UIManager>();
        cr = FindObjectOfType<cameraRotator>();
        matMan = FindObjectOfType<matrixManager>();
    }

    private void  Update()
    {
        Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        RaycastHit hit;
        Debug.DrawRay(Vector3.zero, Camera.main.ScreenPointToRay(Input.mousePosition).direction);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Physics.Raycast(r, out hit ,objMask) )
        {
            Debug.Log("hit " + hit.transform.gameObject.name);
            if (Input.GetMouseButtonDown(0)&&hit.transform.gameObject.tag == "satellite")
            {
                showingSat= hit.transform.gameObject.GetComponent<satellite>();
                uiMan.showSatInfoPanel(showingSat);
                showSat = true;
            }
            
        }
    }

    private void FixedUpdate()
    {
        if (showSat&&uiMan.satInfo.gameObject.activeInHierarchy)
        {
            uiMan.showSatInfoPanel(showingSat); 
        }
    }

    public void DisableGameObj(GameObject go)
    {
        go.SetActive(false);
    }
    public void EnableGameObj(GameObject go)
    {
        go.SetActive(true);
    }
    public void DisableOrEnableGameObj(GameObject go)
    {
        go.SetActive(!go.activeInHierarchy);
    }

    public void ChangeAtmosphere()
    {
        
    }

}

    [System.Serializable]
public class Calculation
{
    public float earthRadius = 6371300f;
    public float earthMass = 5.97f * Mathf.Pow(10,24);
    public float GravityConst = 6.674f * Mathf.Pow(10, -11);
    public float v = 0;

    public float satelliteHeight;
    public float periodTime=0;
    public float satelliteSpeed =0;

     

    float CalculatePeriodTime()
    {
        v = Mathf.Sqrt((GravityConst * earthMass) / (earthRadius+satelliteHeight*1000)); 
        return (2 * Mathf.PI * (earthRadius + satelliteHeight*1000)) / v;
    }

    public float CalculateSatelliteSpeed()
    {
        periodTime = CalculatePeriodTime();
        satelliteSpeed = 1 / ((periodTime / 720) / 7.2f);
        return satelliteSpeed;
    }

}

