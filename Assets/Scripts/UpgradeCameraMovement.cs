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
    public Button upButton;

    // Start is called before the first frame update
    void Start()
    {
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

    public void SetNewTarget(GameObject newTarget)
    {
        target = newTarget.transform;

        upName.text = newTarget.GetComponent<UpgradeTile>().name;
        upDescription.text = newTarget.GetComponent<UpgradeTile>().description;

        upButton.interactable = false;
        if (newTarget.GetComponent<UpgradeTile>().isUnlocked == false) { upButton.interactable = true;  }

    }
}
