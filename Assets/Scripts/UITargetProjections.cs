using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITargetProjections : MonoBehaviour
{
    [SerializeField]
    ActorManager actorManager;
    [SerializeField]
    GameObject targetProjectionPrefab;
    List<GameObject> projectionSprites = new List<GameObject>();
    List<TargetProjection> targetProjections = new List<TargetProjection>();

    float projectileSpeed = 10f;

    private void Awake()
    {
        GameEvents.instance.EventSpaceshipSpawned.AddListener(OnEventSpaceshipSpawned);
        GameEvents.instance.EventSpaceshipDied.AddListener(OnEventSpaceshipDied);
    }

    private void Start()
    {
        UpdateActors();
    }

    void OnEventSpaceshipSpawned(ActorSpaceship ship)
    {
        UpdateActors();
    }

    void OnEventSpaceshipDied(ActorSpaceship ship)
    {
        UpdateActors();
    }

    void UpdateActors()
    {
        foreach (GameObject sprite in projectionSprites)
        {
            Destroy(sprite);
        }
        projectionSprites.Clear();

        targetProjections.Clear();
        List<ActorSpaceship> enemyActors = actorManager.GetActors(Faction.PLAYER);

        foreach (ActorSpaceship enemyActor in enemyActors)
        {
            TargetProjection enemyTargetProjection = enemyActor.GetComponentInChildren<TargetProjection>();
            targetProjections.Add(enemyTargetProjection);

            GameObject sprite = Instantiate(
                targetProjectionPrefab, 
                enemyTargetProjection.GetPosition(projectileSpeed), 
                Quaternion.identity, 
                enemyTargetProjection.transform);

            projectionSprites.Add(sprite);
        }
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < targetProjections.Count; i++)
        {
            GameObject sprite = projectionSprites[i];
            if (sprite != null)
            {
                sprite.transform.position = targetProjections[i].GetPosition(projectileSpeed);
                sprite.transform.LookAt(Camera.main.transform);
            }

        }
        //foreach (TargetProjection targetProjection in targetProjections)
        //{
        //    Debug.Log("tp: " + targetProjection.GetPosition(projectileSpeed) + " ship: " + targetProjection.transform.position);
        //}
    }
}
