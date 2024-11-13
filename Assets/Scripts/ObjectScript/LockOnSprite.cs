using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnSprite : MonoBehaviour
{
 
    [SerializeField] Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(cameraTransform.position + cameraTransform.forward);
        //transform.LookAt(cameraTransform.position);
    }
}
