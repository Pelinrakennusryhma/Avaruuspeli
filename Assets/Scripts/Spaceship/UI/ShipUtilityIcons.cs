using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipUtilityIcons : MonoBehaviour
{
    [SerializeField]
    GameObject iconPrefab;
    void Awake()
    {
        GameEvents.Instance.EventPlayerUtilitiesInited.AddListener(OnPlayerUtilitiesInited);
    }

    void OnPlayerUtilitiesInited(List<IUseable> utils)
    {
        for (int i = 0; i < utils.Count; i++)
        {
            GameObject go = Instantiate(iconPrefab, transform);
            ShipUtilityIcon icon = go.GetComponent<ShipUtilityIcon>();
            icon.Init(utils[i], i + 1);
        }
    }

}
