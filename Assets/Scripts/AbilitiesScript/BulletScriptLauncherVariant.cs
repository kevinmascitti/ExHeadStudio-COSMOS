using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScriptLauncherVariant : Weapon
{


    [SerializeField] private float bulletDestroyTime;
    [SerializeField] float damageRadius;
    [SerializeField] int maxDamage;
    [SerializeField] int minDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] Element bulletElement;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask blockMask;
    [SerializeField] ParticleSystem bulletSmokeEffect;
    [SerializeField] ParticleSystem collisionParticle;

    Rigidbody rb;

    private int maxEnemies = 25;
    private Collider[] enemiesArray;
    private int damage;
    void Awake()
    {
        bulletSmokeEffect.Play();
        enemiesArray = new Collider[maxEnemies];
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletDestroyTime);
        rb.velocity = transform.forward * bulletSpeed; //UNICO CAMBIAMENTO
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionParticle.Play();
        bulletSmokeEffect.Stop();

        if (collision.gameObject.TryGetComponent<FireInteractive>(out FireInteractive fireInteract))
        {
            fireInteract.InteractionsType(fireInteract.typeOfObjectInteraction);
        }

        int hits = Physics.OverlapSphereNonAlloc(gameObject.transform.position, damageRadius, enemiesArray, enemyMask);

        for (int i = 0; i < hits; i++)
        {
            float distance = Vector3.Distance(gameObject.transform.position, enemiesArray[i].transform.position);
            if (!Physics.Raycast(gameObject.transform.position, (enemiesArray[i].transform.position - gameObject.transform.position).normalized, damageRadius, blockMask.value)
                || distance <= 1)
            {
                damage = Mathf.FloorToInt(Mathf.Lerp(maxDamage, minDamage, distance / damageRadius));
                Enemy target = enemiesArray[i].GetComponentInParent<Enemy>();
                target.TakeDamage(damage, bulletElement);

            }
        }

        collisionParticle.Stop();
        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }
}
