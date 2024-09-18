using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamMovementTest : MonoBehaviour
{
    public CharacterController playerController;

    [Header("Controlli Movimento")]
    [SerializeField, Range(0f, 100f)] float movementSpeed;
    [SerializeField, Range(0f, 100f)] float maxJumpHeight = 1f;
    //[SerializeField, Range(0f, 100f)] float maxJumpTime = .5f;
    [SerializeField, Range(0f, 100f)] float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;


    [Header("Variabili Movimento")]
    public bool isJumpPressed;
    public bool isJumpAscension = false;
    public bool isJumpPeak = false;
    public bool isJumpFalling = false;
    //public bool canJumpAgain = true;
    public bool isMoving; 
    float gravity = -9.81f;
    ////float groundedGravity = -0.5f;
    //float initialJumpVelocity;
    //private float horizontalVelocity;
    //public float verticalVelocity;
    private bool isAttacking;
    Vector3 playerVector;
    //Vector3 lastPositionAcquired;
    Transform playerTransform;
    private PlayerCharacter player;
    [Header("Animazioni Movimento")]


    [Header("Camera")]
    [SerializeField] private Transform mainCamera; //non basta assegnare la virtual camera, serve la MAIN
    [SerializeField] float rotationSpeed = 4f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public Vector3 velocity;
    public bool isGrounded;
    private void Awake()
    {
        player = GetComponent<PlayerCharacter>();
        //horizontalVelocity = 0f;
        playerController = GetComponent<CharacterController>();
        //HandleJumpVariables();
        //playerVector = new Vector3(0f, 0f, 0f);
        playerTransform = GetComponent<Transform>();
        UnityEngine.Cursor.visible = false;
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        //lastPositionAcquired = playerTransform.position;
    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if(isGrounded && velocity.y <0)
        {
            isJumpFalling = false;
            velocity.y = -0.5f; 
        }
        else if (isGrounded == false && velocity.y < 0)
        {
            isJumpFalling = true;
        }

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (player.isInputOn)
        {
            if (direction.magnitude >= 0.1f)
            {
                isMoving = true;   
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + mainCamera.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                playerController.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
            }
            else
            {
                isMoving = false;
            }

            if(Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(maxJumpHeight);
            }
            else
            {
                velocity.y += gravity * Time.deltaTime * Time.deltaTime;
            }

            playerController.Move(velocity);
            //ComputePlayerVelocity(playerTransform.position);

            //HandleGravity();
            //HandleJump();
            //if (!isAttacking)
            //    playerController.Move(playerVector * Time.deltaTime);

            //if (Input.GetKeyDown(KeyCode.Space) && canJumpAgain)
            //{
            //    isJumpPressed = true;
            //    //Debug.Log("Salto");

        }
    }


    //private void HandleJumpVariables()
    //{
    //    float timeToApex = maxJumpTime / 2;
    //    gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
    //    //Debug.Log("Gravit: " + gravity);
    //    initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    //    //Debug.Log("Velocit�: " + initialJumpVelocity);

    //}
    //private void HandleJump()
    //{
    //    //Debug.Log(isJumpAscension + ", " + isJumpPressed);
    //    if (isJumpPressed && playerController.isGrounded)
    //    {
    //        //Debug.Log("Jumping");
    //        //StartCoroutine(JumpAnimationTimeOffset());
    //        isJumpAscension = true;
    //        canJumpAgain = false;
    //        isJumpPressed = false;
    //        playerVector.y = initialJumpVelocity;


    //    }
    //    else if (isJumpAscension && verticalVelocity <= 1f && !playerController.isGrounded)
    //    {
    //        //Debug.Log("Falling");
    //        isJumpAscension = false;
    //        isJumpPeak = false;
    //        isJumpFalling = true;
    //    }
    //    else if (playerController.isGrounded && isJumpFalling)
    //    {
    //        isJumpFalling = false;
    //        StartCoroutine(WaitForJumpAgain());
    //    }
    //    else if (isJumpAscension && playerController.isGrounded)
    //    {
    //        isJumpAscension = false;
    //        isJumpPeak = false;
    //        isJumpFalling = true;
    //    }
    //}

    //private void HandleGravity()
    //{
    //    if (playerController.isGrounded)
    //    {

    //        playerVector.y = groundedGravity * Time.deltaTime;
    //    }
    //    else
    //    {
    //        playerVector.y += gravity * Time.deltaTime;
    //    }
    //    //Debug.Log("Funzione: " + playerVector.y);
    //}


    //private void ComputePlayerVelocity(Vector3 newPosition)
    //{
    //    Vector2 playerPosition = new Vector2(newPosition.x, newPosition.z);
    //    Vector2 lastPlayerPos = new Vector2(lastPositionAcquired.x, lastPositionAcquired.z);
    //    float playerPositionY = newPosition.y;
    //    float lastPlayerPosY = lastPositionAcquired.y;
    //    verticalVelocity = (playerPositionY - lastPlayerPosY) / Time.deltaTime;
    //    horizontalVelocity = (playerPosition - lastPlayerPos).magnitude / Time.deltaTime;
    //    lastPositionAcquired = newPosition;
    //}

    //IEnumerator WaitForJumpAgain()
    //{
    //    yield return new WaitForSeconds(1f);
    //    canJumpAgain = true;
    //}

    public bool GetIsJumpAscension()
    {
        if(velocity.y > 0.05f && isGrounded == false)
        {
            isJumpAscension = true;
        }
        else
        {
            isJumpAscension= false;
        }
        return isJumpAscension;
    }

    public bool GetIsJumpPeak()
    {
        if(isGrounded == false && (velocity.y >= -0.01f || velocity.y <= 0.01f))
        {
            isJumpPeak = true;
        }
        else
        {
            isJumpPeak = false;
        }
        return isJumpPeak;
    }
    public bool GetIsJumpFalling()
    {
        return isJumpFalling;
    }
    public bool GetIsMoving()
    {
        return isMoving && playerController.isGrounded;
    }

}





