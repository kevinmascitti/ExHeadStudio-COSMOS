using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{

    //PlayerMovement player;
    [SerializeField] CamMovementTest player;
    [SerializeField] private Animator playerAnimator;

    private bool isJumpAscension;
    private bool isJumpPeak;
    private bool isJumpFalling;
    private bool isMoving;
    private bool isIdling;
    void Awake()
    {
        //player = GetComponent<PlayerMovement>();
        player = GetComponent<CamMovementTest>();
        playerAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
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
}
