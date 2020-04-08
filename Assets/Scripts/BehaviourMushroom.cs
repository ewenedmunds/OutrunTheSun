using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviourMushroom : MonoBehaviour
{

    public float sporeDelay;
    public float delayRandomness;
    private float sporeTimer;

    public GameObject sporeField;

    private Animator anim;

    public AudioSource sporeSource;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        sporeTimer -= Time.deltaTime;
        if (sporeTimer <= 0)
        {
            sporeTimer = 6;
            anim.Play("MushroomAttack");
        }
    }

    public void SporeAttack()
    {
        sporeSource.Play();

        GameObject newSpores = Instantiate(sporeField);
        newSpores.transform.position = transform.position;

        sporeTimer = sporeDelay + Random.Range(0f, delayRandomness);
    }

    public void Die()
    {
        anim.Play("MushroomDie");
    }
}
