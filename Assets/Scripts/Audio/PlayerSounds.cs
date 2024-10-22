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

    private FMOD.Studio.EventInstance highSwing;

    private void PlayHighSwing()
    {
        highSwing = FMODUnity.RuntimeManager.CreateInstance("event:/HighHeavySwing");
        highSwing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        highSwing.start();
        highSwing.release();
    }

    private FMOD.Studio.EventInstance lowSwing;

    private void PlayLowSwing()
    { 
        lowSwing = FMODUnity.RuntimeManager.CreateInstance("event:/LowHeavySwing");
        lowSwing.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        lowSwing.start();
        lowSwing.release();
    }

 }
