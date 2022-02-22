using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private static MusicManager _musicManager;

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
            _musicManager.GetComponent<AudioSource>().Play();
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
    }
}
