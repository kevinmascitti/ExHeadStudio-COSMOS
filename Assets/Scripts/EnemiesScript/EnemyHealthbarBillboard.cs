using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthbarBillboard : MonoBehaviour
{
    //questo script va assegnato al Canva che contiene la healthbar dei nemici

    [SerializeField] Transform cameraTransform;

    private void Awake()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void LateUpdate()
    {
        transform.LookAt(cameraTransform.position + cameraTransform.forward);
    }
}

