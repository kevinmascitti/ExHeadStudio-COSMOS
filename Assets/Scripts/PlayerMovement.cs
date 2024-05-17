using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;

public class PlayerMovement : MonoBehaviour
{

    [Header("Controlli Movimento")]
    [SerializeField, Range(0f, 100f)] float movementSpeed;
    //[SerializeField, Range(0f, 10f)] float speedMultiplier;
    [SerializeField, Range(0f, 100f)] float maxJumpHeight = 1f;
    [SerializeField, Range(0f, 100f)] float maxJumpTime = .5f;


    private CharacterController playerController;

    [Header("Variabili Movimento")]
    private bool isJumpPressed;
    private bool isJumping = false;
    float gravity = -9.81f;
    float groundedGravity = -0.5f;
    Vector3 playerVector;
    float initialJumpVelocity;
    Transform playerTransform;
    private float velocity;
    Vector3 lastPositionAcquired;

    [Header("Animazioni Movimento")]


    [Header("Camera")]

    [SerializeField] private Transform mainCamera; //non basta assegnare la virtual camera, serve la MAIN
    [SerializeField] float rotationSpeed = 4f;
    private void Awake()
    {
        
        velocity = 0f;
        playerController = GetComponent<CharacterController>();
        HandleJumpVariables();
        playerVector = new Vector3(0f, 0f, 0f);
        playerTransform = GetComponent<Transform>();

        // playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
    }

    private void Start()
    {
        lastPositionAcquired = playerTransform.position;
    }

    void Update()
    {
        //Ottengo gli input da tastiera e li salvo in un vettore 2D che mi servirà dopo per calcolare la direzione del player

        Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal") ,Input.GetAxisRaw("Vertical")).normalized;
        //Vector3 inputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));//.normalized;
        playerVector.x = inputs.x * movementSpeed;
        playerVector.z = inputs.y * movementSpeed;
        //playerVector.x = inputs.x * movementSpeed * mainCamera.right.magnitude;
        //playerVector.z = inputs.y * movementSpeed * mainCamera.forward.magnitude;

        //Per la rotazione della camera: i valori del vettore vengono moltiplicati per la direzione della camera
        //float mainCameraX = mainCamera.right.magnitude * playerVector.x;
        //float mainCameraZ = mainCamera.forward.magnitude * playerVector.z;

        //float mainCameraX = mainCamera.right.magnitude;
        //float mainCameraZ = mainCamera.forward.magnitude;
        playerVector = mainCamera.right * playerVector.x + mainCamera.forward * playerVector.z;
        //playerVector = mainCamera.right + mainCamera.forward + new Vector3(inputs.x * movementSpeed, 0f, inputs.z * movementSpeed);
        //Debug.Log("Update: " + playerVector);
        //playerVector.y = 0f;

        // Se il player è a terra e si preme il tasto di salto, il player salta
        if (Input.GetKeyDown(KeyCode.Space) && playerController.isGrounded)
        {
            isJumpPressed = true;
            //Debug.Log("Salto");

        }
        //Debug.Log(playerVector.y);

        /*Se i valori di input del vettore 2D sono a zero, allora il player è fermo, altrimenti ruota il player nella direzione di movimento
        ATTENZIONE: Usare un vettore 3D, con le funzionalità implementate, non fa ruotare in modo corretto, oppure se si setta y=0f, dà errore.
        Il controllo sul vettore 2D anziché su quello 3D evita che compaia l'errore "Look Rotation Viewing Vector Is Zero" */
        if ((inputs.x != 0) || (inputs.y != 0))//(inputs != Vector2.zero)
        {
            //playerTransform.forward = new Vector3(mainCameraX, 0f, mainCameraZ);

            //Prove per la camera, con questi comandi il player punta e ruota nella stessa direzione della camera
            float targetAngle = Mathf.Atan2(inputs.x, inputs.y) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            playerTransform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        //Muove il player, calcola la gravità da applicare e gestisce il salto

        ComputePlayerVelocity(playerTransform.position);

        playerController.Move(playerVector * Time.deltaTime);
        HandleGravity();
        HandleJump();

    }

    private void HandleJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        Debug.Log("Gravità: " + gravity);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        Debug.Log("Velocità: " + initialJumpVelocity);

    }

    private void HandleJump()
    {
        //Debug.Log(playerController.isGrounded + ", " + isJumpPressed);
        if (playerController.isGrounded && isJumpPressed)
        {
            isJumping = true;
            isJumpPressed = false;
            playerVector.y = initialJumpVelocity;

            Debug.Log("Jump: " + playerVector.y);
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
        //Debug.Log("Funzione: " + playerVector.y);
    }

    private void ComputePlayerVelocity(Vector3 newPosition)
    {
        Vector2 playerPosition =  new Vector2(newPosition.x, newPosition.z);
        Vector2 lastPlayerPos = new Vector2(lastPositionAcquired.x, lastPositionAcquired.z);
        velocity = (playerPosition - lastPlayerPos).magnitude / Time.deltaTime;
        lastPositionAcquired = newPosition;
    }

    public bool GetIsJumping()
    {
        return isJumping;
    }
    public bool GetIsMoving()
    {
        return velocity != 0f;
    }


}
