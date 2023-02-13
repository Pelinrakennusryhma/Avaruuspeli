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

    public GameObject DiceParent;
    public GameObject GoldParent;
    public GameObject IronParent;
    public GameObject SilverParent;
    public GameObject CopperParent;

    public void Spawn()
    {
        DiceParent.SetActive(false);
        GoldParent.SetActive(false);
        IronParent.SetActive(false);
        SilverParent.SetActive(false);
        CopperParent.SetActive(false);

        int spawn = Random.Range(1,5);

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
    }

    private void SpawnSilver()
    {
        SilverParent.SetActive(true);

        for (int i = 0; i < SilverPieces.Length; i++)
        {
            SilverPieces[i].OnSpawn();
        }
    }

    private void SpawnCopper()
    {
        CopperParent.SetActive(true);

        for (int i = 0; i < CopperPieces.Length; i++)
        {
            CopperPieces[i].OnSpawn();
        }
    }

    private void SpawnIron()
    {
        IronParent.SetActive(true);

        for (int i = 0; i < IronPieces.Length; i++)
        {
            IronPieces[i].OnSpawn();
        }
    }

    private void SpawnGold()
    {
        GoldParent.SetActive(true);

        for (int i = 0; i < GoldPieces.Length; i++)
        {
            GoldPieces[i].OnSpawn();
        }
    }

    private void SpawnDices()
    {
        DiceParent.SetActive(true);

        for (int i = 0; i < Dices.Length; i++)
        {
            Dices[i].OnSpawn();
        }
    }
}
