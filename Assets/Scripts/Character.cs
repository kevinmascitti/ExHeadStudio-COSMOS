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

public class LULCharacter : MonoBehaviour
{
    public float currentHP;
    public bool isPlayer = false;
    public Stats totalStats;
    public float walkSpeed;
    public float runSpeed;
    public float atkSpeed;

    // Update is called once per frame
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Damage " + totalStats.atk);
            if (isPlayer)
            {
                if(currentHP-(totalStats.atk-totalStats.def)>=0)
                    gameObject.GetComponent<PlayerCharacter>().UpdateHP(currentHP-(totalStats.atk-totalStats.def));
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
        currentHP = newHP;
    }
    
    public virtual void Die()
    {
        // animazione personaggio che muore???
        // VFX nuvoletta di respawn e transizione
    }
}