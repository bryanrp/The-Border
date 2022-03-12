using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChecker : MonoBehaviour
{
    private Player _playerMain;
    private float _resetSpeed = 0;
    private bool _reset = false;

    [SerializeField] private Direction _direction;

    // Start is called before the first frame update
    void Start()
    {
        _playerMain = GetComponentInParent<PlayerOther>()._playerMain;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameRestart())
        {
            _reset = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MovablePlatform"))
        {
            Rigidbody2D collisionRigidbody = collision.GetComponentInParent<Rigidbody2D>();

            if (_direction == Direction.Right)
            {
                float x = collisionRigidbody.velocity.x;
                x = Mathf.Min(0, x);
                if (x != 0)
                {
                    _resetSpeed = x;
                    _reset = true;
                }
            }
            else if (_direction == Direction.Up)
            {
                float y = collisionRigidbody.velocity.y;
                y = Mathf.Min(0, y);
                if (y != 0)
                {
                    _resetSpeed = y;
                    _reset = true;
                }
            }
            else if (_direction == Direction.Left)
            {
                float x = collisionRigidbody.velocity.x;
                x = Mathf.Max(0, x);
                if (x != 0)
                {
                    _resetSpeed = x;
                    _reset = true;
                }
            }
            else if (_direction == Direction.Down)
            {
                float y = collisionRigidbody.velocity.y;
                y = Mathf.Min(0, y);
                if (y != 0)
                {
                    _resetSpeed = y;
                    _reset = true;
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_playerMain.IsActive() && collision.CompareTag("MovablePlatform"))
        {
            if (_reset)
            {
                if (_direction == Direction.Left || _direction == Direction.Right) _playerMain.SetSpeedX(_resetSpeed);
                else _playerMain.SetSpeedY(_resetSpeed);
                _resetSpeed = 0;
                _reset = false;
            }
        }
    }

    enum Direction
    {
        Right,
        Up,
        Left,
        Down
    };
}
