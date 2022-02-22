using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
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
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayJump()
    {
        _audioSource.clip = jump;
        _audioSource.Play();
    }

    public void PlaySwitch()
    {
        if (Random.Range(0, 2) == 0) _audioSource.clip = switchA;
        else _audioSource.clip = switchB;
        _audioSource.Play();
    }

    public void PlayTargetIn()
    {
        _audioSource.clip = targetIn;
        _audioSource.Play();
    }
    
    public void PlayTargetOut()
    {
        _audioSource.clip = targetOut;
        _audioSource.Play();
    }

    public void PlayTargetSwitch()
    {
        _audioSource.clip = targetSwitch;
        _audioSource.Play();
    }

    public void PlayExplode()
    {
        _audioSource.clip = explode;
        _audioSource.Play();
    }

    public void PlayLose()
    {
        _audioSource.clip = lose;
        _audioSource.Play();
    }

    public void PlayWin()
    {
        _audioSource.clip = win;
        _audioSource.Play();
    }

    public void PlayPause()
    {
        _audioSource.clip = pause;
        _audioSource.Play();
    }

    public void PlayStart()
    {
        _audioSource.clip = start;
        _audioSource.Play();
    }

    public void PlayQuit()
    {
        _audioSource.clip = quit;
        _audioSource.Play();
    }
}
