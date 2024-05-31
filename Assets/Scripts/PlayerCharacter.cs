using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Transactions;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum PartType
{
    Head,
    Body,
    RightArm,
    LeftArm,
    Legs
}

public class PlayerCharacter : Character
{
    public float MAX_HP = 100;
    public float def_HP = 100;
    //public Slider sliderHP;

    [SerializeField] private List<Piece> headList;
    [SerializeField] private List<Piece> leftArmList;
    [SerializeField] private List<Piece> rightArmList;
    [SerializeField] private List<Piece> bodyList;
    [SerializeField] private List<Piece> legsList;
    // lista COMPLETA dei PEZZI presenti nel modello da disabilitare o abilitare
    public Dictionary<PartType, List<Piece>> completePiecesList = new Dictionary<PartType, List<Piece>>();
    
    //lista attuale dei soli pezzi attivati nel modello
    public Dictionary<PartType, Piece> composition = new Dictionary<PartType, Piece>();
    public Accessory accessory;
    
    public Animator animator;
    public float attackRange;
    [NonSerialized] public LayerMask enemyLayer;
    public float attackRate = 2f;
    [SerializeField] private float nextActionTimer = 1f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float dodgeDistance = 10f;
    private Vector3 movementDirection;
    public float maxDistanceNPC = 5f;
    public LayerMask npcLayer;

    private Element activeRxElement;//Elemento nel braccio destro
    private Element activeSxElement;//Elemento nel braccio sinistro


    [NonSerialized] public Scenario currentScenario;
    [NonSerialized] public Scenario defaultScenario;

    public static EventHandler OnPlayerDeath;
    public static EventHandler<ScenarioArgs> OnScenarioBegin;
    public static EventHandler OnChoicePieces;
    public static EventHandler OnEndChoicePieces;

    public static System.Action OnUpdate;

    //Aggiunte per la Healthbar
    private float lerpTimer;
    [SerializeField] float chipSpeed;
    public Image frontHealthBar;
    public Image backHealthBar;
    //public Image characterIcon;
    public Image[] icons;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject abilitiesSection;

    // Start is called before the first frame update
    public void Awake()
    {
        base.Awake();
        isPlayer = true;

        //if (sliderHP)
        //{
        //    sliderHP.maxValue = MAX_HP;
        //}
        
        UpdateHP(def_HP);
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");

        completePiecesList[PartType.Head] = headList;
        completePiecesList[PartType.LeftArm] = leftArmList;
        completePiecesList[PartType.RightArm] = rightArmList;
        completePiecesList[PartType.Body] = bodyList;
        completePiecesList[PartType.Legs] = legsList;
        composition[PartType.Head] = completePiecesList[PartType.Head][0];
        composition[PartType.LeftArm] = completePiecesList[PartType.LeftArm][0];
        composition[PartType.RightArm] = completePiecesList[PartType.RightArm][0];
        composition[PartType.Body] = completePiecesList[PartType.Body][0];
        composition[PartType.Legs] = completePiecesList[PartType.Legs][0];
        
        Weapon.OnEnemyCollision += DoDamage;
        ChoicePieceManager.OnChangePiece += ModifyComposition;
    }

