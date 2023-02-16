using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetProjectionIcon : MonoBehaviour
{
    SpaceshipEvents _spaceshipEvents;
    Image icon;
    Color originalColor;
    // Start is called before the first frame update
    void Start()
    {
        icon = GetComponent<Image>();
        originalColor = icon.color;
    }

    public void Init(SpaceshipEvents spaceshipEvents)
    {
        _spaceshipEvents = spaceshipEvents;
        spaceshipEvents.EventSpaceshipHealthChanged.AddListener(OnShipHealthChanged);
        spaceshipEvents.EventSpaceshipDied.AddListener(OnShipDied);
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
        yield return new WaitForSeconds(0.1f);
        icon.color = originalColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
