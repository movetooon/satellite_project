using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lookAt : MonoBehaviour
{
    public GameObject looker;

    private void Update()
    {
        transform.LookAt(looker.transform.position);
    }
}
