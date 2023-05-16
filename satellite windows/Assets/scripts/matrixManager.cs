using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class matrixManager : MonoBehaviour
{
    public List<GameObject> matrixObject = new List<GameObject>();  
    public int[,] matrix = new int[1, 1];
    public List<TMPro.TMP_Text> names = new List<TMPro.TMP_Text>();
    public GameManager gm;
    public LayerMask rayMask;
    public GameObject moreInfo,satInfo,stationInfo;

    List<LineRenderer> satLineRenderer = new List<LineRenderer>();
    public bool isMatrixActive = false;
    public   bool canDisplayNames;


    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }

    private void FixedUpdate()
    {
        if (isMatrixActive)
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition,new Vector3(1.15f,0.35f, Camera.main.transform.localPosition.z),0.05f);
        }
        else
        {
            Camera.main.transform.localPosition = Vector3.Lerp(Camera.main.transform.localPosition, new Vector3(0,0,-3), 0.05f);

        }

       
        FillMatrix(); 
    }

    private void Update()
    {
        displayNames();
    }

    public void showInfo(GameObject obj)
    {
        if (obj.tag == "satellite")
        {
            satInfo.SetActive(true);
        }
    }

    void displayNames()
    { 
        

        for (int i = 0; i < gm.matMan.matrixObject.Count; i++)
        {
             canDisplayNames = Vector3.Distance(matrixObject[i].transform.GetChild(0).transform.position, Camera.main.transform.position) < 3f && Camera.main.fieldOfView < 25;

            if (canDisplayNames)
            {
                moreInfo.SetActive(true);
                names[i].enabled = true;
                names[i].text = matrixObject[i].name;
                names[i].transform.position = Camera.main.WorldToScreenPoint( matrixObject[i].transform.GetChild(0).position); 
            }
            else
            {
                moreInfo.SetActive(false);
                names[i].enabled = false; 
            }
        }

       
    }

    string DisplayMatrix(int[,] matr)
    {
        string res = "";

        for(int i = 0; i < (matrixObject.Count); i++)
        {
            for (int j = 0; j <(matrixObject.Count) ; j++)
            {
                res += matrix[i,j];
            }
            res += '\n';
        }

        return res;
    }

    void FillMatrix()
    {
        matrix = new int[matrixObject.Count, matrixObject.Count];
         


        for(int i = 0; i < matrixObject.Count; i++)
        {
            int stationLines = 0;

            for (int j = 0; j < matrixObject.Count; j++)
            {

                if (matrixObject[i].gameObject.tag == "station")
                { 
                    if(matrixObject[i].transform.GetChild(0).GetChild(0).gameObject.GetComponent<Collider>().bounds.Contains(matrixObject[j].transform.GetChild(0).position)&&
                         Physics.CheckSphere(matrixObject[j].transform.GetChild(0).position, 0.0001f))
                    {
                        
                        matrixObject[i].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(stationLines, matrixObject[i].transform.GetChild(0).position);
                        matrixObject[i].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(stationLines+1, matrixObject[j].transform.GetChild(0).position);
                        stationLines += 2;

                    }
                    else
                    {
                        stationLines = 0;
                        matrixObject[i].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(stationLines, Vector3.zero);
                        matrixObject[i].transform.GetChild(0).gameObject.GetComponent<LineRenderer>().SetPosition(stationLines + 1, Vector3.zero);
                    }
                }
                else
                {
                    if (matrixObject[i] != matrixObject[j])
                    {
                        int intersections = 0;
                        for (int k = 0; k < matrixObject[i].transform.GetChild(0).childCount; k++)
                        { 

                            if (matrixObject[i].transform.GetChild(0).GetChild(k).GetChild(0).GetComponent<MeshCollider>().bounds.Contains(matrixObject[j].transform.GetChild(0).position)
                                && Physics.CheckSphere(matrixObject[j].transform.GetChild(0).position, 0.001f,rayMask))
                            {
                                intersections=1;
                            }
                             
                           
                        }
                        matrix[i, j] =intersections;
                        matrix[j, i] = intersections;
                    }
                    else
                    {

                        matrix[i, j] = 0;
                        matrix[j, i] = 0;
                    }
                }


                 
            }
        }
    }

     public void AddNewObject(GameObject newObj)
    {
        matrixObject.Add(newObj);
        if (newObj.tag == "satellite") satLineRenderer.Add(newObj.GetComponent<LineRenderer>());
    }

}
