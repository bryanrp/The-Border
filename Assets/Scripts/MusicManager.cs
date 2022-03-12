using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _musicManager;
    private const float _musicStartLoop = 0.527f;
    private const float _musicEndLoop = 137.450f;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (_musicManager != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _musicManager = this;
            DontDestroyOnLoad(gameObject);
            _audioSource = GetComponent<AudioSource>();
            _audioSource.Play();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(0, 0, -10);
        if (_audioSource.time > _musicEndLoop)
        {
            _audioSource.time = _musicStartLoop;
        }
    }
}
