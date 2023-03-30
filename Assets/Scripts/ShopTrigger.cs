using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
  
    public bool PlayerIsInTriggerArea;

    public bool PlayerIsInShoppingAreaFacingShop;

    void Start()
    {
        PlayerIsInShoppingAreaFacingShop = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool facesShop = false;

        Vector3 toShop2D = transform.position - ResourceInventory.Instance.transform.position;
        toShop2D = new Vector3(toShop2D.x, 0, toShop2D.z).normalized;

        float angleBetweenForwardAndFacingDirection = Vector3.Angle(toShop2D, ResourceInventory.Instance.transform.forward);

        //Debug.Log("Angle between is " + angleBetweenForwardAndFacingDirection);

        if (angleBetweenForwardAndFacingDirection <= 56.0f)
        {
            facesShop = true;
        }

        if (facesShop && PlayerIsInTriggerArea)
        {
            if (!PlayerIsInShoppingAreaFacingShop) 
            {
                GameManager.Instance.InventoryController.OnEnterShoppingArea();
                PlayerIsInShoppingAreaFacingShop = true;            
                Debug.Log("Entered shopping area");
            }


        }
        else
        {
            if (PlayerIsInShoppingAreaFacingShop) 
            {
                GameManager.Instance.InventoryController.OnExitShoppingArea();
                PlayerIsInShoppingAreaFacingShop = false;            
                Debug.Log("Exited shopping area");
            }

        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerIsInTriggerArea = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerIsInTriggerArea = false;

        }
    }
}
