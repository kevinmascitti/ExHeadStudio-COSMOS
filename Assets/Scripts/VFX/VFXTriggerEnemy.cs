using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTriggerEnemy : MonoBehaviour
{


    [SerializeField] public List<GameObject> vfxList;
    
    public void DeathVFX(){
        vfxList[0].GetComponent<ParticleSystem>().Play();
    }

}
