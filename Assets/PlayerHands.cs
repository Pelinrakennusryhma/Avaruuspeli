using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public ResourceGatherer.ToolType CurrentTool;

    public GameObject Drill;
    public GameObject BlowTorch;

    public void SetTool(ResourceGatherer.ToolType tool)
    {
        Drill.SetActive(false);
        BlowTorch.SetActive(false);

        switch (tool)
        {
            case ResourceGatherer.ToolType.None:
                break;
            case ResourceGatherer.ToolType.Blowtorch:
                BlowTorch.SetActive(true);
                break;
            case ResourceGatherer.ToolType.Drill:
                Drill.SetActive(true);
                break;
            default:
                break;
        }
    }
}
