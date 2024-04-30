using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    public void Start()
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
    }
    
    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Damage " + stats.atk);
            if (isPlayer)
            {
                if(currentHP-(stats.atk-stats.def)>=0)
                    gameObject.GetComponent<PlayerCharacter>().UpdateHP(currentHP-(stats.atk-stats.def));
                else
                    gameObject.GetComponent<PlayerCharacter>().UpdateHP(0);
            }
        }

        if (currentHP <= 0)
        {
            Die();
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

    public void TakeDamage(int damage)
    {
        UpdateHP(currentHP-damage);
        if (currentHP <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        // animazione personaggio che muore???
        // VFX nuvoletta di respawn e transizione
    }
}