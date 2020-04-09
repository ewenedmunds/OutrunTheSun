using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{

    private bool isSleeping = true;

    private float velX;
    private float velY;

    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isSleeping)
        {
            transform.Translate(new Vector3(velX, velY, 0) * Time.deltaTime);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isSleeping)
        {
            isSleeping = false;
            velX = Random.Range(-3f, 3f);
            velY = 7 - Mathf.Abs(velX);
            anim.Play("BatFly");
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
