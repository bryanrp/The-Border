using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Type and other player
    private PlayerOther _playerOther;
    public int Type;
    private bool _active = true;

    // Movement
    private Rigidbody2D _rigidbody;
    private GroundCheck _groundCheck;
    private const float _jumpForce = 10;
    public bool IsAttached = false;

    // Arrow indicator
    [SerializeField] private GameObject _arrow;
    private float _arrowTranslateScale = 0.1f;
    private float _arrowPrevY = 0;

    [SerializeField] private GameObject _playerOtherPrefab;
    [SerializeField] private float _horizontalSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponentInChildren<GroundCheck>();

        if (Type == 1)
        {
            PhysicsManager.Instance.MoveGameObjectToScene(gameObject, Type);
        }
        _playerOther = Instantiate(_playerOtherPrefab).GetComponent<PlayerOther>();
        _playerOther._playerMain = this;
        PhysicsManager.Instance.MoveGameObjectToScene(_playerOther.gameObject, 1 - Type);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGamePlaying())
        {
            Switch();
            
            _arrow.SetActive(_active);

            if (_active)
            {
                ProcessMove();
                ProcessArrow();

                // The following line is not using MovePosition because _secondaryPhysics._rigidbody is not simulated.
                _playerOther._rigidbody.position = _rigidbody.position;
            }
            else
            {
                _playerOther._rigidbody.MovePosition(_rigidbody.position);
            }
        }
    }

    private void ProcessMove()
    {
        float horizontalSpeed = Input.GetAxis("Horizontal") * _horizontalSpeed;
        float verticalSpeed = _rigidbody.velocity.y;
        _rigidbody.velocity = new Vector2(horizontalSpeed, verticalSpeed);

        if (Input.GetKeyDown(KeyCode.Space) && _groundCheck.CanPlayerJump())
        {
            _rigidbody.AddForce(new Vector2(0, _jumpForce), ForceMode2D.Impulse);
            GameManager.Instance.PlayJump();
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
        if ((GameManager.Instance.GetActiveType() == Type) != _active)
        {
            _active = !_active;
            if (_active) Activate();
            else Deactivate();
        }
    }

    private void Activate()
    {
        _playerOther._rigidbody.simulated = false;
        if (IsAttached)
        {
            SetSpeedY(_rigidbody.velocity.y * 0.1f);
        }
    }

    private void Deactivate()
    {
        _playerOther._rigidbody.simulated = true;
    }

    public void PlayDeathParticle()
    {
        GetComponentInChildren<ParticleSystem>().Play();
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

    public bool IsActive()
    {
        return _active;
    }
}
