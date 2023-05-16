using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRotator : MonoBehaviour
{
    public GameObject targetObject;
    public float rotationSpeed = 1.0f;
    public bool isEditing;
    private float currentRotationAngleX = 0.0f;
    private float currentRotationAngleY = 0.0f;


    private Vector3 lastMousePosition;

    void Update()
    {
         
    }


    public void changeFieldOfView()
    {
        if ((Camera.main.fieldOfView - Input.mouseScrollDelta.y) > 0.5f)
            Camera.main.fieldOfView -= Input.mouseScrollDelta.y;
    }

    public void changeCameraPos()
    { 

        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0)&&!isEditing)
        {

            Vector3 delta = Input.mousePosition - lastMousePosition;
            float rotationX = delta.y * (rotationSpeed * (Camera.main.fieldOfView / 60));
            float rotationY = -delta.x * (rotationSpeed*(Camera.main.fieldOfView/60));
            currentRotationAngleX += rotationX;
            currentRotationAngleY += rotationY;


            transform.localRotation = Quaternion.Euler(currentRotationAngleX , currentRotationAngleY + 180, 0);
            lastMousePosition = Input.mousePosition;
        }
    }
}
