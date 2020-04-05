using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSkeleton : MonoBehaviour
{
    public float startingSpeed;

    private int edgeDetection = 1;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyHealth health;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();

        rb.velocity = new Vector2(startingSpeed, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (!health.IsStunned() && IsGrounded())
        {
            rb.velocity = new Vector2(edgeDetection * startingSpeed, rb.velocity.y);
        }

        //Check if the skeleton has approached a wall to the side
        if (Physics2D.Raycast(transform.position + new Vector3(0, -0.3f,0), new Vector2(edgeDetection,0), 0.4f, groundLayer).collider != null && IsGrounded())
        {
            Flip();
        }
        //Check if the skeleton has approached a ledge to the side
        else if (Physics2D.Raycast(transform.position + new Vector3(edgeDetection*0.5f,0,0), new Vector2(0, -1), 0.7f, groundLayer).collider == null && IsGrounded())
        {
            Flip();
        }
    }

    //Change direction of movement
    void Flip()
    {
        edgeDetection *= -1;
        rb.velocity = new Vector2(edgeDetection * startingSpeed, rb.velocity.y);

        sr.flipX = !sr.flipX;
    }

    private bool IsGrounded()
    {
        for (int i = -1; i <= 1; i++)
        {
            if (Physics2D.Raycast(transform.position + new Vector3(i * 0.2f, 0, 0), Vector2.down, 0.55f, groundLayer).collider != null)
            {
                return true;
            }
        }

        return false;
    }

    public void Die()
    {
        GetComponent<Animator>().Play("SkeletonDie");
    }
}
