using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormholeOnWorldMap : MonoBehaviour
{
    public WorldMapClickDetector ClickDetector;

    public WormholeData WormholeData;

    public void Awake()
    {
        ClickDetector = GetComponent<WorldMapClickDetector>();
        ClickDetector.OnObjectClicked -= OnWormholeClicked;
        ClickDetector.OnObjectClicked += OnWormholeClicked;
    }

    public void OnWormholeClicked(WorldMapClickDetector.ClickableObjectType type)
    {
        //Debug.LogWarning("We clicked a wromhole on world map. Preparing to teleportation");
        MotherShipOnWorldMapController.Instance.TeleportFromWormhole(this);
    }
}