    public void Update()
    {
        if(OnUpdate != null) OnUpdate();

        if (Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.Z))
        {
            BaseAttack();
            nextActionTimer = Time.time + 1f;
        }
        else if (Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.X))
        {
            StrongAttack();
            nextActionTimer = Time.time + 1f;
        }
        else if (Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.A))
            {
                LeftDodge();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                BackwardDodge();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                RightDodge();
            }
            else
            {
                ForwardDodge();
            }
        }
        // Raccogliere gli input dai tasti WASD
        float moveHorizontal = 0f;
        float moveVertical = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            moveVertical += 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVertical -= 1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal -= 1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal += 1f;
        }

        // Creare il vettore di movimento combinando gli input
        movementDirection = new Vector3(moveHorizontal, 0f, moveVertical);

        // Normalizzare il vettore per garantire una velocità costante
        if (movementDirection.magnitude > 1)
        {
            movementDirection.Normalize();
        }
        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime);
        }

        // Muovere il personaggio
        transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

        def_HP = Mathf.Clamp(currentHP, 0, MAX_HP);
        UpdateHPUI();

        CheckForNPC();
    }

    public override void UpdateHP(float newHP)
    {
        base.UpdateHP(newHP);
        lerpTimer = 0f;
    }
    
    public void UpdateHPUI()
    {
        if (frontHealthBar && backHealthBar)
        {
            float fillFront = frontHealthBar.fillAmount;
            float fillBack = backHealthBar.fillAmount;
            float healthFraction = currentHP / MAX_HP;

            if (healthFraction >= 0.75f)
            {
                //characterIcon.color = new Color(characterIcon.color.r, characterIcon.color.g, characterIcon.color.b, 1f); //Cambio trasparenza
                //characterIcon.color = Color.green; //Cambio colore
                icons[0].enabled = true; //Cambio sprite
                icons[1].enabled = false;

            }

            else if (healthFraction >= 0.50f && healthFraction < 0.75f)
            {
                //characterIcon.color = new Color(characterIcon.color.r, characterIcon.color.g, characterIcon.color.b, 0.8f);
                //characterIcon.color = Color.yellow;
                icons[0].enabled = false; //Cambio sprite
                icons[1].enabled = true;
            }
            //else if (healthFraction >= 0.25f && healthFraction < 0.50f)
            //{
            //    characterIcon.color.WithAlpha(healthFraction);
            //}
            //else if (healthFraction < 0.25f)
            //{
            //    characterIcon.color.WithAlpha(healthFraction);
            //}

            if (fillBack > healthFraction)
            {
                frontHealthBar.fillAmount = healthFraction;
                backHealthBar.color = Color.red;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
            }

            if (fillFront < healthFraction)
            {
                backHealthBar.fillAmount = healthFraction;
                backHealthBar.color = Color.yellow;
                lerpTimer += Time.deltaTime;
                float percentComplete = lerpTimer / chipSpeed;
                percentComplete = percentComplete * percentComplete;
                frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
            }

            //if(sliderHP)
            //    sliderHP.value = HP;
        }
    }

    public override void Die()
    {
        base.Die();
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        Debug.Log("DIED");
        Respawn();
    }

    public void Respawn()
    {
        UpdateHP(MAX_HP);
        currentScenario = defaultScenario;
        gameObject.transform.position = currentScenario.respawnPoint;
        Debug.Log("RESPAWNED");
    }

    private void DoDamage(object sender, EnemyCollisionArgs args)
    {
        args.enemy.TakeDamage(stats.atk + args.hitter.atk - args.enemy.def, activeRxElement);
        //Da sistemare perché ora viene passato solo l'elemento del braccio destro
    }
    
    private void BaseAttack()
    {
        animator.SetTrigger("BaseAttack");
        Debug.Log("Base Attack done!");
    }
    
    private void StrongAttack()
    {
        animator.SetTrigger("StrongAttack");
        Debug.Log("Strong Attack done!");
    }

    private void RightDodge()
    {
        animator.SetTrigger("RightDodge");
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Vector3.Lerp(transform.position, transform.position + movementDirection, animationDuration);
        Debug.Log("Right Dodge done!");
    }
    
    private void LeftDodge()
    {
        animator.SetTrigger("LeftDodge");
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Vector3.Lerp(transform.position, transform.position + movementDirection, animationDuration);
        Debug.Log("Left Dodge done!");
    }
    
    private void ForwardDodge()
    {
        animator.SetTrigger("ForwardDodge");
        StartCoroutine(DodgeCoroutine());
    }
    
    private IEnumerator DodgeCoroutine()
    {
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length-1;
        Vector3 startPosition = transform.position;
        Vector3 dodgeDirection;
        if (movementDirection == Vector3.zero)
        {
            dodgeDirection = new Vector3(0,0,1);
        }
        else
        {
            dodgeDirection = movementDirection;
        }
        Vector3 endPosition = startPosition + dodgeDirection * dodgeDistance;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
        Debug.Log("Forward Dodge done!");
    }
    
    private void BackwardDodge()
    {
        animator.SetTrigger("BackwardDodge");
        float animationDuration = animator.GetCurrentAnimatorStateInfo(0).length;
        Vector3.Lerp(transform.position, transform.position + movementDirection, animationDuration);
        Debug.Log("Backward Dodge done!");
    }

    private void ModifyComposition(object sender, ChangePieceArgs args)
    {
        completePiecesList[args.partType][args.oldPieceNumber].gameObject.SetActive(false);
        completePiecesList[args.partType][args.newPieceNumber].gameObject.SetActive(true);
        Piece selectedPiece = completePiecesList[args.partType][args.newPieceNumber];
        composition[args.partType] = selectedPiece;
    }

    void CheckForNPC()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit,
                maxDistanceNPC, npcLayer)
            && raycastHit.transform.TryGetComponent(out NPC npc)
            && Input.GetKeyDown(KeyCode.T))
        {
            OnChoicePieces?.Invoke(this, EventArgs.Empty);
            GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            healthBar.SetActive(false);
            abilitiesSection.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Y))
        {
            OnEndChoicePieces?.Invoke(this, EventArgs.Empty);
            GetComponent<PlayerMovement>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            healthBar.SetActive(true);
            abilitiesSection.SetActive(true);
        }
    }
    
}

public class ScenarioArgs : EventArgs
{
    public ScenarioArgs(Scenario a)
    {
        scenario = a;
    }
    public Scenario scenario;
}