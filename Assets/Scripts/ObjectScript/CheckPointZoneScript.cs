using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointZoneScript : MonoBehaviour
{


    [Tooltip("Inserire qui la empty che fa da punto di respawn")]
    [SerializeField] private Transform checkPointTr;
    [SerializeField] private int damageForFall = 10;
    [SerializeField] private CinemachineFreeLook playerCamera;

    private Collider areaCollider;
    private Vector3 camStartPos;

    private void Awake()
    {
        areaCollider = GetComponent<Collider>();
        areaCollider.isTrigger = true;
        camStartPos = playerCamera.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        playerCamera.PreviousStateIsValid = false;
        if (other.CompareTag("Player"))
        {
            other.GetComponent<CharacterController>().enabled = false;
            other.GetComponent<PlayerMovement>().enabled = false;
            other.transform.position = checkPointTr.position;
            other.transform.rotation = Quaternion.LookRotation(-transform.right, Vector3.up);
            other.GetComponent<CharacterController>().enabled = true;
            other.GetComponent<PlayerMovement>().enabled = true;
            other.GetComponent<PlayerCharacter>().TakeDamage(10, Element.Normal);

        }
    }

}
