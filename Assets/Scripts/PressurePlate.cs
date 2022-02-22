using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : PhysicsObject
{
    private SpriteRenderer _spriteRenderer;
    private Sprite _spriteDeactivated;

    private int _numberOfActivatingPlayers = 0;
    private int _numberOfActivatingPlayersPrev = 0;

    [SerializeField] private MovablePlatform _movablePlatform;
    [SerializeField] private Sprite _spriteActivated;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteDeactivated = _spriteRenderer.sprite;
        if (IsDuplicate())
        {
            _spriteRenderer.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDuplicate() && (_gameManager.IsGamePlaying() || _gameManager.IsGameRestart()))
        {
            if ((_numberOfActivatingPlayers == 0) != (_numberOfActivatingPlayersPrev == 0))
            {
                if (_numberOfActivatingPlayers == 0)
                {
                    _movablePlatform.Deactivate();
                    _spriteRenderer.sprite = _spriteDeactivated;
                }
                else
                {
                    _movablePlatform.Activate();
                    _spriteRenderer.sprite = _spriteActivated;
                }
            }
            _numberOfActivatingPlayersPrev = _numberOfActivatingPlayers;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeNumberOfActivatingPlayers(1);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChangeNumberOfActivatingPlayers(-1);
        }
    }

    public void ChangeNumberOfActivatingPlayers(int delta)
    {
        if (!IsDuplicate()) _numberOfActivatingPlayers += delta;
        else ((PressurePlate)_physicsPrimary).ChangeNumberOfActivatingPlayers(delta);
    }

    /// <summary>
    /// Avoid this PressurePlate to duplicate itself by assigning mainPressurePlate to _mainPhysics.
    /// Only call right after object's instantiation.
    /// </summary>
    public void SetDuplicate(PressurePlate mainPressurePlate)
    {
        _physicsPrimary = mainPressurePlate;
    }
}
