using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyShooting : MonoBehaviour
{
    [SerializeField] Transform bulletSpawnPoint;
    [SerializeField] GameObject bulletPrefab;

    public void Shoot()
    {
        
        Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

    }
}
