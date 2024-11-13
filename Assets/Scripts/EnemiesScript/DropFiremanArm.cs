using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFiremanArm : MonoBehaviour
{

    [SerializeField] GameObject FireArm;

     private void SpawnFireArm()
    {  
        StartCoroutine(Spawn());
        
    }

    IEnumerator Spawn(){
        yield return new WaitForSeconds(2f);
          Instantiate(FireArm, transform.position + Vector3.up, transform.rotation);
    }
}
