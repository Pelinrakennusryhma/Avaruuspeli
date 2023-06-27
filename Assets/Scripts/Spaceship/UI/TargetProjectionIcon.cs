using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetProjectionIcon : MonoBehaviour
{
    Color originalColor;
    TargetProjection targetProjection;
    SpaceshipShoot targetterShoot;
    Target targetScript;
    public bool pulsing;

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
    }

    IEnumerator FlashColor()
    {
        targetScript.SetColor(Color.red);
        yield return new WaitForSeconds(0.15f);
        targetScript.SetColor(originalColor);
    }

    private void Update()
    {
        if(targetProjection != null && targetterShoot != null)
        {
            transform.position = targetProjection.GetPosition(targetterShoot.laserSpeed, targetterShoot.transform.position);
        }

        if (pulsing)
        {
            float scaleAdjust = Mathf.PingPong(Time.time * 1f, 0.25f);
            targetScript.SetScale(1f - scaleAdjust);
        } else
        {
            targetScript.SetScale(1f);
        }
    }
}
