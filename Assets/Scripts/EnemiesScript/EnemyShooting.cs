using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;

    
    private void Awake()
    {
     
        AttackState.EnemyShootEvent += Shoot;
    }
    
    public void Shoot(object s, EventArgs args)
    {
        
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

    }
}
