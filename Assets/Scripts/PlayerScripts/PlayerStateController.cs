using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{
    private PlayerMovement player;
    private Animator playerAnimator;
    private FireArmAbility fireArmReference; //la reference è necessaria per usare gli eventi da animator

    private bool isJumpAscension;
    private bool isJumpPeak;
    private bool isJumpFalling;
    private bool isMoving;
    private bool isIdling;
    void Awake()
    {
        player = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        isMoving = player.GetIsMoving();
        isJumpAscension = player.GetIsJumpAscension();
        isJumpPeak = player.GetIsJumpPeak();
        isJumpFalling = player.GetIsJumpFalling();

        //isIdling = !isMoving && !isJumpAscension && !isJumpPeak && !isJumpFalling;
        if(isMoving == false && isJumpAscension == false && isJumpPeak == false && isJumpFalling == false)
        isIdling = true;
        else isIdling = false;

        playerAnimator.SetBool("isJumpAscension", isJumpAscension);
        playerAnimator.SetBool("isJumpPeak", isJumpPeak);
        playerAnimator.SetBool("isJumpFalling", isJumpFalling);
        playerAnimator.SetBool("isMoving", isMoving);
        playerAnimator.SetBool("isIdle", isIdling);
    }


    public void OnAnimationEvent()
    {
        if ((fireArmReference = GetComponentInChildren<FireArmAbility>()) != null)
        {
            fireArmReference.ShootBullet();
        }
    }
}

