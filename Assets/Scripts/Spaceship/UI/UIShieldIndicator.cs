using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class UIShieldIndicator : MonoBehaviour
{
    [SerializeField]
    PlayerControls player;
    [SerializeField]
    TMP_Text shieldedText;
    void Awake()
    {
        GameEvents.Instance.EventSpaceshipSpawned.AddListener(OnSpaceshipSpawned);

        if (shieldedText == null)
        {
            shieldedText = GetComponent<TMP_Text>();
        }
    }

    void OnSpaceshipSpawned(ActorSpaceship actor)
    {
        if (actor is PlayerControls)
        {
            player = (PlayerControls)actor;
        }
    }

    private void Update()
    {
        if (player != null)
        {
            if (Utils.ListContains<SpaceshipShield>(player.ActiveUtils))
            {
                shieldedText.enabled = true;
            }
            else
            {
                shieldedText.enabled = false;
            }
        }
    }
}
