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
    private float lastBaseAttack = 0;
    private int attacksDone = 0;
    [SerializeField] private float maxComboDelay = 1f;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private float nextActionTimer = 0.3f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float dodgeDistance = 1f;
    [SerializeField] private float dodgeRightDistance = 1f;
    [SerializeField] private float dodgeLeftDistance = 1f;
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

        if (attacksDone != 0 && Time.time - lastBaseAttack > maxComboDelay)
        {
            attacksDone = 0;
        }
        
        if (animator.GetBool("isBaseAttack") && 
            (animator.GetCurrentAnimatorStateInfo(0).IsName("isBaseAttack2") || 
             animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack")))
        {
            animator.SetBool("isBaseAttack", false);
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("isBaseAttack2"))
                animator.SetBool("isBaseAttack2", false);
        } /*else if (animator.GetBool("BaseAttack3") && animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack3"))
        {
            animator.SetBool("BaseAttack3", false);
        }*/
        
        if (animator.GetBool("isStrongAttack") && animator.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
        {
            animator.SetBool("isStrongAttack", false);
        }
        
        if (((Time.time >= nextActionTimer && attacksDone == 0) || attacksDone != 0) 
            && Input.GetKeyDown(KeyCode.Z))
        {
            /*if (attacksDone == 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("isBaseAttack2"))
            {
                animator.SetBool("BaseAttack3", true);
                attacksDone = 0;
                nextActionTimer = Time.time + cooldown;
            }
            else */if (attacksDone == 1 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack"))
            {
                // lastBaseAttack = Time.time;
                animator.SetBool("isBaseAttack", true);
                animator.SetBool("isBaseAttack2", true);
                attacksDone = 0;
                // attacksDone++;
                nextActionTimer = Time.time + cooldown;
            }
            else if (attacksDone == 0)
            {
                animator.SetBool("isBaseAttack", true);
                lastBaseAttack = Time.time;
                animator.SetTrigger("BaseAttack");
                attacksDone++;
                nextActionTimer = Time.time + cooldown;
            }

        }
        else if (Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.X))
        {
            animator.SetBool("isStrongAttack", true);
            StrongAttack();
            nextActionTimer = Time.time + cooldown;
        }
        else if (Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.C))
        {
            if (Input.GetKey(KeyCode.A))
            {
                LeftDodge();
            }
            else if (Input.GetKey(KeyCode.D))
            {
                RightDodge();
            }
            else if (Input.GetKey(KeyCode.S))
            {
                BackwardDodge();
            }
            else
            {
                ForwardDodge();
            }

            nextActionTimer = Time.time + cooldown;
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
        
        // // Normalizzare il vettore per garantire una velocità costante
        // if (movementDirection.magnitude > 1)
        // {
        //     movementDirection.Normalize();
        // }
        // if (movementDirection != Vector3.zero)
        // {
        //     Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime);
        // }
        //
        // // Muovere il personaggio
        // transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

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
        animator.SetTrigger("Death");
        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
        Debug.Log("DIED");
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.5f);
        UpdateHP(MAX_HP);
        currentScenario = defaultScenario;
        gameObject.transform.position = currentScenario.respawnPoint;
        Debug.Log("RESPAWNED");
    }

    private void DoDamage(object sender, EnemyCollisionArgs args)
    {
        args.enemy.TakeDamage(stats.atk + args.hitter.atk - args.enemy.def, activeRxElement, args.hitter.GetComponentInParent<LayerMask>());
        //Da sistemare perché ora viene passato solo l'elemento del braccio destro
    }

    private void StrongAttack()
    {
        animator.SetTrigger("StrongAttack");
        Debug.Log("Strong Attack done!");
    }

    private void RightDodge()
    {
        animator.SetTrigger("RightDodge");
        StartCoroutine(DodgeCoroutine(dodgeRightDistance));
    }
    
    private void LeftDodge()
    {
        animator.SetTrigger("LeftDodge");
        StartCoroutine(DodgeCoroutine(dodgeLeftDistance));
    }
    
    private void ForwardDodge()
    {
        animator.SetTrigger("ForwardDodge");
        StartCoroutine(DodgeCoroutine(dodgeDistance));
    }
    
    private IEnumerator DodgeCoroutine(float dodgeDistance)
    {
        float animationDuration = 1f;
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
        Debug.Log("Dodge done!");
    }
    
    private void BackwardDodge()
    {
        animator.SetTrigger("BackwardDodge");
        StartCoroutine(DodgeCoroutine(dodgeDistance));
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