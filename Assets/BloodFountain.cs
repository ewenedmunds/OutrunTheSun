using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BloodFountain : MonoBehaviour
{

    public string name;
    public bool isAvailable;

    public Sprite EmptyFountainSprite;
    public SpriteRenderer[] decoration;
    public TextMeshPro toolTip;
    
    private DataStore data;
    private SpriteRenderer sr;
    private ParticleSystem passiveEffect;
    private ParticleSystem consumeEffect;

    private Transform playerPos;

    // Start is called before the first frame update
    void Start()
    {
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        passiveEffect = transform.GetChild(1).GetComponent<ParticleSystem>();
        consumeEffect = transform.GetChild(1).GetComponent<ParticleSystem>();

        playerPos = GameObject.FindGameObjectWithTag("Player").transform;

        data = GameObject.FindGameObjectWithTag("DataStore").GetComponent<DataStore>();
        if (data.fountains.Contains(name))
        {
            Empty();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isAvailable)
        {
            float alpha = Mathf.Max((5 - (transform.position - playerPos.position).magnitude) / 5, 0);
            toolTip.color = new Color(1, 1, 1, alpha);
            foreach (SpriteRenderer spr in decoration)
            {
                spr.color = new Color(0.3f, 0.1f, 0.1f, alpha/2f);
            }
        }    
    }

    private void Empty()
    {
        toolTip.enabled = false;
        foreach (SpriteRenderer spr in decoration)
        {
            spr.gameObject.SetActive(false);
        }
    }

    public void Consume()
    {
        isAvailable = false;
        data.fountains.Add(name);
        data.availableUpgrades += 1;

        consumeEffect.Play();

        toolTip.enabled = false;
        foreach (SpriteRenderer spr in decoration)
        {
            spr.gameObject.SetActive(false);
        }
    }
}
