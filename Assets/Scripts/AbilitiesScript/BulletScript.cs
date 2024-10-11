using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : Weapon
{

    //OCCHIO A RINOMINARE LE COSE CHE POI NON FUNZIONA PIù
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
    private LockOnCamSwitcher lockOnScript;

    private int maxEnemies = 25;
    private Collider[] enemiesArray;
    private int damage;
    void Awake()
    {
        lockOnScript = GameObject.Find("Player").GetComponent<LockOnCamSwitcher>();
        bulletSmokeEffect.Play();
        enemiesArray = new Collider[maxEnemies];
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletDestroyTime);

        //rb.velocity = (Camera.main.transform.forward.normalized * bulletSpeed + new Vector3(0f, 1f, 0f));//ho aggiunto un offset per non sparare troppo in basso
    }

    private void OnEnable()
    {
        if (lockOnScript.lockOn)
        {
            //aggiungo un offset perchè altrimenti spara in basso
            rb.velocity = ((lockOnScript.GetCurrentEnemyTr().position - GameObject.Find("Player/CHARACTER - L-ARM - FIREMAN/BulletStart Position").transform.position).normalized + new Vector3(0f, 0.1f, 0f)) * bulletSpeed;
            Debug.Log("Con lock");
        }
        else
        {
            rb.velocity = transform.forward * bulletSpeed;
            Debug.Log("Senza lock");
        }
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

    public void SetVelocity(Vector3 newVelocity)
    {
        this.rb.velocity = newVelocity;
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

