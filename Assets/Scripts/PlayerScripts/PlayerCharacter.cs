
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

public enum PartType //GUAI A CHI CAMBIA L'ORDINE
{
    Head,
    Body,
    RightArm,
    LeftArm,
    Legs,
    Weapon
}

public class PlayerCharacter : Character
{
    public float MAX_HP = 100;
    public float def_HP = 100;

    [SerializeField] private List<Piece> headList;
    [SerializeField] private List<Piece> leftArmList;
    [SerializeField] private List<Piece> rightArmList;
    [SerializeField] private List<Piece> bodyList;
    [SerializeField] private List<Piece> legsList;
    [SerializeField] private List<Piece> weaponList;
    // lista COMPLETA dei PEZZI presenti nel modello da disabilitare o abilitare
    public Dictionary<PartType, List<Piece>> completePiecesList = new Dictionary<PartType, List<Piece>>();
    
    //lista attuale dei soli pezzi attivati nel modello
    public Dictionary<PartType, Piece> composition = new Dictionary<PartType, Piece>();
    public Accessory accessory;

    [NonSerialized] public bool isInputOn = true;
    [NonSerialized] public bool isFighting = false;
    private int strongAttackIndex = 0;
    [NonSerialized] public LayerMask enemyLayer;
    public HashSet<int> enemiesHit = new HashSet<int>();

    public Animator animator;
    public float attackRange;
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
    [SerializeField] private ChoicePieceManager choicePieceManager;
    [Tooltip("Lo script è assegnato al prefab HUD")]
    [SerializeField] private PauseMenu pauseMenu;

    private Element activeRxElement;//Elemento nel braccio destro
    private Element activeSxElement;//Elemento nel braccio sinistro


    [NonSerialized] public Scenario currentScenario;
    [NonSerialized] public Scenario defaultScenario;

    public static EventHandler OnPlayerDeath;
    public static EventHandler<ScenarioArgs> OnScenarioBegin;
    public static EventHandler OnChoicePieces;
    public static EventHandler OnEndChoicePieces;

    public static Action OnUpdate;


    [Header("HealthBar")]
    private float lerpTimer;
    [SerializeField] float chipSpeed;
    [Tooltip("Inserire le sprites presenti nella HUD")]
    [SerializeField] private Image frontHealthBar;
    [SerializeField] private Image backHealthBar;
    [Tooltip("Inserire le sprites del volto di Cyrus")]
    [SerializeField] private Image[] icons;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject abilitiesSection;

    public void Awake()
    {
        base.Awake();
        isPlayer = true;
        
        UpdateHP(def_HP);
        animator = GetComponent<Animator>();
        enemyLayer = LayerMask.GetMask("Enemy");

        completePiecesList[PartType.Head] = headList;
        completePiecesList[PartType.LeftArm] = leftArmList;
        completePiecesList[PartType.RightArm] = rightArmList;
        completePiecesList[PartType.Body] = bodyList;
        completePiecesList[PartType.Legs] = legsList;
        completePiecesList[PartType.Weapon] = weaponList;
        composition[PartType.Head] = completePiecesList[PartType.Head][0];
        composition[PartType.LeftArm] = completePiecesList[PartType.LeftArm][0];
        composition[PartType.RightArm] = completePiecesList[PartType.RightArm][0];
        composition[PartType.Body] = completePiecesList[PartType.Body][0];
        composition[PartType.Legs] = completePiecesList[PartType.Legs][0];
        composition[PartType.Weapon] = completePiecesList[PartType.Weapon][0];
        InitializeComposition();
        
        choicePieceManager = GameObject.Find("ChoicePiecesManager").GetComponent<ChoicePieceManager>();
        
        Weapon.OnEnemyCollision += DoDamage;
        ChoicePieceManager.OnChangePiece += ModifyComposition;
        ChoicePieceManager.OnSetPiece += SetPieceComposition;
        BaseAttack1State.OnClearEnemyHitList += ClearEnemyHitList;
        BaseAttack2State.OnClearEnemyHitList += ClearEnemyHitList;
        StrongAttackState.OnClearEnemyHitList += ClearEnemyHitList;

        animator.SetInteger("strongAttackIndex", 0);
    }

