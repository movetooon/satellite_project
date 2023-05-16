using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UImatrixManager : MonoBehaviour
{
    GameManager gm;
    public GameObject table;
    public List<GameObject> raws = new List<GameObject>();
    

    private void Awake()
    {
        gm = FindObjectOfType<GameManager>();
        getRaws();
    }


    private void FixedUpdate()
    { 
        checkNewObjects();
        DisplayMatrix();
    }

    public void CameraMove()
    {
        gm.matMan.isMatrixActive = gameObject.activeInHierarchy;
        

    }

   


    void DisplayMatrix()
    {
        for(int i = 0; i < raws.Count; i++)
        {
            for (int j = 0; j <raws[i].transform.childCount; j++)
            {

                if (i == 0 && j == 0)
                {
                    raws[i].transform.GetChild(j).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "имя";
                } 
                else if (i == 0)
                {
                    raws[i].transform.GetChild(j).GetChild(0).GetComponent<TMPro.TMP_Text>().text = gm.matMan.matrixObject[j-1].name;
                }
                else if (j == 0)
                {
                    raws[i].transform.GetChild(j).GetChild(0).GetComponent<TMPro.TMP_Text>().text = gm.matMan.matrixObject[i-1].name;

                }
                else
                {
                    try 
                    {
                        if (gm.matMan.matrix[i - 1, j - 1] == 0)
                        {
                            raws[i].transform.GetChild(j).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "<color=#354F52>" +
                                gm.matMan.matrix[i - 1, j - 1].ToString()  ;
                        }
                        else
                        {
                            raws[i].transform.GetChild(j).GetChild(0).GetComponent<TMPro.TMP_Text>().text = "<color=#CAD2C5>" +
                               gm.matMan.matrix[i - 1, j - 1].ToString()    ;
                        }
                    }
                    catch  
                    {
                        Debug.Log("cheta nitak");
                    }
                     
                }
            }
        }
    }

    void checkNewObjects()
    {
        if (raws.Count < gm.matMan.matrixObject.Count+1)
        {
            for(int i = 0; i < raws.Count; i++)
            {
                GameObject newElement=Instantiate(raws[i].transform.GetChild(raws[i].transform.childCount - 1).gameObject);
                newElement.transform.parent = raws[i].transform;
                 
            }

           GameObject newRaw= Instantiate(raws[raws.Count - 1]);
            newRaw.transform.parent = table.transform;
            raws.Add(newRaw);


        }
    }

    void getRaws()
    {  
        for(int i = 0; i < table.transform.childCount; i++)
        { 
            raws.Add(table.transform.GetChild(i).gameObject);
        }
    }
}
