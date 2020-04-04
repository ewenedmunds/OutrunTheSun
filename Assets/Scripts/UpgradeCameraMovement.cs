using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeCameraMovement : MonoBehaviour
{

    public Transform target;
    public float moveSpeed;

    public GameObject selector;

    public TextMeshProUGUI upName;
    public TextMeshProUGUI upDescription;
    public TextMeshProUGUI upCount;
    public Button upButton;

    private DataStore data;

    // Start is called before the first frame update
    void Start()
    {
        data = GameObject.FindGameObjectWithTag("DataStore").GetComponent<DataStore>();

        upCount.text = "Available Upgrades: " + data.availableUpgrades.ToString();

        SetNewTarget(target.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, transform.position.y, -10);

        if (Input.GetKeyDown(KeyCode.A))
        {
            SetNewTarget(target.GetComponent<UpgradeTile>().leftNeighbour);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            SetNewTarget(target.GetComponent<UpgradeTile>().rightNeighbour);
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            SetNewTarget(target.GetComponent<UpgradeTile>().upNeighbour);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            SetNewTarget(target.GetComponent<UpgradeTile>().downNeighbour);
        }

        selector.transform.position = target.position;
    }

    //Move to a new upgrade tile in the scene
    public void SetNewTarget(GameObject newTarget)
    {
        target = newTarget.transform;

        //Change the UI text
        upName.text = newTarget.GetComponent<UpgradeTile>().name;
        upDescription.text = newTarget.GetComponent<UpgradeTile>().description;

        //See if the player is able to buy the upgrade
        upButton.interactable = false;
        if (data.availableUpgrades > 0 && newTarget.GetComponent<UpgradeTile>().isUnlocked == false && (newTarget.GetComponent<UpgradeTile>().downNeighbour == newTarget || newTarget.GetComponent<UpgradeTile>().downNeighbour.GetComponent<UpgradeTile>().isUnlocked)) { upButton.interactable = true;  }

    }

    //Buy a new upgrade
    public void SelectUpgrade()
    {
        //Update persistent data
        data.availableUpgrades -= 1;
        data.AddUpgrade(target.GetComponent<UpgradeTile>().name);

        //Change scene
        target.GetComponent<UpgradeTile>().isUnlocked = true;
        upButton.interactable = false;
        upCount.text = "Available Upgrades: " + data.availableUpgrades.ToString();
    }
}
