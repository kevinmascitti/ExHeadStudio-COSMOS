using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointZoneScript : MonoBehaviour
{


    [Tooltip("Inserire qui la empty che fa da punto di respawn")]
    [SerializeField] private Transform checkPointTr;

    private Collider areaCollider;

    private void Start()
    {
        areaCollider = GetComponent<Collider>();
        areaCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.transform.position = checkPointTr.position;
        }
    }

}
