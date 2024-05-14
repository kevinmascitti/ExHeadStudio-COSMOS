using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    //per la camera
    private Transform cameraMain;
    [SerializeField] float rotationSpeed = 4f;

    private void Start()
    {
        playerController = GetComponent<CharacterController>();
        HandleJumpVariables();
        playerVector = new Vector3(0f, 0f, 0f);
        playerTransform = GetComponent<Transform>();

        cameraMain = Camera.main.transform;
    }

    void Update()
    {
        //Ottengo gli input da tastiera e li salvo in un vettore 2D che mi servirà dopo per calcolare la direzione del player
        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        playerVector.x = inputs.x * movementSpeed;
        playerVector.z = inputs.y * movementSpeed;

        //Per la rotazione della camera: i valori del vettore vengono moltipliacati per la direzione della camera
        playerVector = cameraMain.forward * playerVector.z + cameraMain.right * playerVector.x;
        playerVector.y = 0f;

        // Se il player è a terra e si preme il tasto di salto, il player salta
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
        {
            isJumpPressed = true;
        }
        /*Se i valori di input del vettore 2D sono a zero, allora il player è fermo, altrimenti ruota il player nella direzione di movimento
        ATTENZIONE: Usare un vettore 3D, con le funzionalità implementate, non fa ruotare in modo corretto, oppure se si setta y=0f, dà errore.
        Il controllo sul vettore 2D anziché su quello 3D evita che compaia l'errore "Look Rotation Viewing Vector Is Zero" */
        if (inputs != Vector2.zero)
        {
            //playerTransform.forward = new Vector3(inputs.x, 0f, inputs.y);

            //Prove per la camera, con questi comandi il player punta e ruota nella stessa direzione della camera
            float targetAngle = Mathf.Atan2(inputs.x, inputs.y) * Mathf.Rad2Deg + cameraMain.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            playerTransform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        //Muove il player, calcola la gravità da applicare e gestisce il salto
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
