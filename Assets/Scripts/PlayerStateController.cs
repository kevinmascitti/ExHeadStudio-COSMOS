using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{

    PlayerMovement player;
    private Animator playerAnimator;

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

    // Update is called once per frame
    void Update()
    {
        isMoving = player.GetIsMoving();
        isJumpAscension = player.GetIsJumpAscension();
        isJumpPeak = player.GetIsJumpPeak();
        isJumpFalling = player.GetIsJumpFalling();

        isIdling = !isMoving && !isJumpAscension && !isJumpPeak && !isJumpFalling;
        playerAnimator.SetBool("isJumpAscension", isJumpAscension);
        playerAnimator.SetBool("isJumpPeak", isJumpPeak);
        playerAnimator.SetBool("isJumpFalling", isJumpFalling);
        playerAnimator.SetBool("isMoving", isMoving);
        playerAnimator.SetBool("isIdle", isIdling);
    }
}
