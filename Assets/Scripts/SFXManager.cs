using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance { get; private set; }

    private AudioSource _audioSource;

    public AudioClip jump;
    public AudioClip switchA;
    public AudioClip switchB;
    public AudioClip targetIn;
    public AudioClip targetOut;
    public AudioClip targetSwitch;
    public AudioClip explode;
    public AudioClip lose;
    public AudioClip win;
    public AudioClip pause;
    public AudioClip start;
    public AudioClip quit;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Play(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}
