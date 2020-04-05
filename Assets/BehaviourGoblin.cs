using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourGoblin : MonoBehaviour
{
    public float startingSpeed;

    private int edgeDetection = 1;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyHealth health;

    //Spear things
    public GameObject spearPrefab;
    private GameObject mySpear;
    public Transform anchorPos;

    //Aiming
    private GameObject player;
    public float sightRadius;
    private bool isAiming = false;
    public float aimTime;
    public float throwPower;
    private float aimTimer = 2;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();

        rb.velocity = new Vector2(startingSpeed, 0);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {

        //Can we see the player directly?
        Collider2D col = Physics2D.Raycast(transform.position, -transform.position + player.transform.position, sightRadius, groundLayer).collider;
        if (col != null && col.gameObject == player && !isAiming)
        {
            AimSpear();
        }

        if (isAiming && aimTimer > 0)
        {
            aimTimer -= Time.deltaTime;
            if (aimTimer <= 0)
            {
                ThrowSpear();
            }
        }

        if (isAiming)
        {
            //Set rotation of held spear
            var dir = player.transform.position - mySpear.transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
            mySpear.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    //Change direction of movement
    void Flip()
    {
        edgeDetection *= -1;
        rb.velocity = new Vector2(edgeDetection * startingSpeed, rb.velocity.y);

        sr.flipX = !sr.flipX;
    }

    private void AimSpear()
    {
        isAiming = true;
        aimTimer = aimTime;

        //Create new external spear object
        mySpear = Instantiate(spearPrefab, this.transform);
        mySpear.transform.position = anchorPos.position;

        GetComponent<Animator>().Play("GoblinAimSpear");
    }

    public void ThrowSpear()
    {
        if (isAiming)
        {
            isAiming = false;
            mySpear.GetComponent<Rigidbody2D>().velocity = mySpear.transform.up * throwPower;
            mySpear.transform.parent = null;

            GetComponent<Animator>().Play("GoblinIdle");
        }
    }

    private bool IsGrounded()
    {
        for (int i = -1; i <= 1; i++)
        {
            if (Physics2D.Raycast(transform.position + new Vector3(i * 0.2f, 0, 0), Vector2.down, 0.55f * transform.localScale.x, groundLayer).collider != null)
            {
                return true;
            }
        }

        return false;
    }

    public void Die()
    {
        if (mySpear != null)
        {
            Destroy(mySpear);
        }
        GetComponent<Animator>().Play("GoblinDie");
    }
}
