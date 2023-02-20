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

    Canvas canvas;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        GameEvents.instance.EventSpaceshipSpawned.AddListener(OnEventSpaceshipSpawned);
    }

    void OnEventSpaceshipSpawned(ActorSpaceship ship)
    {
        if(ship.faction == FactionEnum.ENEMY)
        {
            AddActor(ship);
        }
    }


    void AddActor(ActorSpaceship ship)
    {
        GameObject sprite = Instantiate(
            targetProjectionPrefab,
            Vector2.zero,
            Quaternion.identity,
            transform);

        sprite.GetComponent<TargetProjectionIcon>().Init(ship, canvas, playerShip.transform);
    }
}
