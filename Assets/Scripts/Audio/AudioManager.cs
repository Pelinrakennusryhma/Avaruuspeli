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

public enum FMODBus 
{
    MASTER,
    MUSIC,
    SFX,
    AMBIENCE
}

[System.Serializable]
public class SavedVolumeData
{
    public float master;
    public float music;
    public float sfx;
    public float ambience;
}

public class AudioManager : MonoBehaviour
{
    [field: Header("Volume")]
    [field: Range(0, 1)]
    [field: SerializeField] public float MasterVolume { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float MusicVolume { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float SFXVolume { get; private set; }
    [field: Range(0, 1)]
    [field: SerializeField] public float AmbienceVolume { get; private set; }

    private Bus masterBus;
    private Bus musicBus;
    private Bus SFXBus;
    private Bus ambienceBus;

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

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        SFXBus = RuntimeManager.GetBus("bus:/SFX");
        ambienceBus = RuntimeManager.GetBus("bus:/Ambience");

        LoadData();
        SetInitialVolumes();
    }

    private void Start()
    {
        InitializeMusic(FMODEvents.Instance.Music);
    }

    void LoadData()
    {
        SavedVolumeData data = JsonUtility.FromJson<SavedVolumeData>(PlayerPrefs.GetString("VolumeData"));
        if (data != null)
        {
            MasterVolume = data.master;
            MusicVolume = data.music;
            SFXVolume = data.sfx;
            AmbienceVolume = data.ambience;
        } else
        {
            MasterVolume = 1f;
            MusicVolume = 1f;
            SFXVolume = 1f;
            AmbienceVolume = 1f;
        }
    }

    public void SaveData()
    {
        SavedVolumeData newVolumeData = new SavedVolumeData()
        {
        master = MasterVolume,
        music = MusicVolume,
        sfx = SFXVolume,
        ambience = AmbienceVolume,
        };
        PlayerPrefs.SetString("VolumeData", JsonUtility.ToJson(newVolumeData));
    }

    void SetInitialVolumes()
    {
        masterBus.setVolume(MasterVolume);
        musicBus.setVolume(MusicVolume);
        SFXBus.setVolume(SFXVolume);
        ambienceBus.setVolume(SFXVolume);
    }

    public float GetBusValue(FMODBus bus)
    {
        switch (bus)
        {
            case FMODBus.MASTER:
                return MasterVolume;
            case FMODBus.MUSIC:
                return MusicVolume;
            case FMODBus.SFX:
                return SFXVolume;
            case FMODBus.AMBIENCE:
                return AmbienceVolume;
            default:
                return 1f;
        }
    }

    public void SetBusValue(FMODBus bus, float value)
    {
        switch (bus)
        {
            case FMODBus.MASTER:
                masterBus.setVolume(value);
                MasterVolume = value;
                break;
            case FMODBus.MUSIC:
                musicBus.setVolume(value);
                MusicVolume = value;
                break;
            case FMODBus.SFX:
                SFXBus.setVolume(value);
                SFXVolume = value;
                break;
            case FMODBus.AMBIENCE:
                ambienceBus.setVolume(value);
                AmbienceVolume = value;
                break;
            default:
                break;
        }
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

        return eventInstance;
    }

    private void CleanUp()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }
    }

    private void OnDestroy()
    {
        //CleanUp();
    }
}
