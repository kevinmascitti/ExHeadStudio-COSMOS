using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : Weapon
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
        rb.velocity = transform.forward * bulletSpeed;
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

        gameObject.SetActive(false);
        //Destroy(gameObject, 2);
    }
    //void Update()
    //{
    //    rb.velocity = transform.forward * bulletSpeed;
    //}
}


////nella classe del proiettile
//[SerializeField] private float travelSpeed;
//[SerializeField] private float destroyDelay;
//[SerializeField] private bool useGravity, updateTravel, useVelocity;

//private RigidBody rig;
//void Start()
//{
//    Destroy(this, destroyDelay);
//    rig.useGravity = useGravity;
//    if (!updateTravel) rig.velocity – transform.forward* travelSpeed;
//}

//void Update()
//{
//    If(updateTravel)

//    {
//        If(useVelocity)

//        {
//            rig.velocity = transform.forward * travelspeed;
//        }
//        Else

//        {
//            Transform.position += transform.forward * travelspeed * time.DeltaTime;
//        }
//    }
//}
//Public bool IsUpdatingTravel()
//{
//    Return isUpdatingTravel;
//}

