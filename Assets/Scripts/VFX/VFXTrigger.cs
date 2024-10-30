using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXTrigger : MonoBehaviour
{


    [SerializeField] private List<GameObject> vfxList;
    [SerializeField] private GameObject pivotGroundHit;
    
    public void TriggerVFXTrailLightAttack1(){
        vfxList[0].GetComponent<ParticleSystem>().Play();
    }
    public void TriggerVFXTrailLightAttack2(){
        vfxList[1].GetComponent<ParticleSystem>().Play();
    }

     public void HeavyAttackGroundImpact(){
        GameObject groundHitParticles = Instantiate(vfxList[2]) as GameObject;
        groundHitParticles.transform.parent = pivotGroundHit.transform;
        groundHitParticles.transform.position = pivotGroundHit.transform.position;
        groundHitParticles.SetActive(true);
        groundHitParticles.GetComponent<ParticleSystem>().Play();
        groundHitParticles.transform.parent = null;
        StartCoroutine(DestroyParticle(groundHitParticles));
    }

    IEnumerator DestroyParticle( GameObject groundHitParticles)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(groundHitParticles);
    }


}
