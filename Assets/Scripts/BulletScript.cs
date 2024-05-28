using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BulletScript : MonoBehaviour
{


    [SerializeField] float bulletDestroyTime;
    [SerializeField] float damageRadius;
    [SerializeField] int maxDamage;
    [SerializeField] int minDamage;
    [SerializeField] float bulletSpeed;
    [SerializeField] LayerMask enemyMask;
    [SerializeField] LayerMask blockMask;
    //[SerializeField] float explosionForce;

    Rigidbody rb;

    private int maxEnemies = 25;
    private Collider[] enemiesArray;
    private int damage;
    void Awake()
    {
        enemiesArray = new Collider[maxEnemies];
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletDestroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        //qui inserire suoni, effetti
        int hits = Physics.OverlapSphereNonAlloc(gameObject.transform.position, damageRadius, enemiesArray, enemyMask);

        for(int i = 0; i < hits; i++) 
        {
            float distance = Vector3.Distance(gameObject.transform.position, enemiesArray[i].transform.position);
            Debug.DrawRay(gameObject.transform.position, (enemiesArray[i].transform.position - gameObject.transform.position).normalized, Color.green, damageRadius);
            if (!Physics.Raycast(gameObject.transform.position, (enemiesArray[i].transform.position - gameObject.transform.position).normalized, damageRadius, blockMask.value))
            {
                damage = Mathf.FloorToInt(Mathf.Lerp(maxDamage, minDamage, distance / damageRadius));
                Debug.Log($"Ho trovato il nemico {enemiesArray[i].name} a " + distance + "gli infliggo " + damage);
                //target.GetComponent<Enemy>().TakeDamage(damage, Element.Fire);
            }
        }
        Debug.Log("Colpito");
        Destroy(gameObject);
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
