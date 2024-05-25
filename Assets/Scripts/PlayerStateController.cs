using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateController : MonoBehaviour
{

    PlayerMovement player;
    private Animator playerAnimator;

    private bool isJumping;
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
        isJumping = player.GetIsJumping();

        isIdling = !isMoving && !isJumping;
        playerAnimator.SetBool("isJumping", isJumping);
        playerAnimator.SetBool("isMoving", isMoving);
        playerAnimator.SetBool("isIdle", isIdling);
    }
}
