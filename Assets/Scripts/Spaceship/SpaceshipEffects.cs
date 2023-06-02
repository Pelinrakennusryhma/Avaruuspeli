using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipEffects : MonoBehaviour
{
    [SerializeField]
    MeshRenderer m_Renderer;
    [SerializeField]
    Material[] electrifiedMaterials;
    Material[] originalMaterials;

    [SerializeField]
    GameObject shieldPrefab;
    ShieldEffect activeShield;
    int cameraIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        originalMaterials = m_Renderer.materials;

        if (IsOnPlayerShip())
        {
            GameEvents.Instance.EventCameraChanged.AddListener(OnCameraChanged);
        }
    }

    bool IsOnPlayerShip()
    {
        PlayerControls playerControls = GetComponentInParent<PlayerControls>();
        return playerControls != null;
    }

    void OnCameraChanged(int camIndex)
    {
        cameraIndex = camIndex;
        if(camIndex == 0)
        {
            if(activeShield != null)
            {
                activeShield.ActivateInsideShield();
            }
        } else
        {
            if (activeShield != null)
            {
                activeShield.ActivateOutsideShield();
            }
        }
    }

    public void Electrify()
    {
        Material[] changedMaterials = new Material[originalMaterials.Length];

        for (int i = 0; i < electrifiedMaterials.Length; i++)
        {
            if(electrifiedMaterials[i] != null)
            {
                changedMaterials[i] = electrifiedMaterials[i];
            } else
            {
                changedMaterials[i] = originalMaterials[i];
            }
        }

        m_Renderer.materials = changedMaterials;
    }

    public void UnElectrify()
    {
        m_Renderer.materials = originalMaterials;
    }

    public void Shield()
    {
        GameObject shieldObj = Instantiate(shieldPrefab, transform);
        activeShield = shieldObj.GetComponent<ShieldEffect>();

        if(cameraIndex == 0)
        {
            activeShield.ActivateInsideShield();
        } else
        {
            activeShield.ActivateOutsideShield();
        }
    }

    public void UnShield()
    {
        Destroy(activeShield.gameObject);
    }
}
