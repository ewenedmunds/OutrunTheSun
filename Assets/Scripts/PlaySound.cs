using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public AudioSource soundSource;

    public void Play()
    {
        soundSource.Play();
    }
}
