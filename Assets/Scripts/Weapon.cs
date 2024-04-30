using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int weaponAtk;
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.GetMask("Enemy"))
        {
            Debug.Log("Enemy damage!");
            collision.collider.gameObject.GetComponent<Enemy>().TakeDamage(gameObject.GetComponent<PlayerCharacter>().stats.atk + weaponAtk - collision.collider.gameObject.GetComponent<Enemy>().def);
        }
    }
}
