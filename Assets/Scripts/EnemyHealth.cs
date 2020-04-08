using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 2;

    private float stunTimer;
    private float invulnTimer;

    public ParticleSystem harmParticles;
    public ParticleSystem deathParticles;

    public Collider2D hurtbox;

    [Tooltip("Change how far an enemy is knocked back when attacked")]
    public float knockbackModifier = 15;

    [Space(10)]
    [Header("Audio Sources")]
    public AudioSource hurtSource;
    public AudioSource deathSource;

    // Update is called once per frame
    void Update()
    {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;

            //Re-enable enemy's ability to damage player at end of stun period
            if (stunTimer <= 0)
            {
                hurtbox.enabled = true;
            }
        }
        if (invulnTimer > 0)
        {
            invulnTimer -= Time.deltaTime;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Check if the trigger was player's attack
        if (collision.tag == "Claw" && invulnTimer <= 0) 
        {
            hurtbox.enabled = false;

            harmParticles.Play();

            stunTimer = 0.6f;
            invulnTimer = 0.3f;

            //Knockback from attack
            GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.localPosition.x * knockbackModifier, 4);

            //Change health
            int damage = 1;
            if (collision.gameObject.transform.parent.GetComponent<VampireMovement>() != null) { damage = collision.gameObject.transform.parent.GetComponent<VampireMovement>().damage;  }
            health -= damage;

            hurtSource.Play();

            if (health <= 0)
            {
                deathSource.Play();
                deathParticles.Play();
                SendMessage("Die", SendMessageOptions.DontRequireReceiver);
            }
            else
            {
                SendMessage("Hurt", damage, SendMessageOptions.DontRequireReceiver);
            }

            collision.gameObject.SendMessageUpwards("RegisterHit", SendMessageOptions.DontRequireReceiver);
        }
    }

    public bool IsStunned()
    {
        return stunTimer > 0;
    }
}
