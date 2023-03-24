using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Faction", order = 1)]
public class Faction : ScriptableObject
{
    public string factionName;
    public Color hullColor;
    public Color laserColor;
    public List<Faction> hostileFactions = new List<Faction>();
    public List<ActorSpaceship> hostileActors = new List<ActorSpaceship>();
}
