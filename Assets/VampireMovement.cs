using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VampireMovement : MonoBehaviour
{

    public float maxSpeed;
    public float accelerationSpeed;

    public float jumpPower;

    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, jumpPower));
        }

        if (Input.GetKey(KeyCode.A))
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
        else if (Input.GetKey(KeyCode.D))
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
        else
        {
            rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, accelerationSpeed * Time.deltaTime), rb.velocity.y);
            if (IsGrounded())
            {
                anim.Play("VampIdle");
            }
        }

        if (!IsGrounded())
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

    private bool IsGrounded()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down, 0.55f, groundLayer).collider != null)
        {
            return true;
        }

        return false;
    }
}
