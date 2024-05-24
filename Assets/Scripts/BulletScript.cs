using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{


    [SerializeField] float bulletDestroyTime;
    [SerializeField] float damageRadius;
    [SerializeField] int damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] LayerMask enemyMask;

    Rigidbody rb;

    private Collider[] enemies;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletDestroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //qui inserire suoni, effetti
        Collider[] enemies =  Physics.OverlapSphere(gameObject.transform.position, damageRadius, enemyMask); //volendo si può usare la versione NonAlloc
        foreach(Collider target in enemies)
        {
            //Debug.Log("Ho trovato un nemico");
            //target.GetComponent<Enemy>().TakeDamage(damage);
            target.SendMessage("TakeDamage", damage);

        }
        
        Debug.Log("Colpito");
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = transform.forward * bulletSpeed;

    }
}
