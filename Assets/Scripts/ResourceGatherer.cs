using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGatherer : MonoBehaviour
{
    public enum ToolType
    {
        None = 0,
        BasicDrill = 1,
        AdvancedDrill = 2,
        DiamondDrill = 3
    }

    public static ResourceGatherer Instance;
         
    public ToolType Tool;
    public List<DestroyableRock> Rocks;
    public List<Collider> RockColliders;
    public FirstPersonPlayerControls Controls;
    public Camera Camera;
    public LayerMask PickUppableLayerMask;

    public PlayerHands Hands;

    Rigidbody rb;
    // Audio
    private EventInstance drillSFX;

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

        //Debug.Log("Setting tool to " + Tool.ToString());
        Hands.SetTool(Tool);

        drillSFX = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.Drill);
        rb = GetComponentInParent<Rigidbody>();
        RuntimeManager.AttachInstanceToGameObject(drillSFX, transform, rb);
    }

    // Start is called before the first frame update
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6) 
        {
            //Debug.Log("Collect " + Time.time);
            GatherableObject gatherable = other.GetComponent<GatherableObject>();

            if (gatherable != null && gatherable.enabled)
            {
                bool weHaveRoom = GameManager.Instance.InventoryController.Inventory.CheckIfWeHaveRoomForItem(gatherable.ResourceType, 1);

                if (weHaveRoom)
                {
                    GameManager.Instance.InventoryController.Inventory.UpdateWeight(gatherable.ResourceType.weight);
                    gatherable.OnPickUp();
                    //Debug.Log("WE have room for item. Can collect");
                }

                else
                {
                    //Debug.Log("No room for pickup");
                }
                

            }

            else
            {
                //Debug.Log("Null gatherable " + Time.time);
            }

            DestroyableRock rock = other.GetComponent<DestroyableRock>();

            if (rock != null)
            {
                Rocks.Add(rock);
                RockColliders.Add(other);
                //Debug.Log("DEstroyable rock set");
            }
            //gameObject.SetActive(false);
        }
    }

    public void OnTriggerExit(Collider other)
    {
        DestroyableRock otherRock = other.GetComponent<DestroyableRock>();
        if(otherRock != null)
        {
            Rocks.Remove(otherRock);
            RockColliders.Remove(other);
            //Debug.Log("DestroyableRock cleared");
        }
    }

    private void Update()
    {
        if (Time.timeScale <= 0)
        {
            return;
        }

        if (Controls.Alpha1Down
            && Tool != ToolType.BasicDrill
            && GameManager.Instance.InventoryController.Inventory.CheckForItem(7))
        {
            Tool = ToolType.BasicDrill;
            Hands.SetTool(Tool);
            Hands.SetWeapon(Weapon.WeaponType.None);
        }

        else if (Controls.Alpha2Down
                 && Tool != ToolType.AdvancedDrill
                 && GameManager.Instance.InventoryController.Inventory.CheckForItem(8))
        {
            Tool = ToolType.AdvancedDrill;
            Hands.SetTool(Tool);
            Hands.SetWeapon(Weapon.WeaponType.None);
        }

        else if (Controls.Alpha3Down
                 && Tool != ToolType.DiamondDrill
                 && GameManager.Instance.InventoryController.Inventory.CheckForItem(21))
        {
            Tool = ToolType.DiamondDrill;
            Hands.SetTool(Tool);
            Hands.SetWeapon(Weapon.WeaponType.None);
        }

        //Debug.Log("Tool is " + Tool.ToString());

        if (Rocks.Count > 0)
        {
            // Chec if we are hitting rock with a raycast
            RaycastHit hitInfo;
            bool hittingRock = Physics.Raycast(Camera.transform.position, 
                                               Camera.transform.forward,
                                               out hitInfo,
                                               4.0f,
                                               PickUppableLayerMask);

            //Debug.Log("hitinfo " + hitInfo.collider);


            if (hitInfo.collider != null && RockColliders.Contains(hitInfo.collider))
            //&& hitInfo.collider.gameObject == Rock.gameObject)
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
                DestroyableRock hitRock = hitInfo.collider.GetComponent<DestroyableRock>();
                if (Tool == ToolType.BasicDrill)
                {
                    hitRock.ReduceHealth(0.3f * Time.deltaTime, Tool);
                    UpdateSound(true);
                }

                else if (Tool == ToolType.AdvancedDrill)
                {
                    hitRock.ReduceHealth(Time.deltaTime, Tool);
                    UpdateSound(true);
                }

                else if (Tool == ToolType.DiamondDrill)
                {
                    hitRock.ReduceHealth(Time.deltaTime * 1.5f, Tool);
                    UpdateSound(true);
                }
                
            }
            else
            {
                UpdateSound(false);
            }
        }
    }

    void UpdateSound(bool shouldPlay)
    {
        // play engine sound when 'forward' is held down.. or something else?
        if (shouldPlay)
        {
            PLAYBACK_STATE playbackState;
            drillSFX.getPlaybackState(out playbackState);
            RuntimeManager.AttachInstanceToGameObject(drillSFX, transform, rb);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                drillSFX.start();
            }
        }
        else
        {
            drillSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
