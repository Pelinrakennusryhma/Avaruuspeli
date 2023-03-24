using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSurface : MonoBehaviour
{
    //// Start is called before the first frame update

    //public DestroyableRock[] Rocks;

    //public bool HasBeenInitialized;

    //public void Init()
    //{
    //    if (!HasBeenInitialized)
    //    {
    //        HasBeenInitialized = true;        
    //        Rocks = GetComponentsInChildren<DestroyableRock>();
    //    }
    //}

    //public void Awake()
    //{
    //    Init();
    //}

    //public void CompareCollider(Collider collider,
    //                            out ResourceInventory.ResourceType resourceType,
    //                            out int amount)
    //{
    //    if (!HasBeenInitialized)
    //    {
    //        Init();
    //    }

    //    if(collider == null)
    //    {
    //        resourceType = ResourceInventory.ResourceType.None;
    //        amount = 0;
    //        //Debug.Log("Null collider");
    //        return;
    //    }

    //    resourceType = ResourceInventory.ResourceType.None;
    //    amount = 0;

    //    for (int i = 0; i < Rocks.Length; i++)
    //    {
    //        if (collider.gameObject
    //            == Rocks[i].gameObject)
    //        {
    //            //Debug.Log("Found a matching collider " + Rocks[i].gameObject.name);
    //            //resourceType = Rocks[i].ResourceType;
    //            amount = Rocks[i].ResourceAmount;
    //            break;
    //        }
    //    }
    //}
}
