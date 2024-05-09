using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Variabili Movimento")]
    [SerializeField, Range(0f, 100f)] float movementSpeed;
    //[SerializeField, Range(0f, 10f)] float speedMultiplier;
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
    Transform playerTransform;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        HandleJumpVariables();
        playerVector = new Vector3(0f, 0f, 0f);
        playerTransform = GetComponent<Transform>();
    }

    void Update()
    {
        //Ottengo gli input da tastiera e li salvo in un vettore 2D che mi servir� dopo per calcolare la direzione del player
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        playerVector.x = inputs.x * movementSpeed;
        playerVector.z = inputs.y * movementSpeed;

        // Se il player � a terra e si preme il tasto di salto, il player salta
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
        {
            isJumpPressed = true;
        }
        /*Se i valori di input del vettore 2D sono a zero, allora il player � fermo, altrimenti ruota il player nella direzione di movimento
        ATTENZIONE: Usare un vettore 3D, con le funzionalit� implementate, non fa ruotare in modo corretto, oppure se si setta y=0f, d� errore.
        Il controllo sul vettore 2D anzich� su quello 3D evita che compaia l'errore "Look Rotation Viewing Vector Is Zero" */
        if (inputs != Vector2.zero)
        {
            playerTransform.forward = new Vector3(inputs.x, 0f, inputs.y);
        }

        //Muove il player, calcola la gravit� da applicare e gestisce il salto
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
