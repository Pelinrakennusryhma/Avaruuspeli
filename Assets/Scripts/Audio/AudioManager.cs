using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

enum MusicArea
{
    WORLD_MAP = 0,
    SPACESHIP_SCENE = 1,
    PLANET = 2
}

public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances = new List<EventInstance>();
    private EventInstance musicEventInstance;
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null)
        {
            Instance.CleanUp();
            Debug.LogWarning("More than one AudioManager around!");
            //SetMusicArea(GetMusicArea());
        }
        else
        {
            Instance = this;
        }

    }

    private void Start()
    {
        InitializeMusic(FMODEvents.Instance.SpaceshipSceneMusic);

        //SetMusicArea(GetMusicArea());
    }

    private MusicArea GetMusicArea()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        MusicArea musicArea = sceneIndex <= 1 ? (MusicArea)sceneIndex : MusicArea.PLANET;
        Debug.Log("MUSIC AREA: " + musicArea + "sceneIndex: " + sceneIndex + "sceneName: " + SceneManager.GetActiveScene().name);
        return musicArea;
    }

    private void InitializeMusic(EventReference musicEventReference)
    {
        musicEventInstance = CreateEventInstance(musicEventReference, false);
        musicEventInstance.start();
    }
    
    public void SetMusicAreaBySceneIndex(int index)
    {
        MusicArea musicArea = index <= 1 ? (MusicArea)index : MusicArea.PLANET;
        SetMusicArea(musicArea);
    }

    void SetMusicArea(MusicArea area)
    {
        Debug.Log("SETTING MUSIC: " + area.ToString());
        musicEventInstance.setParameterByName("MusicArea", (float)area);
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference, bool addToList=true)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        if (addToList)
        {
            eventInstances.Add(eventInstance);
        }
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
