using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionProbe : MonoBehaviour
{
    public GameObject Mirror;
    public Camera Camera;

    public enum Direction { X, Y, Z };

    public Direction Orientation;

    private float Offset;
    private Vector3 ProbeVector;

    private void Update()
    {
        switch (Orientation)
        {
            case Direction.X:
                Offset = Mirror.transform.position.x - Camera.transform.position.x;

                ProbeVector.x = Mirror.transform.position.x + Offset;
                ProbeVector.y = Camera.transform.position.y;
                ProbeVector.z = Camera.transform.position.z;
                break;
            case Direction.Y:
                Offset = Mirror.transform.position.y - Camera.transform.position.y;

                ProbeVector.x = Camera.transform.position.x;
                ProbeVector.y = Mirror.transform.position.y + Offset;
                ProbeVector.z = Camera.transform.position.z;
                break;
            case Direction.Z:
                Offset = Mirror.transform.position.z - Camera.transform.position.z;

                ProbeVector.x = Camera.transform.position.x;
                ProbeVector.y = Camera.transform.position.y;
                ProbeVector.z = Mirror.transform.position.z + Offset;
                break;
        }
        transform.position = ProbeVector;
    }
}
