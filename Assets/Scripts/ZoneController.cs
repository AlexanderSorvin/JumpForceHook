using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    public float sizeX = 10;
    public float sizeY = 10;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + new Vector3(0.0f, 0.0f, 0.0f), transform.position + new Vector3(sizeX, 0.0f, 0.0f));
        Gizmos.DrawLine(transform.position + new Vector3(0.0f, sizeY, 0.0f), transform.position + new Vector3(sizeX, sizeY, 0.0f));
        Gizmos.DrawLine(transform.position + new Vector3(0.0f, 0.0f, 0.0f), transform.position + new Vector3(0.0f, sizeY, 0.0f));
        Gizmos.DrawLine(transform.position + new Vector3(sizeX, 0.0f, 0.0f), transform.position + new Vector3(sizeX, sizeY, 0.0f));

    }
}
