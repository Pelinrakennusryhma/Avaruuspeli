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
    GameObject activeShield;
    // Start is called before the first frame update
    void Start()
    {
        originalMaterials = m_Renderer.materials;
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
        activeShield = Instantiate(shieldPrefab, transform);
    }

    public void UnShield()
    {
        Destroy(activeShield);
    }
}
