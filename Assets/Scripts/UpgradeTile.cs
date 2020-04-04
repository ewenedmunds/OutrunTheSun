using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeTile : MonoBehaviour
{

    public GameObject leftNeighbour;
    public GameObject rightNeighbour;
    public GameObject downNeighbour;
    public GameObject upNeighbour;

    public string name;
    public string description;

    public bool isUnlocked;

    private void Start()
    {
        if (GameObject.FindGameObjectWithTag("DataStore").GetComponent<DataStore>().upgrades.Contains(name))
        {
            isUnlocked = true;
        }
    }

    //Allows a player to click on an upgrade tile to navigate directly there
    private void OnMouseDown()
    {
        Camera.main.transform.parent.GetComponent<UpgradeCameraMovement>().SetNewTarget(gameObject);
    }
}
