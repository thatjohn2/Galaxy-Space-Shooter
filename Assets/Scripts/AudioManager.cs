using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource _backgroundMusic;

    // Start is called before the first frame update
    void Start()
    {
        _backgroundMusic = GetComponentInChildren<AudioSource>();
    }

    public void EndGameMusic()
    {
        _backgroundMusic.pitch = 0.55f;
    }
}
