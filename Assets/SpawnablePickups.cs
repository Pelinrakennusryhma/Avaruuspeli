using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnablePickups : MonoBehaviour
{
    public GatherableDice[] Dices;
    public GatherableGold[] GoldPieces;
    public GatherableSilver[] SilverPieces;
    public GatherableCopper[] CopperPieces;
    public GatherableIron[] IronPieces;
    public GatherbaleDiamond[] Diamonds;
    [SerializeField] List<GameObject> gatherablePrefabs;

    public GameObject DiceParent;
    public GameObject GoldParent;
    public GameObject IronParent;
    public GameObject SilverParent;
    public GameObject CopperParent;
    public GameObject DiamondParent;

    public ResourceInventory.ResourceType ResourceType;
    public int Amount;

    [SerializeField]
    CenterOfGravity _centerOfGravity;

    public int Setup(ResourceInventory.ResourceType resourceType, AsteroidLauncher.MineralDensity mineralDensity, CenterOfGravity centerOfGravity)
    {
        ResourceType = resourceType;
        _centerOfGravity = centerOfGravity;
        return Amount = DecideAmount(mineralDensity);
    }

    GameObject GetResourcePrefab()
    {
        switch (ResourceType)
        {
            case ResourceInventory.ResourceType.None:
                return null;
            case ResourceInventory.ResourceType.TestDice:
                return gatherablePrefabs[0];
            case ResourceInventory.ResourceType.Gold:
                return gatherablePrefabs[1];
            case ResourceInventory.ResourceType.Silver:
                return gatherablePrefabs[2];
            case ResourceInventory.ResourceType.Copper:
                return gatherablePrefabs[3];
            case ResourceInventory.ResourceType.Iron:
                return gatherablePrefabs[4];
            case ResourceInventory.ResourceType.Diamond:
                return gatherablePrefabs[5];
            default:
                return null;
        }
    }

    public void Decorate(GameObject rock)
    {
        for (int i = 0; i < Amount; i++)
        {
            MeshFilter meshFilter = rock.GetComponentInChildren<MeshFilter>();
            Vector3[] allVerts = meshFilter.mesh.vertices;
            Vector3 randomVert = allVerts[Random.Range(0, allVerts.Length)];
            Vector3 spawnPos = meshFilter.transform.TransformPoint(randomVert);
            GameObject decoration = Instantiate(GetResourcePrefab(), spawnPos, Random.rotation, meshFilter.transform);
            GatherableObject gatherableScript = decoration.GetComponent<GatherableObject>();
            gatherableScript.enabled = false;
        }
    }

    public void Spawn()
    {
        DiceParent.SetActive(false);
        GoldParent.SetActive(false);
        IronParent.SetActive(false);
        SilverParent.SetActive(false);
        CopperParent.SetActive(false);
        DiamondParent.SetActive(false);

        switch (ResourceType)
        {
            case ResourceInventory.ResourceType.None:
                break;

            case ResourceInventory.ResourceType.TestDice:
                SpawnDices();
                break;

            case ResourceInventory.ResourceType.Gold:
                SpawnGold();
                break;

            case ResourceInventory.ResourceType.Silver:
                SpawnSilver();
                break;

            case ResourceInventory.ResourceType.Copper:
                SpawnCopper();
                break;

            case ResourceInventory.ResourceType.Iron:
                SpawnIron();
                break;

            case ResourceInventory.ResourceType.Diamond:
                SpawnDiamonds();
                break;

            default:
                break;
        }
    }

    public void SpawnRandom()
    {
        DiceParent.SetActive(false);
        GoldParent.SetActive(false);
        IronParent.SetActive(false);
        SilverParent.SetActive(false);
        CopperParent.SetActive(false);
        DiamondParent.SetActive(false);

        int spawn = Random.Range(1,6);

        //SpawnDices();

        if (spawn == 0)
        {
            SpawnDices();
        }

        else if (spawn == 1)
        {
            SpawnGold();
        }

        else if (spawn == 2)
        {
            SpawnIron();
        }

        else if (spawn == 3)
        {
            SpawnCopper();
        }

        else if (spawn == 4) 
        {
            SpawnSilver();
        }

        else if (spawn == 5)
        {
            SpawnDiamonds();
        }
    }

    private void SpawnDiamonds()
    {
        DiamondParent.SetActive(true);
        
        for (int i = 0; i < Diamonds.Length; i++)
        {
            Diamonds[i].gameObject.SetActive(false);
        }

        int amount = Amount;

        //amount = DecideAmount(amount);

        int spawned = 0;

        while (true)
        {

            for (int i = 0; i < Diamonds.Length; i++)
            {
                int rand = Random.Range(0, Diamonds.Length);

                if (rand == 0
                    && spawned < amount
                    && !Diamonds[i].gameObject.activeSelf)
                {
                    Diamonds[i].gameObject.SetActive(true);
                    Diamonds[i].OnSpawn(_centerOfGravity);
                    spawned++;
                }
            }

            if (spawned >= amount)
            {
                break;
            }

        }

        //for (int i = 0; i < Diamonds.Length; i++)
        //{
        //    Diamonds[i].OnSpawn();
        //}
    }

    private static int DecideAmount(AsteroidLauncher.MineralDensity mineralDensity)
    {
        int amount = 0;

        switch (mineralDensity)
        {
            case AsteroidLauncher.MineralDensity.None:
                break;
            case AsteroidLauncher.MineralDensity.Scarce:
                amount = Random.Range(1, 3);
                break;
            case AsteroidLauncher.MineralDensity.Medium:
                amount = Random.Range(3, 5);
                break;
            case AsteroidLauncher.MineralDensity.High:
                amount = Random.Range(5, 7);
                break;
            case AsteroidLauncher.MineralDensity.Highest:
                amount = Random.Range(6, 8);
                break;
            default:
                break;
        }

        return amount;
    }

    private void SpawnSilver()
    {
        SilverParent.SetActive(true);

        for (int i = 0; i < SilverPieces.Length; i++)
        {
            SilverPieces[i].gameObject.SetActive(false);
        }


        int amount = Amount;
        //amount = DecideAmount(amount);

        int spawned = 0;

        while (true)
        {

            for (int i = 0; i < SilverPieces.Length; i++)
            {
                int rand = Random.Range(0, SilverPieces.Length);

                if (rand == 0
                    && spawned < amount
                    && !SilverPieces[i].gameObject.activeSelf)
                {
                    SilverPieces[i].gameObject.SetActive(true);
                    SilverPieces[i].OnSpawn(_centerOfGravity);
                    spawned++;
                }
            }

            if (spawned >= amount)
            {
                break;
            }

        }
        //for (int i = 0; i < SilverPieces.Length; i++)
        //{
        //    SilverPieces[i].OnSpawn();
        //}
    }

    private void SpawnCopper()
    {
        CopperParent.SetActive(true);

        for (int i = 0; i < CopperPieces.Length; i++)
        {
            CopperPieces[i].gameObject.SetActive(false);
        }

        int amount = Amount;

        //amount = DecideAmount(amount);

        int spawned = 0;

        while (true)
        {

            for (int i = 0; i < CopperPieces.Length; i++)
            {
                int rand = Random.Range(0, CopperPieces.Length);

                if (rand == 0
                    && spawned < amount
                    && !CopperPieces[i].gameObject.activeSelf)
                {
                    CopperPieces[i].gameObject.SetActive(true);
                    CopperPieces[i].OnSpawn(_centerOfGravity);
                    spawned++;
                }
            }

            if (spawned >= amount)
            {
                break;
            }

        }

        //for (int i = 0; i < CopperPieces.Length; i++)
        //{
        //    CopperPieces[i].OnSpawn();
        //}
    }

    private void SpawnIron()
    {
        IronParent.SetActive(true);

        for (int i = 0; i < IronPieces.Length; i++)
        {
            IronPieces[i].gameObject.SetActive(false);
        }

        int amount = Amount;
        //amount = DecideAmount(amount);

        int spawned = 0;

        while (true)
        {

            for (int i = 0; i < IronPieces.Length; i++)
            {
                int rand = Random.Range(0, IronPieces.Length);

                if (rand == 0
                    && spawned < amount
                    && !IronPieces[i].gameObject.activeSelf)
                {
                    IronPieces[i].gameObject.SetActive(true);
                    IronPieces[i].OnSpawn(_centerOfGravity);
                    spawned++;
                }
            }

            if (spawned >= amount)
            {
                break;
            }

        }

        //for (int i = 0; i < IronPieces.Length; i++)
        //{
        //    IronPieces[i].OnSpawn();
        //}
    }

    private void SpawnGold()
    {
        GoldParent.SetActive(true);

        for (int i = 0; i < GoldPieces.Length; i++)
        {
            GoldPieces[i].gameObject.SetActive(false);
        }

        int amount = Amount;
        //amount = DecideAmount(amount);

        int spawned = 0;

        while (true)
        {

            for (int i = 0; i < GoldPieces.Length; i++)
            {
                int rand = Random.Range(0, GoldPieces.Length);

                if (rand == 0
                    && spawned < amount
                    && !GoldPieces[i].gameObject.activeSelf)
                {
                    GoldPieces[i].gameObject.SetActive(true);
                    GoldPieces[i].OnSpawn(_centerOfGravity);
                    spawned++;
                }
            }

            if (spawned >= amount)
            {
                break;
            }

        }

        //for (int i = 0; i < GoldPieces.Length; i++)
        //{
        //    GoldPieces[i].OnSpawn();
        //}
    }

    private void SpawnDices()
    {
        DiceParent.SetActive(true);

        for (int i = 0; i < Dices.Length; i++)
        {
            Dices[i].OnSpawn(_centerOfGravity);
        }
    }
}
