using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Variabili Movimento")]
    [SerializeField, Range(0f, 10f)] float moveSpeed;
    [SerializeField, Range(0f, 10f)] float speedMultiplier;
    [SerializeField, Range(0f, 100f)] float maxJumpHeight = 1f;
    [SerializeField, Range(0f, 100f)] float maxJumpTime = .5f;


    private CharacterController playerController;

    [Header("Movement Variables")]
    private bool isJumpPressed;
    private bool isJumping = false;
    float gravity = -9.81f;
    float groundedGravity = -0.5f;
    Vector3 playerVector;
    float initialJumpVelocity;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        HandleJumpVariables();
        playerVector = new Vector3(0f, 0f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        playerVector.x = Input.GetAxisRaw("Vertical") * moveSpeed;
        playerVector.z = Input.GetAxisRaw("Horizontal") * moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            playerVector.x = playerVector.x * speedMultiplier;

        }
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
        {
            isJumpPressed = true;
        }


        playerController.Move(playerVector * Time.deltaTime);
        HandleGravity();
        HandleJump();
    }

    private void HandleJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void HandleJump()
    {
        if (playerController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            isJumpPressed = false;
            playerVector.y = initialJumpVelocity;

            Debug.Log(playerVector.y);
        }
        else if (!isJumpPressed && isJumping && playerController.isGrounded)
        {
            isJumping = false;
        }
    }

    private void HandleGravity()
    {
        if (playerController.isGrounded)
        {
            playerVector.y = groundedGravity * Time.deltaTime;
        }
        else
        {
            playerVector.y += gravity * Time.deltaTime;
        }
    }
}
