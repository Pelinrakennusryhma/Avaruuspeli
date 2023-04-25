using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public ResourceGatherer.ToolType CurrentTool;

    public GameObject DiamondDrill;
    public GameObject AdvancedDrill;
    public GameObject BasicDrill;

    public void SetTool(ResourceGatherer.ToolType tool)
    {
        AdvancedDrill.SetActive(false);
        BasicDrill.SetActive(false);
        DiamondDrill.SetActive(false);

        switch (tool)
        {
            case ResourceGatherer.ToolType.None:
                break;

            case ResourceGatherer.ToolType.BasicDrill:
                //Debug.Log("We are here 1");
                BasicDrill.SetActive(true);
                break;

            case ResourceGatherer.ToolType.AdvancedDrill:
                //Debug.Log("We are here 2");
                AdvancedDrill.SetActive(true);
                break;

             case ResourceGatherer.ToolType.DiamondDrill:
                DiamondDrill.SetActive(true);
                break;

            default:
                break;
        }
    }
}
