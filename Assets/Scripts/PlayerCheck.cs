using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    private Player _playerMain;
    private float _resetSpeed = 0;
    private bool _reset = false;

    [SerializeField] private int _type;

    // Start is called before the first frame update
    void Start()
    {
        _playerMain = GetComponentInParent<PlayerOther>()._playerMain;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGameOver())
        {
            _reset = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MovablePlatform"))
        {
            Rigidbody2D collisionRigidbody = collision.GetComponentInParent<Rigidbody2D>();

            if (_type < 2)
            {
                // horizontal
                float x = collisionRigidbody.velocity.x;
                if (_type == 0) x = Mathf.Min(0, x);
                else x = Mathf.Max(0, x);
                if (x != 0)
                {
                    _playerMain.SetSpeedX(x * 10f);
                    _resetSpeed = x;
                    _reset = true;
                }
            }
            else
            {
                // up
                float y = collisionRigidbody.velocity.y;
                if (_type == 2) y = Mathf.Min(0, y);
                else y = Mathf.Max(0, y);
                if (y != 0)
                {
                    _playerMain.SetSpeedY(y * 10f);
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
                if (_type < 2) _playerMain.SetSpeedX(_resetSpeed);
                else _playerMain.SetSpeedY(_resetSpeed);
                _resetSpeed = 0;
                _reset = false;
            }
        }
    }
}
