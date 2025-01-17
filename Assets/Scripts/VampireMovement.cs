﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireMovement : MonoBehaviour
{
    //Movement Variables
    public float maxSpeed;
    public float accelerationSpeed;

    public float jumpPower;

    private bool isBusy;

    //Collision
    public LayerMask groundLayer;
    public GameObject interactionSphere;

    //Double Jump
    public bool isAbleToDoubleJump = false;
    private int jumpsAvailable;
    private int maxJumpsAvailable;
    public ParticleSystem doubleJumpParticles;

    //Claw Attack
    public float attackCooldown;
    private float attackTimer;
    public GameObject clawAttack;
    public int damage = 1;
    private int healthRecover;
    private bool isVampirism = false;

    //Dash
    public float dashCooldown;
    private float dashTimer;
    private bool isAbleToDash = false;
    private bool isBatAspect = false;
    public GameObject dashSlash;
    private bool isDashing = false;
    public ParticleSystem batParticles;

    //Components
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private VampireHealth health;
    private DataStore data;

    private Camera cam;

    //Audio Sources
    [Space(10)]
    [Header("Audio Sources")]
    public AudioSource jumpSource;
    public AudioSource clawSource;
    public AudioSource dashSource;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        health = GetComponent<VampireHealth>();
        data = GameObject.FindGameObjectWithTag("DataStore").GetComponent<DataStore>();
        cam = Camera.main;

        loadUpgrades();
    }

    private void loadUpgrades()
    {
        if (data.upgrades.Contains("Batwing Cloak")) { maxJumpsAvailable += 1; }
        if (data.upgrades.Contains("Owlfeather Cloak")) { maxJumpsAvailable += 1; }

        if (data.upgrades.Contains("Wolf Aspect")) { maxSpeed += 1; }
        if (data.upgrades.Contains("Misty Step")) { isAbleToDash = true; }
        if (data.upgrades.Contains("Bestial Claws")) { damage = 2; }

        if (data.upgrades.Contains("Supernatural Resistance")) { health.IncreaseHealth(); }
        if (data.upgrades.Contains("Undead Fortitude")) { health.IncreaseHealth(); }

        if (data.upgrades.Contains("Vampirism")) { isVampirism = true; }
        if (data.upgrades.Contains("Bat Aspect")) { isBatAspect = true; }
    }

    private void Update()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        if (attackTimer > 0)
        {
            attackTimer -= Time.deltaTime;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        IsGrounded();

        //Dash
        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && dashTimer <= 0 && !IsBusy() && isAbleToDash)
        {
            dashTimer = dashCooldown;
            isBusy = true;

            dashSource.Play();

            health.AddInvuln(0.3f);

            anim.Play("VampDash");
            isDashing = true;

            rb.velocity = new Vector2(1.6f*maxSpeed, 0);
            if (sr.flipX == true)
            {
                rb.velocity = new Vector2(-1.6f * maxSpeed, 0);
            }
            if (isBatAspect)
            {
                batParticles.Play();

                dashSlash.SetActive(true);
                dashSlash.GetComponent<Collider2D>().enabled = true;

                if (sr.flipX == true)
                {
                    dashSlash.transform.localPosition = new Vector3(0.25f, 0, 0);
                    dashSlash.transform.localScale = new Vector3(-1, 1, 1);
                }
                else
                {
                    dashSlash.transform.localPosition = new Vector3(-0.25f, 0, 0);
                    dashSlash.transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        //Jumping
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && (IsGrounded() || (jumpsAvailable > 0 && isAbleToDoubleJump)) && !IsBusy())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpPower));

            jumpSource.Play();

            if (!IsGrounded())
            {
                jumpsAvailable -= 1;
                doubleJumpParticles.Play();
            }
        }

        //Claw attack
        if (Input.GetMouseButtonDown(0) && attackTimer <= 0 && !IsBusy())
        {
            VampAttack();
        }

        //Move Left
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && !IsBusy())
        {
            if (rb.velocity.x > -maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, -maxSpeed, accelerationSpeed * Time.deltaTime), rb.velocity.y);
            }

            sr.flipX = true;

            if (IsGrounded())
            {
                anim.Play("VampRun");
            }
        }

        //Move Right
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && !IsBusy())
        {
            if (rb.velocity.x < maxSpeed)
            {
                rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, maxSpeed, accelerationSpeed * Time.deltaTime), rb.velocity.y);
            }

            sr.flipX = false;

            if (IsGrounded())
            {
                anim.Play("VampRun");
            }
        }

        //Idle, slow velocity
        else if (!IsBusy())
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, accelerationSpeed * Time.deltaTime), rb.velocity.y);
            if (IsGrounded())
            {
                anim.Play("VampIdle");
            }
        }

        //Falling
        if (!IsGrounded() && !IsBusy())
        {
            if (rb.velocity.y > 0)
            {
                anim.Play("VampJump");
            }
            else
            {
                anim.Play("VampFall");
            }
        }

        //Interaction
        if (Input.GetKeyDown(KeyCode.E) && !IsBusy())
        {
            GameObject newInteraction = Instantiate(interactionSphere);
            newInteraction.transform.position = transform.position;
        }

    }

    void EndDash()
    {
        isBusy = false;
        anim.Play("VampIdle");
        isDashing = false;

        if (isBatAspect)
        {
            dashSlash.SetActive(false);
            dashSlash.GetComponent<Collider2D>().enabled = false;
        }
    }

    void EndClaw()
    {
        isBusy = false;
        anim.Play("VampIdle");
    }

    //Check whether we are on the ground
    private bool IsGrounded()
    {
        for (int i = -1; i <= 1; i++)
        {
            if (Physics2D.Raycast(transform.position + new Vector3(i*0.2f,0,0), Vector2.down, 0.55f, groundLayer).collider != null)
            {
                jumpsAvailable = maxJumpsAvailable;
                return true;
            }
        }
       
        return false;
    }

    private bool IsBusy()
    {
        return isBusy || (health.health <= 0);
    }

    //Melee claw attack
    void VampAttack()
    {
        attackTimer = attackCooldown;
        isBusy = true;

        rb.velocity = new Vector2(maxSpeed, rb.velocity.y);

        //Check if mouse is on the right side of the screen
        Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
        if (cam.ScreenToWorldPoint(pos).x > transform.position.x)
        {
            sr.flipX = false;

            //Set claw hurtbox
            clawAttack.GetComponent<SpriteRenderer>().flipX = false;
            clawAttack.transform.localPosition = new Vector3(0.5f, 0, 0);
        }
        else
        {
            sr.flipX = true;
            rb.velocity = new Vector2(-maxSpeed, rb.velocity.y);

            clawAttack.GetComponent<SpriteRenderer>().flipX = true;
            clawAttack.transform.localPosition = new Vector3(-0.5f, 0, 0);
        }

        anim.Play("VampClaw");

        clawSource.Play();
    }

    //Register a melee hit on an enemy
    public void RegisterHit()
    {
        if (isVampirism)
        {
            healthRecover += damage;
            if (healthRecover >= 15)
            {
                healthRecover -= 15;
                health.GainHealth();
            }
        }

        if (!isDashing)
        {
            isBusy = false;
            rb.velocity = new Vector2(-rb.velocity.x * 0.7f, rb.velocity.y);

            //Allow player to attack again quicker
            attackTimer = Mathf.Min(attackTimer, 0.1f);

            anim.Play("VampIdle");
        }
    }
}
