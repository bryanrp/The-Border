using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMover : PhysicsObject
{
    private static int _requiredNumberOfTrigger = 2;
    private int _numberOfTrigger = 0;

    [SerializeField] private PhysicsObject _wall;
    [SerializeField] private int _inLevel = 0;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDuplicate() && _gameManager.CurrentLevel() == _inLevel)
        {
            bool shouldWallClose = _numberOfTrigger < _requiredNumberOfTrigger;
            if (_wall.gameObject.activeSelf != shouldWallClose)
            {
                _wall.SetActive(shouldWallClose);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddNumberOfTrigger(1);
            if (_gameManager.CurrentLevel() != _inLevel)
            {
                _gameManager.ChangeToLevel(_inLevel);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddNumberOfTrigger(-1);
        }
    }

    /// <summary>
    /// Add delta to the numberOfTrigger of the main LevelMover
    /// </summary>
    /// <param name="delta"></param>
    public void AddNumberOfTrigger(int delta)
    {
        if (!IsDuplicate())
        {
            _numberOfTrigger += delta;
        }
        else
        {
            ((LevelMover)_physicsPrimary).AddNumberOfTrigger(delta);
        }
    }
}
