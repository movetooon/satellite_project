using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 
public class Rotation : MonoBehaviour
{
    public float speed;
    public Vector3 rotationAxis;
    float time=0;
    public bool calculateTime;

    private void FixedUpdate()
    { 
        time = Time.fixedTime;
        if(calculateTime)
        Debug.Log(transform.localRotation.eulerAngles.y + " time passed: " + time + " " + gameObject.name);
        transform.Rotate(rotationAxis.normalized*speed);
    }

}