    void OnDestroy()
    {
        Weapon.OnEnemyCollision -= DoDamage;
        ChoicePieceManager.OnChangePiece -= ModifyComposition;
        ChoicePieceManager.OnSetPiece -= SetPieceComposition;
        BaseAttack1State.OnClearEnemyHitList -= ClearEnemyHitList;
        BaseAttack2State.OnClearEnemyHitList -= ClearEnemyHitList;
        StrongAttackState.OnClearEnemyHitList -= ClearEnemyHitList;
    }

    public void Update()
    {
        if(OnUpdate != null) OnUpdate();

        isInputOn = !isFighting && !choicePieceManager.isUIOpen && !pauseMenu.isUIOpen; //UGUALE A SCRIVERE IF( isFightin == false && choice... == false && pause... == false) isInputOn=true else isInputOn =false;
        
        /*if (isFighting || choicePieceManager.isUIOpen || pauseMenu.isUIOpen)
            isInputOn = false;                                                  CONVENIVA METTERE LA CONDIZIONE SUL TRUE CON &&, COME FATTO 2 RIGHE SOPRA
        else
            isInputOn = true;
        */
        //Debug.Log(isFighting);

        //DA SISTEMARE POI CON UN ENUM E UNO SWITCH SE ABBIAMO TEMPO
        if (attacksDone != 0 && Time.time - lastBaseAttack > maxComboDelay)
        {
            attacksDone = 0;
        }
        
        if (animator.GetBool("isBaseAttack") && animator.GetCurrentAnimatorStateInfo(0).IsName("Cyrus_Cosmos_Rig_Cyrus_Attacco_Leggero#1_Anticipation"))
        {
            animator.SetBool("isBaseAttack", false);
           SetFightingState(false);
        }
        if (animator.GetBool("isBaseAttack2") && animator.GetCurrentAnimatorStateInfo(0).IsName("Cyrus_Cosmos_Rig_Cyrus_Attacco_Leggero#2_Recovery"))
        {
            animator.SetBool("isBaseAttack2", false);
            SetFightingState(false);
        }
        if (animator.GetBool("isStrongAttack") && 
            (   animator.GetCurrentAnimatorStateInfo(0).IsName("Cyrus_Cosmos_Rig_Cyrus_Attacco_Pesante_#2_Anticipation")
            || animator.GetCurrentAnimatorStateInfo(0).IsName("Cyrus_Cosmos_Rig_Cyrus_Attacco_Pesante_v2_Anticipation"))) // animator.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
        {
            animator.SetBool("isStrongAttack", false);
            SetFightingState(false);
        }
        
        if ((isInputOn || isFighting)
            && ((Time.time >= nextActionTimer && attacksDone == 0) || attacksDone != 0)
            && Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (attacksDone == 0)
            {
                animator.SetBool("isBaseAttack", true); 
                SetFightingState(true);
                lastBaseAttack = Time.time;
                attacksDone++;
                nextActionTimer = Time.time + cooldown;
            }
            else if (attacksDone == 1 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
                animator.GetCurrentAnimatorStateInfo(0).IsName("Cyrus_Cosmos_Rig_Cyrus_Attacco_Leggero#1_Action"))
            {
               // animator.SetBool("isBaseAttack2", true);
                animator.Play("Cyrus_Cosmos_Rig_Cyrus_Attacco_Leggero#2_Anticipation");
                SetFightingState(true);
                attacksDone = 0;
                nextActionTimer = Time.time + cooldown;
            }

        }
        else if ((isInputOn || isFighting)
            && Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.Mouse1))
               // && animator.GetCurrentAnimatorStateInfo(0).IsName("Cyrus_Cosmos_Rig_Cyrus_Attacco_Pesante_#2_Anticipation"))
        {
           // Debug.Log(strongAttackIndex);
            animator.SetInteger("strongAttackIndex", strongAttackIndex);
            animator.SetBool("isStrongAttack", true);
            SetFightingState(true);
            //StrongAttack(); questa è solo u debug
            nextActionTimer = Time.time + cooldown;
            strongAttackIndex++; //soluzione provvisoria per scegliere uno dei due attacchi pesanti a caso, non riesco ad importare numeri random
            if (strongAttackIndex == 2) strongAttackIndex = 0;
        }
        
