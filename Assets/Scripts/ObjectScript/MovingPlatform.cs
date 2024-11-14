using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 2f;
    [SerializeField] private Transform[] wayPoints;

    private int wayPointIndex = 0;

    private void Start()
    {
        transform.position = wayPoints[wayPointIndex].position;
    }
    void FixedUpdate() //Se questa cosa viene messa in Update la piattaforma smette di funzionare
    {
        if(Vector3.Distance(transform.position, wayPoints[wayPointIndex].position) < 0.2f)
        {
            wayPointIndex++;
            if(wayPointIndex ==  wayPoints.Length)
            {
                wayPointIndex = 0;
            }
        }
        transform.position = Vector3.MoveTowards(transform.position, wayPoints[wayPointIndex].position, movementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {

            coll.gameObject.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit(Collider coll)
    {
        if (coll.CompareTag("Player"))
        {
            coll.gameObject.transform.SetParent(null);
        }
    }
}
