using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VampireHealth : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    public ParticleSystem deathEffect;
    private Camera cam;
    public Animator deathAnim;

    public int health;

    private float invulnTimer;
    public float invulnCooldown;

    public float knockbackSpeedX = 5;
    public float knockbackSpeedY = 5;

    public Image[] healthIcons;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        cam = Camera.main;
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

    public void IncreaseHealth()
    {
        healthIcons[health].enabled = true;
        health += 1;
    }

    //Check for contact with enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject collider = collision.gameObject;

        if (collider.tag == "Enemy" && invulnTimer <= 0)
        {
            invulnTimer = invulnCooldown;
            health -= 1;

            //Set health icons
            healthIcons[health].color = new Color(1, 1, 1, 0.3f);

            cam.GetComponent<Animator>().Play("CameraShake");

            //Knockback direction
            if (collider.transform.position.x <= transform.position.x)
            {
                rb.velocity = new Vector2(knockbackSpeedX, knockbackSpeedY);
            }
            else
            {
                rb.velocity = new Vector2(-knockbackSpeedX, knockbackSpeedY);
            }

            //Handle death
            if (health <= 0)
            {
                Debug.Log("you dead");
                GetComponent<Animator>().Play("VampDeath");
                deathAnim.Play("DeathDie");
                deathEffect.Play();
            }
        }
    }
}
