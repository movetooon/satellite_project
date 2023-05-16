using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Toggle manual,earthRotation;
    public SatelliteInfoUI satInfo;


    public GameObject[] sliders;
    public GameObject[] inputs;
    public GameObject cameraRotator,earth;

   public void showSatInfoPanel(satellite sat)
    {
        satInfo.gameObject.SetActive(true);
        satInfo.isShowing = true;
        satInfo.DisplayInfo(sat);
    }

    /*
    private void OnGUI()
    {
        if (earthRotation.isOn)
        {
            cameraRotator.transform.SetParent(earth.transform);
        }
        else
        {
            cameraRotator.transform.SetParent(null);

        }

        for (int i = 0; i < sliders.Length; i++)
        {
            if (manual.isOn)
            {
                sliders[i].SetActive(false); 
            }
            else
            { 
                sliders[i].SetActive(true); 
            }
        }
        for (int i = 0; i < inputs.Length; i++)
        {
            if (manual.isOn)
            {
                inputs[i].SetActive(true);
            }
            else
            {
                inputs[i].SetActive(false);
            }
        }
    }

     */
}
