using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourGhost : MonoBehaviour
{

    public float startingSpeed;
    public float detectionRadius;
    public float chaseRadius;

    public LayerMask groundAndPlayerLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyHealth health;

    public bool isChasing = false;

    private GameObject player;

    private Vector3 home;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();

        player = GameObject.FindGameObjectWithTag("Player");

        home = new Vector3(transform.position.x, transform.position.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, startingSpeed * Time.deltaTime), Mathf.Lerp(rb.velocity.y, 0, startingSpeed * Time.deltaTime));

        if (isChasing)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, startingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, home, startingSpeed * Time.deltaTime);
        }

        if (isChasing)
        {
            if ((player.transform.position - transform.position).magnitude > chaseRadius) { isChasing = false; }

            if (player.transform.position.x < transform.position.x)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
        else
        {
            Debug.DrawRay(transform.position, (-transform.position + player.transform.position));
            Collider2D col = Physics2D.Raycast(transform.position, (-transform.position + player.transform.position), detectionRadius, groundAndPlayerLayer).collider;
            if (col != null && col.gameObject == player)
            {
                isChasing = true;
            }
        }

    }

    public void Die()
    {
        GetComponent<Animator>().Play("GhostDie");
    }
}
