using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public ResourceInventory.ResourceType ResourceType;
  
    public void OnPickUp()
    {
        gameObject.SetActive(false);
        
        if (ResourceInventory.Instance != null)
        {
            ResourceInventory.Instance.CollectResource(ResourceType);
        }
    }
}
