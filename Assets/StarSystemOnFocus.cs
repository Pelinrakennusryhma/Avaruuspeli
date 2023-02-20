using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemOnFocus : MonoBehaviour
{
    public CenterStarOnWorldMap CenterStar;

    public PlanetOnWorldMap[] Planets;

    public AsteroidFieldOnWorldMap[] AsteroidFields;

    public bool HasBeenInitted;

    public void Awake()
    {
        if (!HasBeenInitted)
        {
            Initialize();
        }
    }

    private void Initialize()
    {
        Planets = GetComponentsInChildren<PlanetOnWorldMap>(true);

        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].Init(CenterStar);
            Planets[i].SetInitialStartingPosition();
        }

        HasBeenInitted = true;
    }

    public void OnBecomeFocused()
    {
        if (!HasBeenInitted)
        {
            Initialize();
        }

        SetToBigScale();

        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].DrawOrbit();
        }
    }

    public void OnBecomeUnfocused()
    {
        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].HideOrbit();
        }
    }

    public void SetToBigScale()
    {
        Debug.Log("Set star system to big scale");
    }

    public void SetToSmallScale()
    {
        Debug.Log("Set star system to small scale");
    }
}
