using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class satTools : MonoBehaviour
{
    public float Re = 6371;    // радиус Земли в км
    public float Hs = 6371+1000; // радиус орбиты СР в км
    const float  deg = Mathf.PI / 180;
    public int points_num = 20; // !важно! количество точек для нанесения на карту, при большом количестве возможны лаги

    float x=0,y=0;
    Vector2[] points;

    public float widePetal, longlessCenterRay, wideRayDN, SatelliteHeight, SatelliteAntenna, minAngleEarthStation;


    private void Start()
    {
          points = new Vector2[points_num];
    }

    // FIe - широта центра луча прицеливания, LMEDe - долгота центра луча прицеливания, DN - ширина лепестка ДН антенны на уровне -3дБ 
    // satlat - широта КА (спутника), satlon - широта КА, GAMMAg - минимальный угол места для земной станции
    // !важно! все входные параметры измеряются в радианах, для перевода из градусов умножить на deg (например долгота при значении спутника 90 град на вход подается как 90 * deg ) 

    private void Update()
    {
        BuildZone(widePetal,longlessCenterRay,wideRayDN,SatelliteHeight,SatelliteAntenna,minAngleEarthStation);
    }

    void BuildZone(float FIe, float LMDe, float DN, float satlat, float satlon, float GAMMAg)
    {
        //step1
       float SIGMA = 0;

        float PSIb = Mathf.Atan((Re * Mathf.Cos(FIe - satlat) * Mathf.Sin(LMDe - satlon)) / (Hs - Re * Mathf.Cos(FIe) * Mathf.Cos(LMDe - satlon))); ;
        float PSIp = Mathf.Atan((Re * Mathf.Sin(FIe - satlat) * Mathf.Cos(PSIb)) / (Hs - Re * Mathf.Cos(FIe - satlat) * Mathf.Cos(LMDe - satlon)));

        //step2
        float RO0 = Mathf.Asin((Re * Mathf.Cos(GAMMAg)) / Hs);
        float RO1 = GAMMAg + RO0;
        float RO2 = Hs - Re * Mathf.Sin(RO1);
        float y1 = RO2 * Mathf.Tan(PSIb);
        float z1 = (RO2 * Mathf.Tan(PSIp)) / (Mathf.Cos(PSIb));
        float RO3 = Mathf.Pow(Re * Mathf.Cos(RO1), 2) - Mathf.Pow(y1, 2) - Mathf.Pow(z1, 2);

        //step3 
        float[,] a = new float[,] { { 0, 0, 0 }, { 0, 0, 0 }, { 0, 0, 0 } };
        a[0, 0] = Mathf.Cos(PSIp) * Mathf.Cos(PSIb);
        a[0, 1] = Mathf.Sin(PSIp) * Mathf.Cos(PSIb) * Mathf.Sin(SIGMA) + Mathf.Sin(PSIb) * Mathf.Cos(SIGMA);
        a[0, 2] = Mathf.Sin(PSIp) * Mathf.Cos(PSIb) * Mathf.Cos(SIGMA) - Mathf.Sin(PSIb) * Mathf.Sin(SIGMA);
        a[1, 0] = -Mathf.Cos(PSIp) * Mathf.Sin(PSIb);
        a[1, 1] = -Mathf.Sin(PSIp) * Mathf.Sin(PSIb) * Mathf.Sin(SIGMA) + Mathf.Cos(PSIb) * Mathf.Cos(SIGMA);
        a[1, 2] = -Mathf.Sin(PSIp) * Mathf.Sin(PSIb) * Mathf.Cos(SIGMA) - Mathf.Cos(PSIb) * Mathf.Sin(SIGMA);
        a[2, 0] = -Mathf.Sin(PSIp);
        a[2, 1] = Mathf.Cos(PSIp) * Mathf.Sin(SIGMA);
        a[2, 2] = Mathf.Cos(PSIp) * Mathf.Cos(SIGMA);

        int n = points_num;
        float step = 2 * Mathf.PI / n;

        //steps5-10
        for (int i = 0; i < n; i++)
        {
            float iter = i * step;
            float e = Mathf.Tan(DN / 2) / Mathf.Tan(DN / 2);
            float KSI = Mathf.Atan((Mathf.Tan(DN / 2)) / (Mathf.Sqrt(e * e * Mathf.Pow(Mathf.Sin(iter), 2) + Mathf.Pow(Mathf.Cos(iter), 2))));

            float Xa = -1;
            float Ya = Mathf.Tan(KSI) * Mathf.Cos(iter);
            float Za = Mathf.Tan(KSI) * Mathf.Sin(iter);

            float X1 = a[0, 0] * Xa + a[0, 1] * Ya + a[0, 2] * Za;
            float Y1 = a[1, 0] * Xa + a[1, 1] * Ya + a[1, 2] * Za;
            float Z1 = a[2, 0] * Xa + a[2, 1] * Ya + a[2, 2] * Za;

            float TAU = Mathf.Atan(Y1 / (-X1));
            float DELTA = Mathf.Atan(Z1 / (-X1));

            //step 11
            if (Mathf.Atan(Mathf.Sqrt(Mathf.Pow(Mathf.Tan(TAU), 2) + Mathf.Pow(Mathf.Tan(DELTA), 2))) <= RO0)
            {
                float p = Mathf.Sqrt(1 + Mathf.Pow(Mathf.Tan(TAU), 2) + Mathf.Pow(Mathf.Tan(DELTA), 2));
                float XN = (Hs / (p * p)) * (Mathf.Sqrt(1 - (p * p) * (1 - (Re * Re) / (Hs * Hs))) - 1);
                float YN = (-XN) * Mathf.Tan(TAU);
                float ZN = (-XN) * Mathf.Tan(DELTA);
                float D = Mathf.Sqrt(XN * XN + YN * YN + ZN * ZN);

                float FI = Mathf.Asin((D * Mathf.Tan(DELTA)) / (p * Re)) + satlat;
                float LMD = Mathf.Asin((D * Mathf.Tan(TAU)) / (p * Re * Mathf.Cos(FI))) + satlon;
                float C = Mathf.Abs(LMD);
                if (C > Mathf.PI)
                {
                    LMD = -LMD * (2 * Mathf.PI - C) / C;
                }

                x = LMD / deg;
                 y = FI / deg;
                points[i] = new Vector2(x, y);
                // x,y - искомые координаты в градусах
                // LMD, FI - искомые координаты в радианах
                // !not implemented! можно либо сразу рисовать точку через графический движок либо сохранять в какой-либо структуре данных
            }
        }
    }


    private void OnDrawGizmos()
    {
        for(int i = 0; i < points_num;i++)
        {
            Gizmos.DrawSphere(points[i], 1f);
        }
    }
}
