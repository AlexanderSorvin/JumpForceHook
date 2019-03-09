using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    Transform BackgroundT;
    Transform CameraT;

    void Start()
    {
        BackgroundT = GetComponent<Transform>();
        CameraT = Camera.main.GetComponent<Transform>();
    }

    void FixedUpdate()
    {
        BackgroundT.position = new Vector3(CameraT.position.x, BackgroundT.position.y, BackgroundT.position.z);
    }
}
