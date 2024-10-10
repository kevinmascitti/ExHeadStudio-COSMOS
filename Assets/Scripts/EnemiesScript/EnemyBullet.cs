using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{

    [SerializeField] private float bulletDestroyTime;
    [SerializeField] float damageRadius;
    [SerializeField] int maxDamage;
    [SerializeField] int minDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] Element bulletElement;
    [SerializeField] LayerMask playerMask;
    [SerializeField] LayerMask blockMask;
    [SerializeField] ParticleSystem bulletSmokeEffect;
    [SerializeField] ParticleSystem collisionParticle;
    Collider[] playerHit = new Collider[1];
    Rigidbody rb;

    private int damage;
    /* public EnemyBullet(int minD, int maxD, float bSpeed, float bDTime)
    {
        bulletDestroyTime = bDTime;
        minDamage=minD;
        maxDamage = maxD;
        bulletSpeed = bSpeed;
    }*/
    void Awake()
    {
        bulletSmokeEffect.Play();
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletDestroyTime);
        rb.velocity = (gameObject.transform.forward * bulletSpeed);//ho aggiunto un offset per non sparare troppo in basso
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionParticle.Play();
        bulletSmokeEffect.Stop();
         if(Physics.OverlapSphereNonAlloc(gameObject.transform.position, damageRadius, playerHit, playerMask) != 0)
        {
            float distance = Vector3.Distance(gameObject.transform.position, playerHit[0].transform.position);
            if (!Physics.Raycast(gameObject.transform.position, (playerHit[0].transform.position - gameObject.transform.position).normalized, damageRadius, blockMask.value)
                || distance <= 1)
            {
                damage = Mathf.FloorToInt(Mathf.Lerp(maxDamage, minDamage, distance / damageRadius));
                var target = playerHit[0].GetComponent<PlayerCharacter>();
                target.TakeDamage(damage, bulletElement);

            }
        }

        collisionParticle.Stop();
        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }

    public void SetVelocity(Vector3 newVelocity)
    {
        this.rb.velocity = newVelocity;
    }
    //void Update()
    //{
    //    rb.velocity = transform.forward * bulletSpeed;
    //}
}


// Update is called once per frame

