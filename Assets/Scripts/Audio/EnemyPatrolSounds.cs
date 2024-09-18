using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyPatrolSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance goblinAttack;

    public void PlayGoblinAttack()
    {
        goblinAttack = FMODUnity.RuntimeManager.CreateInstance("event:/GoblinClaw");
        goblinAttack.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        goblinAttack.start();
        goblinAttack.release();

        Debug.Log("Lmao");
    }
}
