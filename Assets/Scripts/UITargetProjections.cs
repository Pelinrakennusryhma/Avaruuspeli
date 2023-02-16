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
    List<GameObject> projectionSprites = new List<GameObject>();
    List<TargetProjection> targetProjections = new List<TargetProjection>();

    Canvas canvas;

    float projectileSpeed = 10f;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
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


        for (int i = 0; i < enemyActors.Count; i++)
        {
            TargetProjection enemyTargetProjection = enemyActors[i].GetComponentInChildren<TargetProjection>();
            targetProjections.Add(enemyTargetProjection);

            GameObject sprite = Instantiate(
                targetProjectionPrefab,
                Vector2.zero,
                Quaternion.identity,
                transform);

            sprite.GetComponent<TargetProjectionIcon>().Init(enemyActors[i].GetComponentInChildren<SpaceshipEvents>());

            projectionSprites.Add(sprite);
        }
    }

    Vector2 GetPosOnCanvas(int index)
    {
        TargetProjection targetProjection = targetProjections[index];
        if (targetProjection == null) return Vector2.zero;
        Vector2 temp = Camera.main.WorldToViewportPoint(targetProjection.GetPosition(projectileSpeed));

        //Calculate position considering our percentage, using our canvas size
        //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
        temp.x *= canvas.renderingDisplaySize.x;
        temp.y *= canvas.renderingDisplaySize.y;

        //The result is ready, but, t$$anonymous$$s result is correct if canvas recttransform pivot is 0,0 - left lower corner.
        //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
        //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
        //returned value will still be correct.

        //temp.x -= canvas.sizeDelta.x * canvas.pivot.x;
        //temp.y -= canvas.sizeDelta.y * canvas.pivot.y;

        return temp;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < targetProjections.Count; i++)
        {
            GameObject sprite = projectionSprites[i];
            if (sprite != null)
            {
                sprite.transform.position = GetPosOnCanvas(i);
                //sprite.transform.LookAt(Camera.main.transform);
            }

        }
        //foreach (TargetProjection targetProjection in targetProjections)
        //{
        //    Debug.Log("tp: " + targetProjection.GetPosition(projectileSpeed) + " ship: " + targetProjection.transform.position);
        //}
    }
}
