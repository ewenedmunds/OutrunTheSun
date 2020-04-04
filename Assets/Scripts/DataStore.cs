using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStore : MonoBehaviour
{
    public List<string> upgrades;
    public List<string> fountains;
    public int availableUpgrades;

    private static DataStore instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddUpgrade(string upgrade)
    {
        upgrades.Add(upgrade);
    }

    public void AddFountain(string fountain)
    {
        fountains.Add(fountain);
    }
}
