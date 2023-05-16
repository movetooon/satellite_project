using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class stationAdder : MonoBehaviour
{
    public GameObject station,stationHolder;
    public Slider latitude, height;
    public TMP_Text latText, heiText;
    public TMP_InputField stationName;
    

    cameraRotator cam;
    GameObject editingStation;
    float startFieldOfView;
    bool changeView=false;
    float t=0;
    Vector3 lookAtStation;
    GameManager gm;
    

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        cam = gm.cr;
    }

    private void  Update()
    {
        if (editingStation != null)
        {
            if (stationName.text.Length>0)
                editingStation.name = stationName.text;
            else editingStation.name = "станция" + gm.stationsCount;


            editingStation.transform.localRotation = Quaternion.Euler(height.value,0  , latitude.value );

          

            if ((height.value >= -180 && height.value <= -90)|| (height.value >= 90 && height.value <= 180))
            {
                 lookAtStation = new Vector3(360-editingStation.transform.localRotation.eulerAngles.z + 180,
                  360-editingStation.transform.localRotation.eulerAngles.x, 0);
            }
            else
            {
                  lookAtStation = new Vector3(editingStation.transform.localRotation.eulerAngles.z + 180,
                   editingStation.transform.localRotation.eulerAngles.x, 180);
            }

            cam.transform.position = Vector3.Lerp(cam.transform.position, editingStation.transform.position, 0.05f);
            cam.transform.localRotation = Quaternion.Lerp(cam.transform.localRotation, Quaternion.Euler(lookAtStation), 0.05f);

            cam.isEditing = true;
            DisplayValues();
            
        }

        if (cam.isEditing&&changeView)
        {
             t+=Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(startFieldOfView, 10, t);
        }
        else if(changeView)
        {
            t += Time.deltaTime;
            Camera.main.fieldOfView = Mathf.Lerp(10,startFieldOfView, t);
            if (t > 1)
            {
                changeView = false; 
            }
        }
    }

    public void DisplayValues()
    {
        if (latitude.value > 0)
        { 
            latText.text = (Mathf.Abs(latitude.value).ToString())+" ю.ш";
        }
        else
        {
            latText.text = (Mathf.Abs(latitude.value).ToString())+" с.ш"; 
        }

        if (height.value > 0)
        {
           heiText.text = (Mathf.Abs(height.value).ToString()) + " з.д";
        }
        else
        {
            heiText.text = (Mathf.Abs(height.value).ToString()) + " в.д";
        }
    }

    public void ApplyStation()
    {
        

        gm.matMan.AddNewObject(editingStation);
        gm.stationsCount++;
        gameObject.SetActive(false);
        editingStation = null;
        latitude.value = 0;
        height.value = 0;
        t = 0;
        TMPro.TMP_Text newName = Instantiate(gm.matMan.names[0].gameObject).GetComponent<TMP_Text>();
        newName.transform.parent = gm.matMan.names[0].transform.parent;
        gm.matMan.names.Add(newName);

        cam.isEditing =false;
         

    }

    public void CreateStation()
    {
        gm = FindObjectOfType<GameManager>();
        cam = Camera.main.transform.parent.GetComponent<cameraRotator>();
        t = 0; 
        cam.isEditing = true;
        changeView = true;
        startFieldOfView = Camera.main.fieldOfView; 
        editingStation = Instantiate(station, Vector3.zero, Quaternion.identity);
        editingStation.transform.parent = stationHolder.transform;
        gameObject.SetActive(true);
    }

}
