using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Globalization;

public class SatelliteInfoUI : MonoBehaviour
{
    public TMP_Text SatName,minHeight, maxHeight, orbitalSpeed,coordinates,CoveredArea;
    public bool isShowing;

    public float er=0.99999f ,deltaPhi;
    string lat, hei;
    GameManager gm;
    rayCalculations rc;
    public satellite ourSat;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        rc = gm.gameObject.GetComponent<rayCalculations>();
    }



    public void DisplayInfo(satellite sat)
    {
        ourSat = sat;
        SatName.text = "имя спутника: " + ourSat.gameObject.name;
        minHeight.text = "минимальная высота: " + ourSat.minHeight.ToString();
        maxHeight.text = "максимальная высота: " + ourSat.maxHeight.ToString();
        orbitalSpeed.text = "скорость в данный момент: " + ourSat.orbitalSpeed.ToString();

        float teta = Mathf.Acos(ourSat.transform.position.normalized.z) - 90 * Mathf.Deg2Rad;
        float phi = Mathf.Atan(ourSat.transform.position.normalized.x / (ourSat.transform.position.normalized.y + er)) + 23.5f * Mathf.Deg2Rad;
        Debug.Log("teta: " + teta * Mathf.Rad2Deg + ",phi: " + phi * Mathf.Rad2Deg);

        float x = Mathf.Abs(Mathf.Acos(Mathf.Cos(teta) * Mathf.Cos(phi)) * Mathf.Rad2Deg - 90);
        float y = Mathf.Abs(Mathf.Acos(Mathf.Cos(teta) * Mathf.Sin(phi)) * Mathf.Rad2Deg - 90);

        float tet90 = Mathf.Acos(Mathf.Cos(teta) * Mathf.Sin(phi)) * Mathf.Rad2Deg;

        if (teta > -90 && teta < 90)
        {
            hei = "в.д";
        }
        else
        {
            hei = "з.д";

        }

        float ph90 = Mathf.Acos(Mathf.Cos(teta) * Mathf.Cos(phi)) * Mathf.Rad2Deg;
        Debug.Log("coordinates: " + tet90 + " | " + ph90);

        if (teta > 0)
        {
            lat = "ю.ш";
        }
        else
        {
            lat = "с.ш";

        } 
        coordinates.text = "пролитает над координатами: " + "\n" + x.ToString("F2", CultureInfo.InvariantCulture) + " " + lat +
            y.ToString("F2", CultureInfo.InvariantCulture) + " " + hei;

        Collider[] satRays=new Collider[ourSat.transform.GetChild(0).childCount];

        for(int i = 0; i < satRays.Length; i++)
        {
            satRays[i] = ourSat.transform.GetChild(0).GetChild(i).GetChild(0).gameObject.GetComponent<Collider>();
        }


        float area = rc.CalculateRayArea(satRays);

        CoveredArea.text = "площадь покрываемая спутником: " + area;

    }

}
