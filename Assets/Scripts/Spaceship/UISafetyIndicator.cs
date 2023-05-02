using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UISafetyIndicator : MonoBehaviour
{
    [SerializeField]
    SpaceshipECM ecmScript;
    [SerializeField]
    Image safetyImage;
    void Awake()
    {
        GameEvents.Instance.EventSpaceshipSpawned.AddListener(OnSpaceshipSpawned);

        if(safetyImage == null)
        {
            safetyImage = GetComponent<Image>();
        }
    }

    void OnSpaceshipSpawned(ActorSpaceship actor)
    {
        if(actor is PlayerControls)
        {
            SpaceshipECM ecm = actor.ship.GetComponent<SpaceshipECM>();
            if(ecm != null)
            {
                ecmScript = ecm;
            }
        }
    }

    private void Update()
    {
        if(ecmScript != null)
        {
            if (ecmScript.Active)
            {
                safetyImage.enabled = true;
            } else
            {
                safetyImage.enabled = false;
            }
        }
    }
}
