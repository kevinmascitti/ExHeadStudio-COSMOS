using System.Collections;
using System.Collections.Generic;
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

    //Listone delle cose da controllare
    private Dictionary<Element, int> statusCharge = new Dictionary<Element, int>();
    private Dictionary<Element, ElementTimer> statusTimer = new Dictionary<Element, ElementTimer>();
    private Dictionary<Element, int> effectCountdown = new Dictionary<Element, int>();
    private Dictionary<Element, ElementTimer> effectTimer = new Dictionary<Element, ElementTimer>();
    private Dictionary<Element, bool> effectsApplied = new Dictionary<Element, bool>();
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

        ElementTimer.Elapsed += HandleTimer;
    }

    // Update is called once per frame
    public void Update()
    {
        //Se il timer di uno qualunque tra effetti o status si azzera, allora fai quello che devi fare
        foreach (Element element in elementsOfStatusApplied)
        {
            // if (statusTimer[element] <= 0f)
            // {
            //     statusCharge[element] = 0;
            //     elementsOfStatusApplied.Remove(element);
            // }
        }
        foreach(Element element in elementsOfEffectsApplied)
        {
            // if (effectTimer[element] <= 0f)
            // {
            //     Effect(element);
            // }
        }


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Damage " + stats.atk);
            if (isPlayer)
            {
                if (currentHP - (stats.atk - stats.def) >= 0)
                    gameObject.GetComponent<PlayerCharacter>().UpdateHP(currentHP - (stats.atk - stats.def));
                else
                    gameObject.GetComponent<PlayerCharacter>().UpdateHP(0);
            }
        }

        //Se la lista degli elementi applicati all'oggetto non � vuota, diminuisci il timer dello status o dell'effetto di quell'elemento
        if(elementsOfStatusApplied.Count > 0) {
            // foreach (Element element in elementsOfStatusApplied)
            // {
            //     if (statusTimer[element] > 0f)
            //         statusTimer[element] -= Time.deltaTime;
            // }
        }
        else isStatusApplied = false;
        if (elementsOfEffectsApplied.Count > 0)
        {
        //     foreach (Element element in elementsOfEffectsApplied)
        //     {
        //         if (effectTimer[element] > 0f)
        //             effectTimer[element] -= Time.deltaTime;
        //     }
        }
        else isEffectApplied = false;
        
    }

    private void HandleTimer(object sender, ElementTimerArgs args)
    {
        if (args.type == TimerType.Status)
        {
            statusTimer[args.element].Stop();
            elementsOfStatusApplied.Remove(args.element);
        }
        else if (args.type == TimerType.Effect)
        {
            Effect(args.element);
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
        Debug.Log("Danno inflitto: " + damage);

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
        Debug.Log("Applicazione Stato:" + element);

        if (!effectsApplied[element])
        {
            statusCharge[element] += elementalDamage;
            if(!elementsOfStatusApplied.Contains(element))
                elementsOfStatusApplied.Add(element);
            isStatusApplied = true;
            // statusTimer[element] = 5f;
        }
        if (statusCharge[element] >= 100 )
        {
            ApplyElementEffect(element);
        }
        
    }
    //Applicato abbastanza status, applica l'effetto e applicalo una volta ogni 5 secondi per 5 volte.
    public void ApplyElementEffect(Element element)
    {
        isEffectApplied = true;
        effectCountdown[element] = 5;
        // effectTimer[element] = 5f;

    }

    //Applica il danno o il malus di quello specifico elemento
    public void Effect(Element e)
    {


        effectCountdown[e] -= 1;
        // effectTimer[e] = 5f;
    }
    public virtual void Die()
    {
        // animazione personaggio che muore???
        // VFX nuvoletta di respawn e transizione
    }
}