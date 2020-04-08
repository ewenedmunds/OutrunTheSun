using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourSpectre : MonoBehaviour
{
    //Ghost Attributes
    public float startingSpeed;
    public float detectionRadius;
    public float chaseRadius;

    public LayerMask groundAndPlayerLayer;

    //Components
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private EnemyHealth health;
    private Collider2D contact;
    private GameObject hurtbox;
    private GameObject player;
    private Vector3 home;
    private Animator anim;

    //Boss things
    public GameObject arm;
    public GameObject ghostWallRight;
    public GameObject ghostWallLeft;
    public GameObject clawingHands;

    public int damagePhaseThreshold = 10;
    private int phase = 0;

    public GameObject ghostPrefab;

    public string bossState = "";
    private float bossTimer = 0;

    public GameObject leftChains;
    public GameObject rightChains;

    public AudioSource angerSource;
    public AudioSource chaseSource;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<EnemyHealth>();
        contact = GetComponent<Collider2D>();
        hurtbox = transform.GetChild(2).gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();

        home = new Vector3(transform.position.x, transform.position.y, 0);

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(Mathf.Lerp(rb.velocity.x, 0, startingSpeed * Time.deltaTime), Mathf.Lerp(rb.velocity.y, 0, startingSpeed * Time.deltaTime));

        //Chase player directly
        if (bossState == "Chase")
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, startingSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, home, 2*startingSpeed * Time.deltaTime);

            //If spectre has returned to centre of arena
            if ((home-transform.position).magnitude <= 0.3f)
            {
                //Activate arm raise and summon 
                if (bossState == "ArmSummon")
                {
                    bossState = "Summoning";
                    
                    sr.flipX = true;
                    arm.GetComponent<SpriteRenderer>().flipX = true;

                    anim.Play("SpectreWallAttack", 2);
                    anim.Play("SpectreArmRaise", 1);
                }
                else if (bossState == "FloorSummon")
                {
                    bossState = "Summoning";

                    anim.Play("SpectreFloorAttack", 2);
                }
            }
        }

        if (bossState == "Chase")
        {
            if ((player.transform.position - transform.position).magnitude > chaseRadius) { bossState = ""; }

            if (player.transform.position.x < transform.position.x)
            {
                sr.transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                sr.transform.localScale = new Vector3(1, 1, 1);
            }
        }
        else if (bossState == "")
        {

            Debug.DrawRay(transform.position, (-transform.position + player.transform.position));
            Collider2D col = Physics2D.Raycast(transform.position, (-transform.position + player.transform.position), detectionRadius, groundAndPlayerLayer).collider;
            if (col != null && col.gameObject == player)
            {
                bossState = "Chase";
                chaseSource.Play();
                leftChains.SetActive(true);
            }
        }

    }

    public void Hurt(int amount)
    {
        rb.velocity = new Vector2(rb.velocity.x, Random.Range(-30f, 30f));

        chaseRadius = 50;
        detectionRadius = 50;

        damagePhaseThreshold -= amount;
        if (damagePhaseThreshold <= 0)
        {
            phase += 1;

            angerSource.Play();

            if (phase >= 3)
            {
                for (int i = -1; i <= 1; i+=2)
                {
                    GameObject newGhost = Instantiate(ghostPrefab);
                    newGhost.transform.position = home + new Vector3(i * 10, 10, 0);
                }
            }

            startingSpeed += 0.5f;

            sr.transform.localScale = new Vector3(1, 1, 1);

            damagePhaseThreshold = 10;
            sr.color = new Color(1, 1, 1, 0.4f);

            contact.enabled = false;
            hurtbox.SetActive(false);

            if (phase % 2 == 0)
            {
                bossState = "ArmSummon";
            }
            else
            {
                bossState = "FloorSummon";
            }
        }
    }


    public void SummonLeftWall()
    {
        GameObject newWall = Instantiate(ghostWallLeft);
    }

    public void SummonRightWall()
    {
        GameObject newWall = Instantiate(ghostWallRight);
    }

    public void SummonFloor()
    {
        GameObject newWall = Instantiate(clawingHands);
    }

    public void EndAttack()
    {
        bossState = "";
        anim.Play("SpectreArmDown", 1);
        anim.Play("SpectreEmptyAnim", 2);

        sr.color = new Color(1, 1, 1, 1f);

        contact.enabled = true;
        hurtbox.SetActive(true);
    }

    public void Die()
    {
        GetComponent<Animator>().Play("SpectreDie");

        leftChains.SetActive(false);
        rightChains.SetActive(false);
    }
}
