using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VampireHealth : MonoBehaviour
{
    //Components
    private Rigidbody2D rb;
    public ParticleSystem deathEffect;
    private Camera cam;
    public Animator deathAnim;

    public int health;
    public bool isAbleToDie = true;

    private float invulnTimer;
    public float invulnCooldown;

    public float knockbackSpeedX = 5;
    public float knockbackSpeedY = 5;

    public Image[] healthIcons;

    public TextMeshProUGUI sunrise;
    private float sunriseTimer;

    public bool isCountingDown = true;

    public AudioSource hurtSource;

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

        if (isCountingDown)
        {
            sunriseTimer += Time.deltaTime;
        }

        if (sunriseTimer >= 300)
        {
            health = 0;
            sunriseTimer = 300;
            StartDeath();
        }

        float num = 300f - Mathf.Round(sunriseTimer * 10) / 10f;

        sunrise.text = "Sunrise: " + num.ToString() + "s";
        
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

            hurtSource.Play();

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
                StartDeath();
            }
        }
    }

    public void StartDeath()
    {
        //Make sure death effects are only triggered once
        if (isAbleToDie)
        {
            isAbleToDie = false;
            Debug.Log("you dead");

            //Play anim effects
            GetComponent<Animator>().Play("VampDeath");
            deathAnim.Play("DeathDie");
            deathEffect.Play();
        }
    }
}
