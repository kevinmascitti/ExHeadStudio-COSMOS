using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class BulletLauncher : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float startTime, repeatTime;
    [SerializeField] private Transform startPosition;

    private void Start()
    {
        InvokeRepeating("InstantiateBullet", startTime, repeatTime);
    }

    private void InstantiateBullet()
    {
        Instantiate(bulletPrefab, startPosition);
    }
}
