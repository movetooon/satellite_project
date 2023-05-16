using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MAthfutilits  
{
    public static float GeronArea(Vector3 a, Vector3 b, Vector3 c)
    {
        Debug.DrawLine(a, b, new Color(1, 1, 1, 0.1f));
        Debug.DrawLine(b, c, new Color(1, 1, 1, 0.1f));
        Debug.DrawLine(c, a, new Color(1, 1, 1, 0.1f));

        float Aside = (a - b).magnitude;
        float Bside = (b - c).magnitude;
        float Cside = (c - a).magnitude;


        float halfP = (Aside + Bside + Cside) / 2;

        float area = Mathf.Sqrt(halfP * ((halfP - Aside) * (halfP - Bside) * (halfP - Cside)));

        return area * (6371 * 6371);

    }
}
