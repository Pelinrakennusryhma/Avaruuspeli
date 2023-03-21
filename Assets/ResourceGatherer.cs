using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGatherer : MonoBehaviour
{
    public enum ToolType
    {
        None = 0,
        Blowtorch = 1,
        Drill = 2
    }

    public ToolType Tool;
    public List<DestroyableRock> Rocks;
    public List<Collider> RockColliders;
    public FirstPersonPlayerControls Controls;
    public Camera Camera;
    public LayerMask PickUppableLayerMask;

    public PlayerHands Hands;

    public void Awake()
    {
        Camera = transform.parent.GetComponentInChildren<Camera>();
        Tool = ToolType.Blowtorch;
        Controls = transform.parent.GetComponent<FirstPersonPlayerControls>();
        Hands = transform.parent.GetComponentInChildren<PlayerHands>();
        Hands.SetTool(Tool);
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
                gatherable.OnPickUp();
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
        if (Controls.Alpha1Down)
        {
            Tool = ToolType.Blowtorch;
            Hands.SetTool(Tool);
        }

        else if (Controls.Alpha2Down)
        {
            Tool = ToolType.Drill;
            Hands.SetTool(Tool);
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
                if (Tool == ToolType.Blowtorch)
                {
                    hitRock.ReduceHealth(0.3f * Time.deltaTime, Tool);
                }

                else if (Tool == ToolType.Drill)
                {
                    hitRock.ReduceHealth(Time.deltaTime, Tool);
                } 
            }
        }
    }
}
