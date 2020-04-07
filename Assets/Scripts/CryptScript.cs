using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CryptScript : MonoBehaviour
{
    private bool isAbleToWin = true;

    public Animator sunriseAnim;
    public Animator panelAnim;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && isAbleToWin)
        {
            isAbleToWin = false;
            sunriseAnim.speed = 0;
            panelAnim.Play("VictoryAnim");
            GameObject.FindGameObjectWithTag("Player").GetComponent<VampireHealth>().isCountingDown = false;
        }
    }
}
