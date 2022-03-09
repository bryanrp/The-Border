using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : PhysicsObject
{
    protected SpriteRenderer _spriteRenderer;
    private Sprite _spriteDeactivated;

    protected int _numberOfActivatingPlayers = 0;
    protected int _numberOfActivatingPlayersPrev = 0;

    [SerializeField] protected MovablePlatform _movablePlatform;
    [SerializeField] private Sprite _spriteActivated;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteDeactivated = _spriteRenderer.sprite;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (!IsDuplicate() && (GameManager.Instance.IsGamePlaying() || GameManager.Instance.IsGameRestart()))
        {
            if ((_numberOfActivatingPlayers == 0) != (_numberOfActivatingPlayersPrev == 0))
            {
                if (_numberOfActivatingPlayers == 0)
                {
                    _movablePlatform.Deactivate();
                    Deactivate();
                }
                else
                {
                    _movablePlatform.Activate();
                    Activate();
                }
            }
            _numberOfActivatingPlayersPrev = _numberOfActivatingPlayers;
        }
    }

    public void AddNumberOfActivatingPlayers(int delta)
    {
        if (!IsDuplicate()) _numberOfActivatingPlayers += delta;
        else ((PressurePlate)_physicsPrimary).AddNumberOfActivatingPlayers(delta);
    }

    protected virtual void Activate()
    {
        _spriteRenderer.sprite = _spriteActivated;
    }

    protected virtual void Deactivate()
    {
        _spriteRenderer.sprite = _spriteDeactivated;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddNumberOfActivatingPlayers(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AddNumberOfActivatingPlayers(-1);
        }
    }
}
