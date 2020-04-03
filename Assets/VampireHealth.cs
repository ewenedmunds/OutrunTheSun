using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireHealth : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;

    public int health;

    private float invulnTimer;
    public float invulnCooldown;

    public float knockbackSpeedX = 5;
    public float knockbackSpeedY = 5;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (invulnTimer > 0)
        {
            invulnTimer -= Time.deltaTime;
        }
    }

    public void AddInvuln(float amount)
    {
        invulnTimer = Mathf.Max(invulnTimer, amount);
    }

    //Check for contact with enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;

        if (collider.tag == "Enemy" && invulnTimer <= 0)
        {
            invulnTimer = invulnCooldown;
            health -= 1;

            if (collider.transform.position.x <= transform.position.x)
            {
                rb.velocity = new Vector2(knockbackSpeedX, knockbackSpeedY);
            }
            else
            {
                rb.velocity = new Vector2(-knockbackSpeedX, knockbackSpeedY);
            }
        }
    }
}
