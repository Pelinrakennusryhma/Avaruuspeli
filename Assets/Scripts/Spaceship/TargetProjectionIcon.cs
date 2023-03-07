using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetProjectionIcon : MonoBehaviour
{
    SpaceshipEvents _spaceshipEvents;
    Image icon;
    Color originalColor;
    float projectileSpeed;
    TargetProjection _targetProjection;
    Canvas _canvas;
    Transform _targetter;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
        originalColor = icon.color;
    }

    public void Init(ActorSpaceship ship, Canvas canvas, Transform targetter)
    {
        _canvas = canvas;
        _spaceshipEvents = ship.GetComponentInChildren<SpaceshipEvents>();
        _targetter = targetter;
        _targetProjection = ship.GetComponentInChildren<TargetProjection>();
        SpaceshipShoot spaceshipShoot = ship.GetComponentInChildren<SpaceshipShoot>();
        projectileSpeed = spaceshipShoot.laserSpeed;
        _spaceshipEvents.EventSpaceshipHitByPlayer.AddListener(OnShipHitByPlayer);
        _spaceshipEvents.EventSpaceshipDied.AddListener(OnShipDied);
        GameEvents.Instance.EventPlayerSpaceshipDied.AddListener(OnPlayerDied);
    }

    void OnShipDied()
    {
        Destroy(gameObject);
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
        icon.color = Color.red;
        yield return new WaitForSeconds(0.15f);
        icon.color = originalColor;
    }

    private void FixedUpdate()
    {
        UpdateDrawPosition();
    }

    void UpdateDrawPosition()
    {
        Vector3 projectionPos = _targetProjection.GetPosition(projectileSpeed, _targetter.position);

        Vector3 pos = Camera.main.WorldToScreenPoint(projectionPos);
        //Debug.Log("pos.z: " + pos.z);
        //pos.z = 0f;
        if (pos.z > 0)
        {
            Debug.Log("posOnScreen: " + pos + "screen: " + Screen.width + "x" + Screen.height);
            // Target projection is in front of the camera



        }
        else
        {
            // draw on the edge of the screen somehow
            //Vector3 pos = Camera.main.WorldToScreenPoint(projectionPos);
            //pos.z = 0f;
            //transform.position = pos;
            Debug.Log("posOffScreen: " + pos);
            pos.x = -pos.x;
            pos.y = -pos.y;


        }
        pos.x = Mathf.Clamp(pos.x, 0, Screen.width);
        pos.y = Mathf.Clamp(pos.y, 0, Screen.height);
        transform.position = pos;
        Debug.Log("finalPos: " + pos);
    }
}
