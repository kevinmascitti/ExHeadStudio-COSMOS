using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponAtk;
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Debug.Log("Enemy damage!");
            other.gameObject.GetComponent<Enemy>().TakeDamage(gameObject.GetComponentInParent<PlayerCharacter>().stats.atk + weaponAtk - other.gameObject.GetComponent<Enemy>().def);
        }
    }
}
