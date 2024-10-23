using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class AIArea: MonoBehaviour
{
    public int areaID;
    public Dictionary<int, GameObject> enemyList = new Dictionary<int, GameObject>();
    public int count;
    
    public bool isPlayerInside=false;
    public static EventHandler<OnPlayerArg> OnPlayerExit;
    public static EventHandler<OnPlayerArg> OnPlayerEnter;
   
    public BoxCollider areaCollider;
    public void Awake()
    {
        areaCollider = GetComponent<BoxCollider>();
        
    }
    
    virtual public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Enemy") || other.gameObject.tag.Equals("ShootingEnemy") && !enemyList.ContainsKey(other.gameObject.GetInstanceID()))
        {
            enemyList.Add(other.gameObject.GetInstanceID(), other.gameObject);
            other.gameObject.GetComponent<StateController>().areaID = areaID;
            other.gameObject.GetComponent<StateController>().SetAreaBounds(areaCollider);
            count=enemyList.Count;
            if(isPlayerInside)
            {
                other.gameObject.GetComponent<StateController>().canChase = true;
            }
        }
        //All'inizio del gioco, salvo in ogni area i nemici all'interno e in caso il player
        else if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerInside = true;
            OnPlayerEnter?.Invoke(this, new OnPlayerArg(areaID));
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        //Se il player esce dalla zona, i nemici smettono di inseguirlo
        if (other.gameObject.tag.Equals("Player"))
        {
            isPlayerInside = false;
            OnPlayerExit?.Invoke(this, new OnPlayerArg(areaID));
        }
        
    }
    
   
}
public class OnPlayerArg : EventArgs
{
    public OnPlayerArg(int i)
    {
        areaId = i;
    }

    public int areaId;
}
