using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetLauncher : MonoBehaviour
{
    public void LaunchPlanet(GalaxyData galaxyData,
                             StarSystemData starSystemData, 
                             PlanetData planetData)
    {
        Debug.Log("We should launch a planet at galaxy" 
                  + galaxyData.ID 
                  + " at star system " 
                  + starSystemData.ID
                  + " with planet id " 
                  +  planetData.ID);
    }
}
