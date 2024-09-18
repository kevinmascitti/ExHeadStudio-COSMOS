using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    private FMOD.Studio.EventInstance foosteps;

    private void PlayFootstep()
    {
        foosteps = FMODUnity.RuntimeManager.CreateInstance("event:/FootSteps");
        foosteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        foosteps.start();
        foosteps.release();
    }

    private FMOD.Studio.EventInstance jump;

    private void PlayJump()
    {
        jump = FMODUnity.RuntimeManager.CreateInstance("event:/Jump");
        jump.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        jump.start();
        jump.release();
    }

    private FMOD.Studio.EventInstance axeSwing;

    private void PlayAxeSwing()
    {
        axeSwing = FMODUnity.RuntimeManager.CreateInstance("event:/AxeSwing");
        axeSwing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        axeSwing.start();
        axeSwing.release();
    }
}
