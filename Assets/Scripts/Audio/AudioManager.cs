using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance { get; private set; }

    private EventInstance ambianceEventInstance;
    [field: Header("Dungeon Ambience")]
    [field: SerializeField] public EventReference dungeonAmbience { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
    }
    public static void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    private void Start()
    {
        InitializeAmbience(dungeonAmbience);
    }
    private void InitializeAmbience(EventReference ambienceEventReference)
    {
        ambianceEventInstance = RuntimeManager.CreateInstance(ambienceEventReference);
        ambianceEventInstance.start();
    }
    

}
