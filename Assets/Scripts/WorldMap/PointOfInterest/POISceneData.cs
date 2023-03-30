using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public abstract class POISceneData : ScriptableObject
{
    [field: SerializeField]
    public string Title { get; private set; }
    [SerializeField]
    [TextArea(5, 1)]
    protected string descriptionTemplate;
    [field: SerializeField]
    public string SceneToLoad { get; private set; }

    public abstract string GetDescription();
}

