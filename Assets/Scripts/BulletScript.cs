using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : Weapon
{


    [SerializeField] float bulletDestroyTime;
    [SerializeField] float damageRadius;
    [SerializeField] int maxDamage;
    [SerializeField] int minDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] Element bulletElement;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask blockMask;
    [SerializeField] ParticleSystem collisionParticle;
    //[SerializeField] float explosionForce;

    Rigidbody rb;

    private int maxEnemies = 25;
    private Collider[] enemiesArray;
    private int damage;
    void Awake()
    {
        enemiesArray = new Collider[maxEnemies];
        rb = GetComponent<Rigidbody>();
        ParticleSystem trailParticle = GetComponent<ParticleSystem>();
        trailParticle.Play();
        Destroy(gameObject, bulletDestroyTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(collisionParticle, gameObject.transform.position, Quaternion.identity);
        collisionParticle.Play();
        
        if(collision.gameObject.TryGetComponent<FireInteractive>(out FireInteractive fireInteract))
        {
            fireInteract.InteractionsType(fireInteract.typeOfObjectInteraction);
            //FireInteractions interactionsConst = fireInteract.GetComponent<FireInteractions>();
            //fireInteract.InteractionsType(FireInteractions.Destructible);
        }


        //qui inserire suoni, effetti
        int hits = Physics.OverlapSphereNonAlloc(gameObject.transform.position, damageRadius, enemiesArray, enemyMask);

        for(int i = 0; i < hits; i++) 
        {
            float distance = Vector3.Distance(gameObject.transform.position, enemiesArray[i].transform.position);
            Debug.Log(distance);
            //Debug.Log(enemiesArray[i].name + (enemiesArray[i].transform.position - gameObject.transform.position).normalized);
            //Debug.DrawRay(gameObject.transform.position, (enemiesArray[i].transform.position - gameObject.transform.position).normalized, Color.green, 10f);
            if (!Physics.Raycast(gameObject.transform.position, (enemiesArray[i].transform.position - gameObject.transform.position).normalized, damageRadius, blockMask.value)
                || distance <=1)
            {
                damage = Mathf.FloorToInt(Mathf.Lerp(maxDamage, minDamage, distance / damageRadius));
                Debug.Log($"Ho trovato il nemico {enemiesArray[i].name} a " + distance + "gli infliggo " + damage);
                Enemy target = enemiesArray[i].GetComponentInParent<Enemy>();
                target.TakeDamage(damage, bulletElement,enemyMask);
                
            }
        }
        //Debug.Log("Colpito");
        gameObject.SetActive(false);
        Destroy(gameObject, 2);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * bulletSpeed;
    }

    public void OnDrawGizmos() //la funzione serve a visualizzare l'area del danno
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(gameObject.transform.position, damageRadius);
    }


}
