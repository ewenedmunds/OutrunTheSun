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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (stunTimer > 0)
        {
            stunTimer -= Time.deltaTime;
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
            GetComponent<Rigidbody2D>().velocity = new Vector2(collision.transform.localPosition.x * 18, 4);

            //Change health
            health -= 1;
            if (health <= 0)
            {
                deathParticles.Play();
                SendMessage("Die");
            }

            collision.gameObject.SendMessageUpwards("RegisterHit");
        }
    }

    public bool IsStunned()
    {
        return stunTimer > 0;
    }
}
