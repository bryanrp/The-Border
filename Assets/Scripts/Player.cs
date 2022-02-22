using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private static GameManager _gameManager;

    // Type and other player
    private PlayerOther _secondaryPhysics;
    public int Type;
    private bool _active = true;

    // Movement
    private Rigidbody2D _rigidbody;
    private float _jumpForce = 10;
    private float _addSpeedX = 0;
    public bool CanJump = false;
    public bool IsAttached = false;

    // Arrow indicator
    private GameObject _arrow;
    private float _arrowTranslateScale = 0.1f;
    private float _arrowPrevY = 0;

    [SerializeField] private GameObject _playerOtherPrefab;
    [SerializeField] private float _horizontalSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager == null) _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _arrow = transform.Find("Arrow").gameObject;

        if (Type == 1)
        {
            _gameManager.MoveGameObjectToScene(gameObject, Type);
        }
        _secondaryPhysics = Instantiate(_playerOtherPrefab).GetComponent<PlayerOther>();
        _secondaryPhysics._playerMain = this;
        _gameManager.MoveGameObjectToScene(_secondaryPhysics.gameObject, 1 - Type);
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameManager.IsGamePlaying())
        {
            Switch();
            
            _arrow.SetActive(_active);

            if (_active)
            {
                ProcessMove();
                ProcessArrow();

                // The following line is not using MovePosition because _secondaryPhysics._rigidbody is not simulated.
                _secondaryPhysics._rigidbody.position = _rigidbody.position;
            }
            else
            {
                _secondaryPhysics._rigidbody.MovePosition(_rigidbody.position);
            }
        }
    }

    private void ProcessMove()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal") * _horizontalSpeed + _addSpeedX;
        float verticalSpeed = _rigidbody.velocity.y;
        _rigidbody.velocity = new Vector2(horizontalSpeed, verticalSpeed);
        _addSpeedX = 0;

        if (Input.GetKeyDown(KeyCode.Space) && CanJump)
        {
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            CanJump = false;
            _gameManager.PlayJump();
        }
    }

    private void ProcessArrow()
    {
        float y = -Mathf.Abs(Mathf.Sin(2 * Time.time)) * _arrowTranslateScale;
        float dy = y - _arrowPrevY;
        _arrow.transform.Translate(0, dy, 0);
        _arrowPrevY = y;
    }

    public void Switch()
    {
        if ((_gameManager.GetActiveType() == Type) != _active)
        {
            _active = !_active;
            if (_active) Activate();
            else Deactivate();
        }
    }

    private void Activate()
    {
        _secondaryPhysics._rigidbody.simulated = false;
        if (IsAttached)
        {
            SetSpeedY(_rigidbody.velocity.y * 0.1f);
        }
    }

    private void Deactivate()
    {
        _secondaryPhysics._rigidbody.simulated = true;
    }

    /// <summary>
    /// Set gameObjectParent.transform as the parent of this transform.
    /// </summary>
    /// <param name="gameObjectParent"></param>
    public void Attach(GameObject gameObjectParent)
    {
        transform.parent = gameObjectParent.transform;
    }

    /// <summary>
    /// Set this transform to be in global space or no parent.
    /// </summary>
    public void Detach()
    {
        transform.parent = null;
    }

    /// <summary>
    /// Set the rigidbody horizontal or x-velocity.
    /// </summary>
    /// <param name="x"></param>
    public void SetSpeedX(float x)
    {
        _rigidbody.velocity = new Vector2(x, _rigidbody.velocity.y);
    }

    /// <summary>
    /// Set the rigidbody vertical or y-velocity.
    /// </summary>
    /// <param name="y"></param>
    public void SetSpeedY(float y)
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, y);
    }

    /// <summary>
    /// Add the rigidbody horizontal or x-velocity only on the next frame. Used for moving this player on top of a horizontally-moving platform.
    /// </summary>
    /// <param name="x"></param>
    public void AddSpeedX(float x)
    {
        // Debug.Log("Speed added: " + x);
        _addSpeedX = x;
    }

    public bool IsActive()
    {
        return _active;
    }
}
