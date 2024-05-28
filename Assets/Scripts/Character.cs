using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Windows.Speech;
using static UnityEditor.Rendering.FilterWindow;
using FixedUpdate = UnityEngine.PlayerLoop.FixedUpdate;

public enum Element
{
    Normal,
    Water,
    Fire,
    Lightning,
    Earth
}

public class Stats
{
    public int atk = 0;
    public int def = 0;
    public Dictionary<Element,int> elemAtk = new Dictionary<Element, int>();
    public Dictionary<Element,int> elemDef = new Dictionary<Element, int>();
}

public class Character : MonoBehaviour
{
    [Header("Tempi recupero status e effetti")]
    [SerializeField] private float status;
    [SerializeField] private float effect;
    public TextMeshProUGUI statusText;
    public TextMeshProUGUI effectText;

    [Header("Informazioni character")]
    public float currentHP;
    public bool isPlayer = false;
    public Stats stats = new Stats();
    public int atk;
    public int def;
    public int normalAtk;
    public int normalDef;
    public int waterAtk;
    public int waterDef;
    public int fireAtk;
    public int fireDef;
    public int lightningAtk;
    public int lightningDef;
    public int earthAtk;
    public int earthDef;
    public float walkSpeed;
    public float runSpeed;
    public float atkSpeed;

    [Header("Dizionari")]
    [SerializeField] private Dictionary<Element, int> statusCharge = new Dictionary<Element, int>();
    [SerializeField] private Dictionary<Element, ElementTimer> statusTimer = new Dictionary<Element, ElementTimer>();
    [SerializeField] private Dictionary<Element, int> effectCountdown = new Dictionary<Element, int>();
    [SerializeField] private Dictionary<Element, ElementTimer> effectTimer = new Dictionary<Element, ElementTimer>();
    [SerializeField] private Dictionary<Element, bool> effectsApplied = new Dictionary<Element, bool>();
    private List<Element> elementsOfStatusApplied = new List<Element>();
    private List<Element> elementsOfEffectsApplied = new List<Element>();
    private bool isStatusApplied;
    private bool isEffectApplied;


    public void Awake()
    {
        stats.atk = atk;
        stats.def = def;
        stats.elemAtk.Add(Element.Normal, normalAtk);
        stats.elemDef.Add(Element.Normal, normalDef);
        stats.elemAtk.Add(Element.Water, waterAtk);
        stats.elemDef.Add(Element.Water, waterDef);
        stats.elemAtk.Add(Element.Fire, fireAtk);
        stats.elemDef.Add(Element.Fire, fireDef);
        stats.elemAtk.Add(Element.Lightning, lightningAtk);
        stats.elemDef.Add(Element.Lightning, lightningDef);
        stats.elemAtk.Add(Element.Earth, earthAtk);
        stats.elemDef.Add(Element.Earth, earthDef);
        foreach(Element e in stats.elemDef.Keys) {
            effectsApplied.Add(e, false);
            statusCharge.Add(e, 0);
            effectCountdown.Add(e, 0);
            statusTimer.Add(e, new ElementTimer(status, false, TimerType.Status, e, this.GetInstanceID()));
            effectTimer.Add(e, new ElementTimer(effect, false, TimerType.Effect, e, this.GetInstanceID()));

        }
        ElementTimer.Elapsed += HandleTimer;
    }
    
    // Update is called once per frame
    public void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Debug.Log("Damage " + stats.atk);
            if (isPlayer)
            {
                if (currentHP - (stats.atk - stats.def) >= 0)
                    gameObject.GetComponent<PlayerCharacter>().UpdateHP(currentHP - (stats.atk - stats.def));
                else
                    gameObject.GetComponent<PlayerCharacter>().UpdateHP(0);
            }
        }

        if (effectsApplied[Element.Fire])
            Debug.Log(effectTimer[Element.Fire]);
       
    }

    private void HandleTimer(object sender, ElementTimerArgs args)
    {
        if (this.GetInstanceID()==args.id)
        {
        if (args.type == TimerType.Status)
        {
            statusTimer[args.element].Stop();
                statusCharge[args.element] = 0;
                statusText.text = "0";
        }
        else if (args.type == TimerType.Effect)
        {
            if (effectCountdown[args.element] > 1)
            {
                Debug.Log("Effetto!");
            Effect(args.element);
            effectCountdown[args.element] -= 1;
            effectTimer[args.element].Begin();

                effectText.text = effectCountdown[args.element].ToString();
            }
            else if (effectCountdown[args.element] == 1)
            {
                Effect(args.element);
                effectCountdown[args.element] = 0;
                effectsApplied[args.element] = false;
                Debug.Log("Rimozione effetto: " + args.element);
                effectTimer[args.element].Stop();
                effectText.text = effectCountdown[args.element].ToString();
            }
        }
            
        }
        
    }

    public virtual void UpdateHP(float newHP)
    {
        if (newHP >= 0)
        {
            currentHP = newHP;
        }
        else
        {
            currentHP = 0;
        }
    }

    public void TakeDamage(int damage, Element element)
    {

        UpdateHP(currentHP-damage);
        TakeElementalStatus(damage, element);
        if (currentHP <= 0)
        {
            Die();
        }
    }
    

    //Applica lo status all'oggetto, salvalo nella lista e fai partire il timer
    public void TakeElementalStatus(int elementalDamage, Element element)
    {

        if (!effectsApplied[element] )
        {
        Debug.Log("Applicazione stato:" + element);
            
            statusCharge[element] += elementalDamage;
            statusText.text = statusCharge[element].ToString();
            isStatusApplied = true;
            statusTimer[element].Begin();
        }
        else;

        if (statusCharge[element] >= 10 && !effectsApplied[element])
        {
            statusCharge[element] = 0;
            ApplyElementEffect(element);
            statusTimer[element].Stop();
            statusText.text = statusCharge[element].ToString();
        }

    }
    //Applicato abbastanza status, applica l'effetto e applicalo una volta ogni 5 secondi per 5 volte.
    public void ApplyElementEffect(Element element)
    {
        Debug.Log("Effetto Applicato");
        effectsApplied[element] = true;

        effectCountdown[element] = 5;
        effectTimer[element].Begin();
        effectText.text = effectCountdown[element].ToString();
        // effectTimer[element] = 5f;

    }

    //Applica il danno o il malus di quello specifico elemento
    public void Effect(Element e)
    {
        switch (e)
        {
            case Element.Fire:

                //Applicazione dell'effetto fuoco (malus, danno....)

                break;
            case Element.Water:
                //Applicazione effetto acqua (malus, danno...)
                break;
            case Element.Lightning:

                //Applicazione effetto fulmine (malus, danno...)
                break;
            case Element.Earth:

                //Applicazione effetto terra (malus, danno...)
            break;  

            default:
                
            break;
        }
    }
    public virtual void Die()
    {
        // animazione personaggio che muore???
        // VFX nuvoletta di respawn e transizione
    }
}