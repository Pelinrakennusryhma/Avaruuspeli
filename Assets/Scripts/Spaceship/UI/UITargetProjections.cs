using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UITargetProjections : MonoBehaviour
{
    [SerializeField]
    ActorManager actorManager;
    [SerializeField]
    GameObject targetObjectPrefab;
    [SerializeField]
    GameObject playerShip;
    [SerializeField] 
    Faction playerFaction;

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        GameEvents.Instance.EventSpaceshipSpawned.AddListener(OnEventSpaceshipSpawned);
    }

    void OnEventSpaceshipSpawned(ActorSpaceship actor)
    {
        if(playerFaction.hostileFactions.Contains(actor.faction))
        {
            AddIndicator(actor);
        } else if(actor.faction.factionName == "Player")
        {
            playerShip = actor.ship;
        }
    }


    void AddIndicator(ActorSpaceship actor)
    {
        GameObject targetObject = Instantiate(targetObjectPrefab, actor.ship.transform);
        TargetProjectionIcon tpIcon = targetObject.GetComponent<TargetProjectionIcon>();
        tpIcon.Init(playerShip);
    }

    //void AddSprite(ActorSpaceship ship)
    //{
    //    GameObject sprite = Instantiate(
    //        targetProjectionPrefab,
    //        Vector2.zero,
    //        Quaternion.identity,
    //        transform);

    //    sprite.GetComponent<TargetProjectionIcon>().Init(ship, canvas, playerShip.transform);
    //}
}
