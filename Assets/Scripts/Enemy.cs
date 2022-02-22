using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PhysicsObject
{
    private const float _particlePerArea = 10;

    private ParticleSystem _particleSystem;

    [SerializeField] private int _type;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        _particleSystem = GetComponent<ParticleSystem>();
        if (_particleSystem != null) ChangeParticlePerSecond();
    }

    // Update is called once per frame
    void Update()
    {
        if (_particleSystem != null && _particleSystem.isPaused != _gameManager.IsGamePause())
        {
            if (_gameManager.IsGamePause()) _particleSystem.Pause();
            else _particleSystem.Play();
        }
    }

    private void ChangeParticlePerSecond()
    {
        float area = _particleSystem.shape.scale.x * _particleSystem.shape.scale.y;
        var emission = _particleSystem.emission;
        emission.rateOverTime = area * _particlePerArea;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (collision.GetComponent<Player>().Type != _type))
        {
            StartCoroutine(_gameManager.GameOver(collision.GetComponent<Player>()));
        }
    }
}
