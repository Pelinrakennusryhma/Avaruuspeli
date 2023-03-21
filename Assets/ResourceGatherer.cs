using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGatherer : MonoBehaviour
{
    public enum ToolType
    {
        None = 0,
        BasicDrill = 1,
        AdvancedDrill = 2
    }

    public static ResourceGatherer Instance;
         
    public ToolType Tool;
    public DestroyableRock Rock;
    public Collider RockCollider;
    public FirstPersonPlayerControls Controls;
    public Camera Camera;
    public LayerMask PickUppableLayerMask;

    public PlayerHands Hands;

    public void Awake()
    {
        Instance = this; 
        Camera = transform.parent.GetComponentInChildren<Camera>();
        Tool = ToolType.None;
        Controls = transform.parent.GetComponent<FirstPersonPlayerControls>();
        Hands = transform.parent.GetComponentInChildren<PlayerHands>();

    }

    public void Start()
    {
        // Check if inventory has one equipped

        //ShowHideInventory.Instance.e
        Tool = GameManager.Instance.InventoryController.GetCurrentEquippedTool();

        Debug.Log("Setting tool to " + Tool.ToString());
        Hands.SetTool(Tool);
    }

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) 
        {
            //Debug.Log("Collect " + Time.time);
            GatherableObject gatherable = other.GetComponent<GatherableObject>();
            bool hasRoomInInventory = false;

            if (gatherable != null) 
            {

                ResourceInventory.ResourceType itemType = gatherable.ResourceType;

                Item item = null;

                switch (itemType)
                {
                    case ResourceInventory.ResourceType.None:
                        break;
                    case ResourceInventory.ResourceType.TestDice:
                        break;
                    case ResourceInventory.ResourceType.Gold:
                        item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(1);
                        break;
                    case ResourceInventory.ResourceType.Silver:
                        item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(8);
                        break;
                    case ResourceInventory.ResourceType.Copper:
                        item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(9);
                        break;
                    case ResourceInventory.ResourceType.Iron:
                        item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(0);
                        break;
                    case ResourceInventory.ResourceType.Diamond:
                        item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(10);
                        break;
                    default:
                        break;
                }

                if (item != null) 
                {
                    hasRoomInInventory = GameManager.Instance.InventoryController.Inventory.CheckIfWeHaveRoomForItem(item);
                }
            }

            if (gatherable != null
                && hasRoomInInventory)
            {
                gatherable.OnPickUp();
            }

            else
            {
                //Debug.Log("Null gatherable " + Time.time);
            }

            DestroyableRock rock = other.GetComponent<DestroyableRock>();

            if (rock != null)
            {
                Rock = rock;
                RockCollider = other;
                //Debug.Log("DEstroyable rock set");
            }
            //gameObject.SetActive(false);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<DestroyableRock>())
        {
            Rock = null;
            RockCollider = null;
            //Debug.Log("DestroyableRock cleared");
        }
    }

    private void Update()
    {
        if (Time.timeScale <= 0)
        {
            return;
        }

        if (Controls.Alpha1Down)
        {
            Tool = ToolType.BasicDrill;
            Hands.SetTool(Tool);
        }

        else if (Controls.Alpha2Down)
        {
            Tool = ToolType.AdvancedDrill;
            Hands.SetTool(Tool);
        }

        //Debug.Log("Tool is " + Tool.ToString());

        if (Rock != null)
        {
            // Chec if we are hitting rock with a raycast
            RaycastHit hitInfo;
            bool hittingRock = Physics.Raycast(Camera.transform.position, 
                                               Camera.transform.forward,
                                               out hitInfo,
                                               4.0f,
                                               PickUppableLayerMask);

            //Debug.Log("hitinfo " + hitInfo.collider);

            if (hitInfo.collider != null
                && hitInfo.collider.gameObject == Rock.gameObject)
            {
                hittingRock = true;
                //Debug.Log("Hitting rock " + Time.time);
            }

            else
            {
                hittingRock = false;
                //Debug.Log("Not hitting rock " + Time.time);
            }

            if (Controls.Fire1Down
                && hittingRock) 
            {
                if (Tool == ToolType.BasicDrill)
                {
                    Rock.ReduceHealth(0.3f * Time.deltaTime, Tool);
                }

                else if (Tool == ToolType.AdvancedDrill)
                {
                    Rock.ReduceHealth(Time.deltaTime, Tool);
                } 
            }
        }
    }
}
