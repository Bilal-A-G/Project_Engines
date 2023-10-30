using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class RotateButton : MonoBehaviour
{
    [DllImport("RotateOnButtonClickDLL")]
    public static extern float RotateObject(float currentRotation);

    public GameObject gameObjectToRotate;

    public void OnButtonClick()
    {
        var currentRotation = gameObjectToRotate.transform.rotation.eulerAngles.y;
        var newRotation = RotateObject(currentRotation);
        gameObjectToRotate.transform.rotation = Quaternion.Euler(0f, newRotation, 0f);
    }
}