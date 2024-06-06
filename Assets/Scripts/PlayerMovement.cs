using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.UI;
using UnityEngine.UIElements;

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
    private bool isJumpAscension = false;
    private bool isJumpPeak = false;
    private bool isJumpFalling = false;
    private bool canJumpAgain = true;
    float gravity = -9.81f;
    float groundedGravity = -0.5f;
    Vector3 playerVector;
    float initialJumpVelocity;
    Transform playerTransform;
    private float horizontalVelocity;
    public float verticalVelocity;
    Vector3 lastPositionAcquired;
    private bool isAttacking;

    private PlayerCharacter player;
    [Header("Animazioni Movimento")]


    [Header("Camera")]
    [SerializeField] private Transform mainCamera; //non basta assegnare la virtual camera, serve la MAIN
    [SerializeField] float rotationSpeed = 4f;
    private void Awake()
    {
        player=GetComponent<PlayerCharacter>();
        horizontalVelocity = 0f;
        playerController = GetComponent<CharacterController>();
        HandleJumpVariables();
        playerVector = new Vector3(0f, 0f, 0f);
        playerTransform = GetComponent<Transform>();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;


        // playerCamera = GameObject.FindGameObjectWithTag("PlayerCamera").transform;
    }

    private void Start()
    {
        lastPositionAcquired = playerTransform.position;
    }

    void Update()
    {
        if (player.isInputOn)
        {
            //Ottengo gli input da tastiera e li salvo in un vettore 2D che mi servir� dopo per calcolare la direzione del player
            //isAttacking=player.GetIsAttacking();
            Vector2 inputs = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
            //Vector3 inputs = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));//.normalized;
            playerVector.x = inputs.x * movementSpeed;
            playerVector.z = inputs.y * movementSpeed;
            //float yMovement = playerVector.y;
            //playerVector.x = inputs.x * movementSpeed * mainCamera.right.magnitude;
            //playerVector.z = inputs.y * movementSpeed * mainCamera.forward.magnitude;

            //Per la rotazione della camera: i valori del vettore vengono moltiplicati per la direzione della camera
            //float mainCameraX = mainCamera.right.magnitude * playerVector.x;
            //float mainCameraZ = mainCamera.forward.magnitude * playerVector.z;

            //float mainCameraX = mainCamera.right.magnitude;
            //float mainCameraZ = mainCamera.forward.magnitude;
            Vector3 inputMovement = mainCamera.right * playerVector.x + mainCamera.forward * playerVector.z;

            inputMovement.y = playerVector.y;
            playerVector = inputMovement;
            //playerVector.y=yMovement;
            //playerVector = new Vector3();
            //playerVector = mainCamera.right + mainCamera.forward + new Vector3(inputs.x * movementSpeed, 0f, inputs.z * movementSpeed);
            //Debug.Log("Update: " + playerVector);
            //playerVector.y = 0f;
            //Debug.Log("Right: "+ mainCamera.right);
            // Se il player � a terra e si preme il tasto di salto, il player salta
            if (Input.GetKeyDown(KeyCode.Space) && canJumpAgain)
            {
                isJumpPressed = true;
                //Debug.Log("Salto");

            }
            //Debug.Log(playerVector.y);


            /*Se i valori di input del vettore 2D sono a zero, allora il player � fermo, altrimenti ruota il player nella direzione di movimento
            ATTENZIONE: Usare un vettore 3D, con le funzionalit� implementate, non fa ruotare in modo corretto, oppure se si setta y=0f, d� errore.
            Il controllo sul vettore 2D anzich� su quello 3D evita che compaia l'errore "Look Rotation Viewing Vector Is Zero" */
            if ((inputs.x != 0) || (inputs.y != 0)) //(inputs != Vector2.zero)
            {
                //playerTransform.forward = new Vector3(mainCameraX, 0f, mainCameraZ);

                //Prove per la camera, con questi comandi il player punta e ruota nella stessa direzione della camera
                float targetAngle = Mathf.Atan2(inputs.x, inputs.y) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
                playerTransform.rotation =
                    Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            //Muove il player, calcola la gravit� da applicare e gestisce il salto

            ComputePlayerVelocity(playerTransform.position);
            //if(!playerController.isGrounded)
            //  Debug.Log(verticalVelocity);
            //Debug.Log(playerVector);
            HandleGravity();
            HandleJump();
            if (!isAttacking)
                playerController.Move(playerVector * Time.deltaTime);
        }
    }

    private void HandleJumpVariables()
    {
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        //Debug.Log("Gravit�: " + gravity);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
        //Debug.Log("Velocit�: " + initialJumpVelocity);

    }
    private void HandleJump()
    {
        //Debug.Log(isJumpAscension + ", " + isJumpPressed);
        if (isJumpPressed && playerController.isGrounded)
        {
            //Debug.Log("Jumping");
            //StartCoroutine(JumpAnimationTimeOffset());
            isJumpAscension = true;
            canJumpAgain = false;
            isJumpPressed = false;
            playerVector.y = initialJumpVelocity;
            

        }
        else if(isJumpAscension && verticalVelocity <= 1f && !playerController.isGrounded)
        {
            //Debug.Log("Falling");
            isJumpAscension = false;
            isJumpPeak=false;
            isJumpFalling = true;
        }
        else if (playerController.isGrounded && isJumpFalling)
        {
            isJumpFalling=false;
            StartCoroutine(WaitForJumpAgain());
        }
        else if(isJumpAscension && playerController.isGrounded)
        {
            isJumpAscension = false;
            isJumpPeak = false;
            isJumpFalling = true;
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
        float playerPositionY = newPosition.y;
        float lastPlayerPosY = lastPositionAcquired.y;
        verticalVelocity = (playerPositionY - lastPlayerPosY) / Time.deltaTime;
        horizontalVelocity = (playerPosition - lastPlayerPos).magnitude / Time.deltaTime;
        lastPositionAcquired = newPosition;
    }

    IEnumerator WaitForJumpAgain()
    {
        yield return new WaitForSeconds(1f);
        canJumpAgain = true;
    }

    public bool GetIsJumpAscension()
    {
        return isJumpAscension;
    }

    public bool GetIsJumpPeak()
    {
        return isJumpPeak;
    }
    public bool GetIsJumpFalling()
    {
        return isJumpFalling;
    }
    public bool GetIsMoving()
    {
        return horizontalVelocity != 0f && playerController.isGrounded;
    }


}
