using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float maxSpeed;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(Mathf.Clamp(Mathf.Lerp(transform.position.x, player.transform.position.x, maxSpeed * Time.deltaTime), minX, maxX), Mathf.Clamp(Mathf.Lerp(transform.position.y, player.transform.position.y, maxSpeed * Time.deltaTime), minY, maxY), transform.position.z);
    }
}
