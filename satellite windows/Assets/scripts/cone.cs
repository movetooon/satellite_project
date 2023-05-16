using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cone : MonoBehaviour
{
    public float height = 1f;   // ������ ������
    public float angle = 30f;   // ���� ������ � ��������
    public Transform satellite,satellineAxis;
     

    private void Update()
    {
        float coof = (satellite.localPosition.y) / (satellite.localPosition.y + 1.5f);
        satellineAxis.transform.localScale = new Vector3(satellineAxis.transform.localScale.x, satellineAxis.transform.localScale.y, transform.localScale.y);
        height = satellite.localPosition.y * (10);
        float radius = height * Mathf.Tan(angle/2 * Mathf.Deg2Rad);   // ��������� ������
        transform.localScale = new Vector3(radius*2, height, radius*2 );   // ������������� ����� ������
    }
}
