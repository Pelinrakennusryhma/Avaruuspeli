using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetProjectionIcon : MonoBehaviour
{
    SpaceshipEvents _spaceshipEvents;
    Image icon;
    Color originalColor;
    TargetProjection targetProjection;
    Canvas _canvas;
    SpaceshipShoot targetterShoot;
    Target targetScript;

    //void Start()
    //{
    //    icon = GetComponent<Image>();
    //    originalColor = icon.color;
    //}

    private void Awake()
    {
        GameEvents.Instance.EventPlayerSpaceshipDied.AddListener(OnPlayerDied);
    }

    public void Init(GameObject targetter)
    {
        targetScript = GetComponent<Target>();
        originalColor = targetScript.TargetColor;
        targetterShoot = targetter.GetComponent<SpaceshipShoot>();

        targetProjection = GetComponentInParent<TargetProjection>();
        SpaceshipEvents events = targetProjection.GetComponent<SpaceshipEvents>();
        events.EventSpaceshipHitByPlayer.AddListener(OnShipHitByPlayer);
    }

    void OnPlayerDied()
    {
        Destroy(gameObject);
    }

    void OnShipHitByPlayer()
    {
        StartCoroutine(FlashColor());
        Debug.Log("hit by player");
    }

    IEnumerator FlashColor()
    {
        targetScript.SetColor(Color.red);
        yield return new WaitForSeconds(0.15f);
        targetScript.SetColor(originalColor);
    }

    private void Update()
    {
        //Debug.Log(targetterShoot);
        transform.position = targetProjection.GetPosition(targetterShoot.laserSpeed, targetterShoot.transform.position);
    }
}
