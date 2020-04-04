using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSphere : MonoBehaviour
{
    private float timer;

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.1f)
        {
            foreach (Collider2D collider in Physics2D.OverlapCircleAll(transform.position, 1.5f))
            {
                collider.gameObject.SendMessage("Interact", SendMessageOptions.DontRequireReceiver);
            }
            Destroy(gameObject);
        }
    }

}