        else if (isInputOn && Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.C))
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
        if(stats.atk > args.enemy.def)
        {
           // Debug.Log(stats.atk + args.hitter.atk - args.enemy.def + activeRxElement);
            args.enemy.TakeDamage(stats.atk + args.hitter.atk - args.enemy.def, activeRxElement);
        }
            
       
      
        
        //Da sistemare perché ora viene passato solo l'elemento del braccio destro
    }

    private void ClearEnemyHitList()
    {
        enemiesHit.Clear();
    }

    private void StrongAttack()
    {
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

    private void BackwardDodge()
    {
        animator.SetTrigger("BackwardDodge");
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
 
    private void InitializeComposition()
    {
        foreach(Piece p in completePiecesList[PartType.Head])
            if(p != composition[PartType.Head])
                p.gameObject.SetActive(false);
        
        foreach(Piece p in completePiecesList[PartType.LeftArm])
            if(p != composition[PartType.LeftArm])
                p.gameObject.SetActive(false);
        
        foreach(Piece p in completePiecesList[PartType.RightArm])
            if(p != composition[PartType.RightArm])
                p.gameObject.SetActive(false);
        
        foreach(Piece p in completePiecesList[PartType.Body])
            if(p != composition[PartType.Body])
                p.gameObject.SetActive(false);
        
        foreach(Piece p in completePiecesList[PartType.Legs])
            if(p != composition[PartType.Legs])
                p.gameObject.SetActive(false);
        
        foreach(Piece p in completePiecesList[PartType.Weapon])
            if(p != composition[PartType.Weapon])
                p.gameObject.SetActive(false);
        
    }
    
    private void ModifyComposition(object sender, ChangePieceArgs args) //evento invocato quando viene cambiato un pezzo
    {
        completePiecesList[args.partType][args.oldPieceNumber].gameObject.SetActive(false);
        completePiecesList[args.partType][args.newPieceNumber].gameObject.SetActive(true); //questi lavorano sui modelli
        Piece selectedPiece = completePiecesList[args.partType][args.newPieceNumber];
        composition[args.partType] = selectedPiece;///quale pezzo corrisponde al tipo di parte del corpo
    }
    
    private void SetPieceComposition(object sender, SetPieceArgs args)
    {
        completePiecesList[args.partType][args.pieceNumber].gameObject.SetActive(true);
        Piece selectedPiece = completePiecesList[args.partType][args.pieceNumber];
        composition[args.partType] = selectedPiece;
    }

    void CheckForNPC()
    {
        Ray ray = new Ray();
        RaycastHit[] raycastHit;
        raycastHit = Physics.SphereCastAll(transform.position, 2f, Vector3.forward, maxDistanceNPC, npcLayer);
        if (raycastHit.Length > 0 && raycastHit[0].collider
            && raycastHit[0].transform.TryGetComponent(out NPC npc)
            && Input.GetKeyDown(KeyCode.E)
            && !choicePieceManager.isUIOpen)
        {
            OnChoicePieces?.Invoke(this, EventArgs.Empty);
            GetComponent<PlayerMovement>().enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            healthBar.SetActive(false);
            abilitiesSection.SetActive(false);
        }
        else if (choicePieceManager.isUIOpen && Input.GetKeyDown(KeyCode.E))
        {
            OnEndChoicePieces?.Invoke(this, EventArgs.Empty);
            GetComponent<PlayerMovement>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            healthBar.SetActive(true);
            abilitiesSection.SetActive(true);
        }
    }

    public void SetFightingState(bool state)
    {
        isFighting = state;
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




//VERSIONE PRECEDENTE
//using JetBrains.Annotations;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using System.Transactions;
//using TMPro;
//using Unity.VisualScripting;
//using UnityEngine;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;

//public enum PartType
//{
//    Head,
//    Body,
//    RightArm,
//    LeftArm,
//    Legs,
//    Weapon
//}

//public class PlayerCharacter : Character
//{
//    public float MAX_HP = 100;
//    public float def_HP = 100;
//    //public Slider sliderHP;

//    [SerializeField] private List<Piece> headList;
//    [SerializeField] private List<Piece> leftArmList;
//    [SerializeField] private List<Piece> rightArmList;
//    [SerializeField] private List<Piece> bodyList;
//    [SerializeField] private List<Piece> legsList;
//    [SerializeField] private List<Piece> weaponList;
//    // lista COMPLETA dei PEZZI presenti nel modello da disabilitare o abilitare
//    public Dictionary<PartType, List<Piece>> completePiecesList = new Dictionary<PartType, List<Piece>>();

//    //lista attuale dei soli pezzi attivati nel modello
//    public Dictionary<PartType, Piece> composition = new Dictionary<PartType, Piece>();
//    public Accessory accessory;

//    [NonSerialized] public bool isInputOn = true;
//    [NonSerialized] public bool isFighting = false;
//    public Animator animator;
//    public float attackRange;
//    [NonSerialized] public LayerMask enemyLayer;
//    private float lastBaseAttack = 0;
//    private int attacksDone = 0;
//    [SerializeField] private float maxComboDelay = 1f;
//    [SerializeField] private float cooldown = 0.5f;
//    [SerializeField] private float nextActionTimer = 0.3f;
//    [SerializeField] private float speed = 5f;
//    [SerializeField] private float dodgeDistance = 1f;
//    [SerializeField] private float dodgeRightDistance = 1f;
//    [SerializeField] private float dodgeLeftDistance = 1f;
//    private Vector3 movementDirection;
//    public float maxDistanceNPC = 5f;
//    public LayerMask npcLayer;
//    [SerializeField] private ChoicePieceManager choicePieceManager;
//    [SerializeField] private PauseMenu pauseMenu;

//    private Element activeRxElement;//Elemento nel braccio destro
//    private Element activeSxElement;//Elemento nel braccio sinistro

//    //private int damageTaken;

//    [NonSerialized] public Scenario currentScenario;
//    [NonSerialized] public Scenario defaultScenario;

//    public static EventHandler OnPlayerDeath;
//    public static EventHandler<ScenarioArgs> OnScenarioBegin;
//    public static EventHandler OnChoicePieces;
//    public static EventHandler OnEndChoicePieces;

//    public static System.Action OnUpdate;

//    //Aggiunte per la Healthbar
//    private float lerpTimer;
//    [SerializeField] float chipSpeed;
//    public Image frontHealthBar;
//    public Image backHealthBar;
//    //public Image characterIcon;
//    public Image[] icons;
//    [SerializeField] private GameObject healthBar;
//    [SerializeField] private GameObject abilitiesSection;

//    // Start is called before the first frame update
//    public void Awake()
//    {
//        base.Awake();
//        isPlayer = true;

//        //if (sliderHP)
//        //{
//        //    sliderHP.maxValue = MAX_HP;
//        //}

//        UpdateHP(def_HP);
//        animator = GetComponent<Animator>();
//        enemyLayer = LayerMask.GetMask("Enemy");

//        completePiecesList[PartType.Head] = headList;
//        completePiecesList[PartType.LeftArm] = leftArmList;
//        completePiecesList[PartType.RightArm] = rightArmList;
//        completePiecesList[PartType.Body] = bodyList;
//        completePiecesList[PartType.Legs] = legsList;
//        completePiecesList[PartType.Weapon] = weaponList;
//        composition[PartType.Head] = completePiecesList[PartType.Head][0];
//        composition[PartType.LeftArm] = completePiecesList[PartType.LeftArm][0];
//        composition[PartType.RightArm] = completePiecesList[PartType.RightArm][0];
//        composition[PartType.Body] = completePiecesList[PartType.Body][0];
//        composition[PartType.Legs] = completePiecesList[PartType.Legs][0];
//        composition[PartType.Weapon] = completePiecesList[PartType.Weapon][0];
//        InitializeComposition();

//        choicePieceManager = GameObject.Find("ChoicePiecesManager").GetComponent<ChoicePieceManager>();

//        Weapon.OnEnemyCollision += DoDamage;
//        ChoicePieceManager.OnChangePiece += ModifyComposition;
//        ChoicePieceManager.OnSetPiece += SetPieceComposition;
//    }

//    public void Update()
//    {
//        if (OnUpdate != null) OnUpdate();

//        if (isFighting || choicePieceManager.isUIOpen || pauseMenu.isUIOpen)
//            isInputOn = false;
//        else
//            isInputOn = true;

//        if (attacksDone != 0 && Time.time - lastBaseAttack > maxComboDelay)
//        {
//            attacksDone = 0;
//        }

//        // if (animator.GetBool("isBaseAttack") && 
//        //     (animator.GetCurrentAnimatorStateInfo(0).IsName("isBaseAttack2") || 
//        //      animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack")))
//        // {
//        //     animator.SetBool("isBaseAttack", false);
//        //     if(animator.GetCurrentAnimatorStateInfo(0).IsName("isBaseAttack2"))
//        //         animator.SetBool("isBaseAttack2", false);
//        // } /*else if (animator.GetBool("BaseAttack3") && animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack3"))
//        // {
//        //     animator.SetBool("BaseAttack3", false);
//        // }*/
//        if (animator.GetBool("isBaseAttack") && animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack"))
//        {
//            animator.SetBool("isBaseAttack", false);
//            //SetFightingState("False");
//        }
//        if (animator.GetBool("isBaseAttack2") && animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack2"))
//        {
//            animator.SetBool("isBaseAttack2", false);
//            //SetFightingState("False");
//        }
//        if (animator.GetBool("isStrongAttack") && animator.GetCurrentAnimatorStateInfo(0).IsName("StrongAttack"))
//        {
//            animator.SetBool("isStrongAttack", false);
//            //SetFightingState("False");
//        }

//        if ((isInputOn || isFighting)                    //(isInputOn || (!isInputOn && isFighting)) 
//            && ((Time.time >= nextActionTimer && attacksDone == 0) || attacksDone != 0)
//            && Input.GetKeyDown(KeyCode.Mouse0))
//        {
//            /*if (attacksDone == 2 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
//                animator.GetCurrentAnimatorStateInfo(0).IsName("isBaseAttack2"))
//            {
//                animator.SetBool("BaseAttack3", true);
//                attacksDone = 0;
//                nextActionTimer = Time.time + cooldown;
//            }
//            else */
//            if (attacksDone == 0)
//            {
//                animator.SetBool("isBaseAttack", true);
//                // SetFightingState("True");
//                lastBaseAttack = Time.time;
//                attacksDone++;
//                nextActionTimer = Time.time + cooldown;
//            }
//            else if (attacksDone == 1 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
//                animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack"))
//            {
//                // lastBaseAttack = Time.time;
//                // animator.SetBool("isBaseAttack", true);
//                animator.SetBool("isBaseAttack2", true);
//                //SetFightingState("True");
//                attacksDone = 0;
//                // attacksDone++;
//                nextActionTimer = Time.time + cooldown;
//            }

//            /*if (attacksDone == 1 && animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.5f &&
//                animator.GetCurrentAnimatorStateInfo(0).IsName("BaseAttack"))
//            {
//                // lastBaseAttack = Time.time;
//                // animator.SetBool("isBaseAttack", true);
//                animator.SetBool("isBaseAttack2", true);
//                //SetFightingState("True");
//                attacksDone = 0;
//                // attacksDone++;
//                nextActionTimer = Time.time + cooldown;
//            }
//            else if (attacksDone == 0)
//            {
//                animator.SetBool("isBaseAttack", true);
//               // SetFightingState("True");
//                lastBaseAttack = Time.time;
//                attacksDone++;
//                nextActionTimer = Time.time + cooldown;
//            }*/

//        }
//        else if ((isInputOn || isFighting)//((isInputOn || (!isInputOn && isFighting))
//            && Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.Mouse1))
//        {
//            animator.SetBool("isStrongAttack", true);
//            //SetFightingState("True");
//            StrongAttack();
//            nextActionTimer = Time.time + cooldown;
//        }

//        else if (isInputOn && Time.time >= nextActionTimer && Input.GetKeyDown(KeyCode.C))
//        {
//            if (Input.GetKey(KeyCode.A))
//            {
//                LeftDodge();
//            }
//            else if (Input.GetKey(KeyCode.D))
//            {
//                RightDodge();
//            }
//            else if (Input.GetKey(KeyCode.S))
//            {
//                BackwardDodge();
//            }
//            else
//            {
//                ForwardDodge();
//            }

//            nextActionTimer = Time.time + cooldown;
//        }
//        // // Raccogliere gli input dai tasti WASD
//        // float moveHorizontal = 0f;
//        // float moveVertical = 0f;
//        //
//        // if (Input.GetKey(KeyCode.W))
//        // {
//        //     moveVertical += 1f;
//        // }
//        // if (Input.GetKey(KeyCode.S))
//        // {
//        //     moveVertical -= 1f;
//        // }
//        // if (Input.GetKey(KeyCode.A))
//        // {
//        //     moveHorizontal -= 1f;
//        // }
//        // if (Input.GetKey(KeyCode.D))
//        // {
//        //     moveHorizontal += 1f;
//        // }
//        //
//        // // Creare il vettore di movimento combinando gli input
//        // movementDirection = new Vector3(moveHorizontal, 0f, moveVertical);
//        //
//        // // Normalizzare il vettore per garantire una velocità costante
//        // if (movementDirection.magnitude > 1)
//        // {
//        //     movementDirection.Normalize();
//        // }
//        // if (movementDirection != Vector3.zero)
//        // {
//        //     Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
//        //     transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, speed * Time.deltaTime);
//        // }
//        //
//        // // Muovere il personaggio
//        // transform.Translate(movementDirection * speed * Time.deltaTime, Space.World);

//        def_HP = Mathf.Clamp(currentHP, 0, MAX_HP);
//        UpdateHPUI();

//        CheckForNPC();
//    }

//    public override void UpdateHP(float newHP)
//    {
//        base.UpdateHP(newHP);
//        lerpTimer = 0f;
//    }

//    public void UpdateHPUI()
//    {
//        if (frontHealthBar && backHealthBar)
//        {
//            float fillFront = frontHealthBar.fillAmount;
//            float fillBack = backHealthBar.fillAmount;
//            float healthFraction = currentHP / MAX_HP;

//            if (healthFraction >= 0.75f)
//            {
//                //characterIcon.color = new Color(characterIcon.color.r, characterIcon.color.g, characterIcon.color.b, 1f); //Cambio trasparenza
//                //characterIcon.color = Color.green; //Cambio colore
//                //icons[0].enabled = true; //Cambio sprite
//                //icons[1].enabled = false;

//            }

//            //else if (healthFraction >= 0.50f && healthFraction < 0.75f)
//            //{
//            //    characterIcon.color = new Color(characterIcon.color.r, characterIcon.color.g, characterIcon.color.b, 0.8f);
//            //    characterIcon.color = Color.yellow;
//            //    icons[0].enabled = false; //Cambio sprite
//            //    icons[1].enabled = true;
//            //}
//            //else if (healthFraction >= 0.25f && healthFraction < 0.50f)
//            //{
//            //    characterIcon.color.WithAlpha(healthFraction);
//            //}
//            //else if (healthFraction < 0.25f)
//            //{
//            //    characterIcon.color.WithAlpha(healthFraction);
//            //}

//            if (fillBack > healthFraction)
//            {
//                frontHealthBar.fillAmount = healthFraction;
//                backHealthBar.color = Color.red;
//                lerpTimer += Time.deltaTime;
//                float percentComplete = lerpTimer / chipSpeed;
//                percentComplete = percentComplete * percentComplete;
//                backHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
//            }

//            if (fillFront < healthFraction)
//            {
//                backHealthBar.fillAmount = healthFraction;
//                backHealthBar.color = Color.yellow;
//                lerpTimer += Time.deltaTime;
//                float percentComplete = lerpTimer / chipSpeed;
//                percentComplete = percentComplete * percentComplete;
//                frontHealthBar.fillAmount = Mathf.Lerp(fillFront, backHealthBar.fillAmount, percentComplete);
//            }

//            //if(sliderHP)
//            //    sliderHP.value = HP;
//        }
//    }

//    public override void Die()
//    {
//        base.Die();
//        animator.SetTrigger("Death");
//        OnPlayerDeath?.Invoke(this, EventArgs.Empty);
//        Debug.Log("DIED");
//        StartCoroutine(Respawn());
//    }

//    IEnumerator Respawn()
//    {
//        yield return new WaitForSeconds(1.5f);
//        UpdateHP(MAX_HP);
//        currentScenario = defaultScenario;
//        gameObject.transform.position = currentScenario.respawnPoint;
//        Debug.Log("RESPAWNED");
//    }

//    private void DoDamage(object sender, EnemyCollisionArgs args)
//    {
//        if (stats.atk > args.enemy.def)
//        {
//            Debug.Log(stats.atk + args.hitter.atk - args.enemy.def + activeRxElement);
//            args.enemy.TakeDamage(stats.atk + args.hitter.atk - args.enemy.def, activeRxElement);
//        }




//        //Da sistemare perché ora viene passato solo l'elemento del braccio destro
//    }

//    private void StrongAttack()
//    {
//        Debug.Log("Strong Attack done!");
//    }

//    private void RightDodge()
//    {
//        animator.SetTrigger("RightDodge");
//        StartCoroutine(DodgeCoroutine(dodgeRightDistance));
//    }

//    private void LeftDodge()
//    {
//        animator.SetTrigger("LeftDodge");
//        StartCoroutine(DodgeCoroutine(dodgeLeftDistance));
//    }

//    private void ForwardDodge()
//    {
//        animator.SetTrigger("ForwardDodge");
//        StartCoroutine(DodgeCoroutine(dodgeDistance));
//    }

//    private IEnumerator DodgeCoroutine(float dodgeDistance)
//    {
//        float animationDuration = 1f;
//        Vector3 startPosition = transform.position;
//        Vector3 dodgeDirection;
//        if (movementDirection == Vector3.zero)
//        {
//            dodgeDirection = new Vector3(0, 0, 1);
//        }
//        else
//        {
//            dodgeDirection = movementDirection;
//        }
//        Vector3 endPosition = startPosition + dodgeDirection * dodgeDistance;
//        float elapsedTime = 0f;

//        while (elapsedTime < animationDuration)
//        {
//            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / animationDuration);
//            elapsedTime += Time.deltaTime;
//            yield return null;
//        }

//        transform.position = endPosition;
//        Debug.Log("Dodge done!");
//    }

//    private void BackwardDodge()
//    {
//        animator.SetTrigger("BackwardDodge");
//        StartCoroutine(DodgeCoroutine(dodgeDistance));
//    }

//    private void InitializeComposition()
//    {
//        foreach (Piece p in completePiecesList[PartType.Head])
//            if (p != composition[PartType.Head])
//                p.gameObject.SetActive(false);

//        foreach (Piece p in completePiecesList[PartType.LeftArm])
//            if (p != composition[PartType.LeftArm])
//                p.gameObject.SetActive(false);

//        foreach (Piece p in completePiecesList[PartType.RightArm])
//            if (p != composition[PartType.RightArm])
//                p.gameObject.SetActive(false);

//        foreach (Piece p in completePiecesList[PartType.Body])
//            if (p != composition[PartType.Body])
//                p.gameObject.SetActive(false);

//        foreach (Piece p in completePiecesList[PartType.Legs])
//            if (p != composition[PartType.Legs])
//                p.gameObject.SetActive(false);

//        foreach (Piece p in completePiecesList[PartType.Weapon])
//            if (p != composition[PartType.Weapon])
//                p.gameObject.SetActive(false);

//    }

//    private void ModifyComposition(object sender, ChangePieceArgs args)
//    {
//        completePiecesList[args.partType][args.oldPieceNumber].gameObject.SetActive(false);
//        completePiecesList[args.partType][args.newPieceNumber].gameObject.SetActive(true);
//        Piece selectedPiece = completePiecesList[args.partType][args.newPieceNumber];
//        composition[args.partType] = selectedPiece;
//    }

//    private void SetPieceComposition(object sender, SetPieceArgs args)
//    {
//        completePiecesList[args.partType][args.pieceNumber].gameObject.SetActive(true);
//        Piece selectedPiece = completePiecesList[args.partType][args.pieceNumber];
//        composition[args.partType] = selectedPiece;
//    }

//    void CheckForNPC()
//    {
//        Ray ray = new Ray();
//        RaycastHit[] raycastHit;
//        raycastHit = Physics.SphereCastAll(transform.position, 2f, Vector3.forward, maxDistanceNPC, npcLayer);
//        if (raycastHit.Length > 0 && raycastHit[0].collider
//            && raycastHit[0].transform.TryGetComponent(out NPC npc)
//            && Input.GetKeyDown(KeyCode.E)
//            && !choicePieceManager.isUIOpen)
//        {
//            OnChoicePieces?.Invoke(this, EventArgs.Empty);
//            GetComponent<PlayerMovement>().enabled = false;
//            Cursor.lockState = CursorLockMode.None;
//            Cursor.visible = true;
//            healthBar.SetActive(false);
//            abilitiesSection.SetActive(false);
//        }
//        else if (choicePieceManager.isUIOpen && Input.GetKeyDown(KeyCode.E))
//        {
//            OnEndChoicePieces?.Invoke(this, EventArgs.Empty);
//            GetComponent<PlayerMovement>().enabled = true;
//            Cursor.lockState = CursorLockMode.Locked;
//            Cursor.visible = false;
//            healthBar.SetActive(true);
//            abilitiesSection.SetActive(true);
//        }
//    }

//    public void SetFightingState(string state)
//    {
//        if (state.ToLower().Contains("true"))
//        {
//            Debug.Log("sta combattendo");
//            isFighting = true;
//        }
//        else if (state.ToLower().Contains("false"))
//        {
//            Debug.Log("NON sta combattendo");
//            isFighting = false;
//        }
//    }

//}

//public class ScenarioArgs : EventArgs
//{
//    public ScenarioArgs(Scenario a)
//    {
//        scenario = a;
//    }
//    public Scenario scenario;
//}