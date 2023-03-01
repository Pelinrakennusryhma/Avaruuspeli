using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaverLoader : MonoBehaviour
{
    public UniverseSaveData SaveData;

    public const string Path = "/UniverseData.json";


    public bool LoadUniverse()
    {
        bool success = ReadFromFile();
        //Debug.Log("Trying to LOAD universe");

        return success;
    }

    public List<GalaxyData> GetSavedGalaxyDatas()
    {
        return SaveData.Galaxies;
    }

    public void SaveUniverse(List<GalaxyData> galaxies)
    {
        SaveData.Galaxies = galaxies;


        WriteToFile();
        //Debug.Log("Trying to SAVE universe");
    }

    public bool LoadGalaxy(int galaxyId,
                           out GalaxyData data)
    {
        bool success = false;
        data = null;
        //Debug.Log("Trying to LOAD galaxy");

        for (int i = 0; i < SaveData.Galaxies.Count; i++)
        {
            if (SaveData.Galaxies[i].ID == galaxyId)
            {
                data = SaveData.Galaxies[i];
                success = true;
                break;
            }
        }

        return success;
    }

    public void SaveGalaxy(int galaxyId,
                           List<StarSystemData> starSystems)
    {

        bool found = false;

        for (int i = 0; i < SaveData.Galaxies.Count; i++)
        {
            if (SaveData.Galaxies[i].ID == galaxyId)
            {
                found = true;
                SaveData.Galaxies[i].StarSystems = starSystems;
                break;
            }
        }

        if (found) 
        {
            Debug.Log("Found a galaxy");
        }

        else
        {
            Debug.Log("Didn't find a galaxy");
        }
        //Debug.Log("Trying to SAVE galaxy");

        WriteToFile();
    }

    public bool LoadStarSystem(int galaxyId,
                               int starSystemId,
                               out StarSystemData data)
    {
        bool success = false;
        data = null;

        //Debug.Log("Trying to LOAD star system");

        for (int i = 0; i < SaveData.Galaxies.Count; i++)
        {
            if (SaveData.Galaxies[i].ID == galaxyId)
            {
                for (int j = 0; j < SaveData.Galaxies[i].StarSystems.Count; j++)
                {
                    if (SaveData.Galaxies[i].StarSystems[j].ID == starSystemId)
                    {
                        data = SaveData.Galaxies[i].StarSystems[j];
                        success = true;
                        //Debug.LogWarning("We found the correct star system in iteration");
                        break;
                    }
                }

                //Debug.LogWarning("WE found the correct galaxy in iteration");
                break;
            }
        }

        return success;
    }

    public void SaveStarSystem(int galaxyId,
                               int starsystemId,
                               CenterStarData centerStar,
                               List<PlanetData> planets,
                               List<AsteroidFieldData> asteroidFields,
                               Vector3 pos,
                               Quaternion rot,
                               Vector3 localScale,
                               StarSystemData data)
    {

        Debug.LogWarning("Caliing save star system with star system id " + starsystemId + " planets lenght is " + planets.Count);
        bool found = false;

        for (int i = 0; i < SaveData.Galaxies.Count; i++)
        {
            if (SaveData.Galaxies[i].ID == galaxyId)
            {
                Debug.Log("Iterating through the found galaxy");

                int foundJ = -1;

                for (int j = 0; j < SaveData.Galaxies[i].StarSystems.Count; j++)
                {
                    Debug.Log("Iterating star system. Now at id " + SaveData.Galaxies[i].StarSystems[j].ID + " searching for " + starsystemId + " star systems lenght is " + SaveData.Galaxies[i].StarSystems.Count);

                    if (SaveData.Galaxies[i].StarSystems[j].ID == starsystemId)
                    {
                        found = true;
                        foundJ = j;

                        if (found)
                        {
                            Debug.LogError("Found the j with star system id " + starsystemId);

                        }
                        //break;
                    }
                }


                StarSystemData starSystemData = new StarSystemData();                
                
                if (data != null) 
                {
                    starSystemData = data;
                }

                else
                {
                    starSystemData.ID = starsystemId;
                    starSystemData.CenterStar = centerStar;
                    starSystemData.Planets = planets;
                    starSystemData.Asteroids = asteroidFields;
                    starSystemData.SetPosRotAndScale(pos, rot, localScale);
                }




                if (found
                    && foundJ >= 0)
                {                    
                    SaveData.Galaxies[i].StarSystems[foundJ] = starSystemData;
                    Debug.LogError("Putting star system to previously found pos");

                }

                else
                {
                    SaveData.Galaxies[i].StarSystems.Add(starSystemData);
                    Debug.LogError("Putting star system to new pos");
                }
            }

            if (found)
            {
                break;
            }
        }

        if (found)
        {
            Debug.Log("Found a star system");
        }

        else
        {
            Debug.Log("Didn't find a star system");
        }

        WriteToFile();

        //Debug.Log("Trying to SAVE star system");
    }

    public void SavePlanet(int galaxyId,
                           int starSystemId,
                           PlanetData data)
    {
        for (int i = 0; i < SaveData.Galaxies.Count; i++)
        {
            if (SaveData.Galaxies[i].ID == galaxyId)
            {
                for (int j = 0; j < SaveData.Galaxies[i].StarSystems.Count; j++)
                {
                    if (SaveData.Galaxies[i].StarSystems[j].ID == starSystemId)
                    {
                        bool foundAPlanetToReplace = false;
                        int foundK = -1;

                        for (int k = 0; k < SaveData.Galaxies[i].StarSystems[j].Planets.Count; k++)
                        {
                            if (SaveData.Galaxies[i].StarSystems[j].Planets[k].ID == data.ID)
                            {
                                foundAPlanetToReplace = true;
                                foundK = k;
                                Debug.Log("Found a planet to replace");
                                break;
                            }
                        }

                        if (foundAPlanetToReplace
                            && foundK >= 0)
                        {
                            SaveData.Galaxies[i].StarSystems[j].Planets[foundK] = data;
                        }

                        else
                        {
                            SaveData.Galaxies[i].StarSystems[j].Planets.Add(data);
                        }

                        break;
                    }
                }
                break;
            }
        }
    }

    // This maybe is called too much, if every time a star system is generated we write to file????
    public void WriteToFile()
    {
        string universeDataString = JsonUtility.ToJson(SaveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + Path, universeDataString);
        Debug.LogWarning("WRITING TO SAVE FILE");
    }

    public bool ReadFromFile()
    {
        Debug.LogWarning("READING FROM SAVE FILE");
        bool succesfullRead = false;

        string saveFile = Application.persistentDataPath + Path;

        if (System.IO.File.Exists(saveFile))
        {
            Debug.Log("WE have a save file that exists");

            string savedUniverse = System.IO.File.ReadAllText(saveFile);
            SaveData = JsonUtility.FromJson<UniverseSaveData>(savedUniverse);

            if (SaveData != null)
            {
                succesfullRead = true;
                Debug.Log("We had a valid save data object");
            }

            else
            {
                succesfullRead = false;
                SaveData = new UniverseSaveData();
            }
        }

        return succesfullRead;
    }
}
