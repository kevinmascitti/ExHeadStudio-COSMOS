using System;
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
        highSwing = FMODUnity.RuntimeManager.CreateInstance("event:/HeavySwing");
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


    private FMOD.Studio.EventInstance axeSwing2;

    private void PlaySwing2()
    {
        axeSwing2 = FMODUnity.RuntimeManager.CreateInstance("event:/AxeSwing 2");
        axeSwing2.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        axeSwing2.start();
        axeSwing2.release();
    }

    private FMOD.Studio.EventInstance fireBall;

    private void PlayFireBall()
    {
        fireBall = FMODUnity.RuntimeManager.CreateInstance("event:/Fireball");
        fireBall.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        fireBall.start();
        fireBall.release();
    }

    private FMOD.Studio.EventInstance gate;

    private void PlayGatePuzzle()
    {
        gate = FMODUnity.RuntimeManager.CreateInstance("event:/GatePuzzle");
        gate.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        gate.start();
        gate.release();
    }

    private FMOD.Studio.EventInstance cure;

    private void PlayCure()
    {
        cure = FMODUnity.RuntimeManager.CreateInstance("event:/Heal");
        cure.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        cure.start();
        cure.release();
    }

    private FMOD.Studio.EventInstance waterJet;

    private void PlayWaterJet()
    {
        waterJet = FMODUnity.RuntimeManager.CreateInstance("event:/Water Jet");
        waterJet.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        waterJet.start();
        waterJet.release();
    }

    private FMOD.Studio.EventInstance pickUp;

    private void PlayPickUp()
    {
        pickUp = FMODUnity.RuntimeManager.CreateInstance("event:/Pick Up");
        pickUp.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        pickUp.start();
        pickUp.release();
    }

    private FMOD.Studio.EventInstance trumpet;

    public void PlayTrumpetPuzzle()
    {
        trumpet = FMODUnity.RuntimeManager.CreateInstance("event:/Trumpet");
        trumpet.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        trumpet.start();
        trumpet.release();
    }
}
