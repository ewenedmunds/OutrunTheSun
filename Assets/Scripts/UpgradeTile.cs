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

    private void OnMouseDown()
    {
        Camera.main.transform.parent.GetComponent<UpgradeCameraMovement>().SetNewTarget(gameObject);
    }
}
