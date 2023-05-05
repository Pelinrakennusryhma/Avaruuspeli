using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISafetyIndicator : MonoBehaviour
{
    [SerializeField]
    PlayerControls player;
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
            player = (PlayerControls)actor;
        }
    }

    private void Update()
    {
        if(player != null)
        {
            if(Utils.ListContains<SpaceshipECM>(player.ActiveUtils))
            {
                safetyImage.enabled = true;
            } else
            {
                safetyImage.enabled = false;
            }
        }
    }
}
