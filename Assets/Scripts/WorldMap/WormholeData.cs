using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WormholeData : UniverseData
{
    public int ID;
    public int GalaxyId;

    public int PairWormholeGalaxyID;

    public WormholeData PairWormhole; // This doesn't serialize properly and I don't know why. Doesn't show up in editor
}
