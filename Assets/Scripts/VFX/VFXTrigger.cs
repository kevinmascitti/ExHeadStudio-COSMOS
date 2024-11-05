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

    public void TriggerVFXTrailLightAttack3(){
        vfxList[2].GetComponent<ParticleSystem>().Play();
    }

    public void TriggerVFXTrailHeavyAttack(){
        vfxList[3].GetComponent<ParticleSystem>().Play();
    }

    public void HeavyAttackGroundImpact(){
        GameObject groundHitParticles = Instantiate(vfxList[4]) as GameObject;
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

    public void FireRingFireBall(){
          vfxList[5].GetComponent<ParticleSystem>().Play();
          vfxList[6].GetComponent<ParticleSystem>().Play();
          vfxList[7].GetComponent<ParticleSystem>().Play();
        vfxList[8].GetComponent<ParticleSystem>().Play();
    }

}
