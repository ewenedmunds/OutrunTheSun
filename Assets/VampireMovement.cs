using System.Collections;
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

    //Double Jump
    public bool isAbleToDoubleJump = false;
    private bool isJumpAvailable;
    public ParticleSystem doubleJumpParticles;

    //Claw Attack
    public float attackCooldown;
    private float attackTimer;
    public GameObject clawAttack;

    //Dash
    public float dashCooldown;
    private float dashTimer;

    //Components
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    

    private Camera cam;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        cam = Camera.main;
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
        if (Input.GetKeyDown(KeyCode.LeftShift) && dashTimer <= 0 && !isBusy)
        {
            dashTimer = dashCooldown;
            isBusy = true;

            anim.Play("VampDash");

            rb.velocity = new Vector2(1.6f*maxSpeed, 0);
            if (sr.flipX == true)
            {
                rb.velocity = new Vector2(-1.6f*maxSpeed, 0);
            }
        }

        //Jumping
        if (Input.GetKeyDown(KeyCode.Space) && (IsGrounded() || (isJumpAvailable && isAbleToDoubleJump)) && !isBusy)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpPower));

            if (!IsGrounded())
            {
                isJumpAvailable = false;
                doubleJumpParticles.Play();
            }
        }

        //Claw attack
        if (Input.GetMouseButtonDown(0) && attackTimer <= 0 && !isBusy)
        {
            attackTimer = attackCooldown;
            isBusy = true;

            rb.velocity = new Vector2(maxSpeed, rb.velocity.y);
            //Check if mouse is on the right side of the screen
            Vector3 pos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 5);
            if (cam.ScreenToWorldPoint(pos).x > transform.position.x)
            {
                sr.flipX = false;
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
        }

        //Move Left
        if (Input.GetKey(KeyCode.A) && !isBusy)
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
        else if (Input.GetKey(KeyCode.D) && !isBusy)
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
        else if (!isBusy)
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, accelerationSpeed * Time.deltaTime), rb.velocity.y);
            if (IsGrounded())
            {
                anim.Play("VampIdle");
            }
        }

        //Falling
        if (!IsGrounded() && !isBusy)
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
    }

    void EndDash()
    {
        isBusy = false;
        anim.Play("VampIdle");
    }

    void EndClaw()
    {
        Debug.Log("Called a end claw");
        isBusy = false;
        anim.Play("VampIdle");
    }

    private bool IsGrounded()
    {
        for (int i = -1; i <= 1; i++)
        {
            if (Physics2D.Raycast(transform.position + new Vector3(i*0.2f,0,0), Vector2.down, 0.55f, groundLayer).collider != null)
            {
                isJumpAvailable = true;
                return true;
            }
        }
        

        return false;
    }
}
