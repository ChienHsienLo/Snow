using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform camTransform;
    [SerializeField] Transform targetTransform;
    [SerializeField] bool followX;
    [SerializeField] bool followY;
    [SerializeField] bool followZ;

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3
        (
            followX? targetTransform.position.x : camTransform.position.x,
            followY? targetTransform.position.y : camTransform.position.y,
            followZ? targetTransform.position.z : camTransform.position.z
        );

        camTransform.position = newPosition;        
    }


}
