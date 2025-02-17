using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class folowCamera : MonoBehaviour
{
    public GameObject targetCamera;

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = targetCamera.transform.localPosition;
        transform.localRotation = targetCamera.transform.localRotation;
    }
}
