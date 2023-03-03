using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UITargetProjections : MonoBehaviour
{
    [SerializeField]
    ActorManager actorManager;
    [SerializeField]
    GameObject targetProjectionPrefab;
    [SerializeField]
    Transform playerShip;
    [SerializeField] 
    Faction playerFaction;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        GameEvents.Instance.EventSpaceshipSpawned.AddListener(OnEventSpaceshipSpawned);
    }

    void OnEventSpaceshipSpawned(ActorSpaceship ship)
    {
        if(playerFaction.hostileFactions.Contains(ship.faction))
        {
            AddSprite(ship);
        }
    }


    void AddSprite(ActorSpaceship ship)
    {
        GameObject sprite = Instantiate(
            targetProjectionPrefab,
            Vector2.zero,
            Quaternion.identity,
            transform);

        sprite.GetComponent<TargetProjectionIcon>().Init(ship, canvas, playerShip.transform);
    }
}
