using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances = new List<EventInstance>();
    public static AudioManager Instance { get; private set; }

    void Awake()
    {
        if(Instance != null)
        {
            Instance.CleanUp();
            Debug.LogWarning("More than one AudioManager around!");
        }
        Instance = this;
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        eventInstances.Add(eventInstance);
        Debug.Log("Add audio");
        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        //eventInstances.Clear();
        Debug.Log("CleanUp Audio");
    }

    private void OnDestroy()
    {
        //CleanUp();
    }
}
