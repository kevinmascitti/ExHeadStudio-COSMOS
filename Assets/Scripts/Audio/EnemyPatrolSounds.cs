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
    }

    private FMOD.Studio.EventInstance goblinStep;

    public void PlayGoblinSteps()
    {
        goblinStep = FMODUnity.RuntimeManager.CreateInstance("event:/EnemyFootseps");
        goblinStep.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        goblinStep.start();
        goblinStep.release();
    }

    private FMOD.Studio.EventInstance goblinIdle;

    public void PlayGoblinIdle()
    {
        goblinIdle = FMODUnity.RuntimeManager.CreateInstance("event:/PatrolIdle");
        goblinIdle.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        goblinIdle.start();
        goblinIdle.release();
    }

    private FMOD.Studio.EventInstance goblinHurt;

    public void PlayGoblinHurt()
    {
        goblinHurt = FMODUnity.RuntimeManager.CreateInstance("event:/GoblinHurt");
        goblinHurt.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        goblinHurt.start();
        goblinHurt.release();
    }

    private FMOD.Studio.EventInstance despawn;

    public void PlayDespawn()
    {
        despawn = FMODUnity.RuntimeManager.CreateInstance("event:/DeSpawn");
        despawn.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        despawn.start();
        despawn.release();
    }
}