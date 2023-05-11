using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEffect : MonoBehaviour
{
    [SerializeField] // normals facing inside
    GameObject insideEffect;
    [SerializeField] // normals facing outside
    GameObject outsideEffect;

    public void ActivateInsideShield()
    {
        insideEffect.SetActive(true);
        outsideEffect.SetActive(false);
    }

    public void ActivateOutsideShield()
    {
        insideEffect.SetActive(false);
        outsideEffect.SetActive(true);
    }
}
