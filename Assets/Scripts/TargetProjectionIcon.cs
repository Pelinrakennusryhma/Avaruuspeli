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
        _spaceshipEvents.EventSpaceshipHealthChanged.AddListener(OnShipHealthChanged);
        _spaceshipEvents.EventSpaceshipDied.AddListener(OnShipDied);
    }

    void OnShipDied()
    {
        Destroy(gameObject);
    }

    void OnShipHealthChanged()
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
        Vector3 projectionPos = _targetProjection.GetPosition(projectileSpeed, _targetter.position);
        Vector3 heading = projectionPos - Camera.main.transform.position;
        if (Vector3.Dot(Camera.main.transform.forward, heading) > 0)
        {
            // Target projection is in front of the camera
            transform.position = Camera.main.WorldToScreenPoint(projectionPos);
        }
        
    }

    //Vector2 GetPosOnCanvas()
    //{
    //    Vector2 temp = Camera.main.WorldToViewportPoint(_targetProjection.GetPosition(projectileSpeed));

    //    //Calculate position considering our percentage, using our canvas size
    //    //So if canvas size is (1100,500), and percentage is (0.5,0.5), current value will be (550,250)
    //    temp.x *= _canvas.renderingDisplaySize.x;
    //    temp.y *= _canvas.renderingDisplaySize.y;

    //    //The result is ready, but, t$$anonymous$$s result is correct if canvas recttransform pivot is 0,0 - left lower corner.
    //    //But in reality its middle (0.5,0.5) by default, so we remove the amount considering cavnas rectransform pivot.
    //    //We could multiply with constant 0.5, but we will actually read the value, so if custom rect transform is passed(with custom pivot) , 
    //    //returned value will still be correct.

    //    //RectTransform rect = _canvas.GetComponent<RectTransform>();

    //    //temp.x -= _canvas.renderingDisplaySize.x * rect.pivot.x;
    //    //temp.y -= _canvas.renderingDisplaySize.y * rect.pivot.y;

    //    return temp;
    //}
}
