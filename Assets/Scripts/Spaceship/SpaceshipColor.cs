using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipColor : MonoBehaviour
{
    Faction faction;
    // Start is called before the first frame update
    void Start()
    {
        ActorSpaceship actor = GetComponentInParent<ActorSpaceship>();
        if(actor != null)
        {
            ColorHull(actor.faction.hullColor);
        }
    }

    void ColorHull(Color color)
    {
        MeshRenderer meshRenderer = GetComponentInChildren<MeshRenderer>();
        meshRenderer.material.color = color;
    }
}
