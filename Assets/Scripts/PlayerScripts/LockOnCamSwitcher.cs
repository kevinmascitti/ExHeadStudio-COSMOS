using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnCamSwitcher : MonoBehaviour
{
    [SerializeField] CinemachineFreeLook playerFreeLookCam;
    [SerializeField] CinemachineFreeLook lockOnCam;

    private bool isLockOn = true;

    public bool lockOn;
    void Start()
    {
        playerFreeLookCam.Priority = 11;
        lockOnCam.Priority = 10;
        /*playerFreeLookCam.enabled = true;
        lockOnCam.enabled = false;*/
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            Debug.Log("cambiocamera");
            if (!isLockOn)
            {
                playerFreeLookCam.Priority = 11;
                lockOnCam.Priority = 10;
                /*playerFreeLookCam.enabled = true;
                lockOnCam.enabled = false;*/
                isLockOn = true;
                lockOn = false;

            }
            else
            {
                playerFreeLookCam.Priority = 10;
                lockOnCam.Priority = 11;
                /*playerFreeLookCam.enabled = false;
                lockOnCam.enabled = true;*/
                isLockOn = false;
                lockOn = true;
            }



        }
    }
}
