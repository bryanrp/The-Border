using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : PhysicsObject
{
    /*
        when active, disable dynamic rigidbody
        let PlayerCheck and GroundCheck and PlayerInput control the movement
        when passive, enable dynamic rigidbody (PROBLEM, player will fly off the screen because of the MovablePlaform's speed)
        
        disable dynamic rigidbody altogether
        active works well
        passive works? probably. lets try
        
        PROBLEM:
        1) occured when MovablePlatform and Player is on different PhysicsScene
        2) occured in condition (1) when 
        ACTIVE:
        horizontal: needs to use dynamic rigidbody
        below: needs to use dynamic rigidbody
        up: jump and movableplatform. movableplatform has to tell player when its going to stop
        PASSIVE:
        horizontal: needs to use dynamic
        below: needs to use dynamic
        up: movableplatform
        
        CASES:
        Horizontal + Down: [DONE]
        active - active: dynamic v
        active - passive: dynamic v 
        passive - active: dynamic - check v (when MovablePlatform stops, reduce the speed)
        passive - passive: dynamic v
        active to passive - active: dynamic v
        active to passive - passive: dynamic v
        passive to active - active: dynamic v (cause when active, horizontal speed is controlled by input)
        passive to active - passive: dynamic v
        Up:
        active - active: dynamic - stop (when MovablePlatform stops, stop the up speed) [DONE]
        active - passive: dynamic - stop (---) [DONE]
        passive - active: dynamic - stop (---) [DONE] => check when MovablePlatform stop going up (stop moving or deactivated)
        passive - passive: dynamic - stop (---) [DONE]
        active to passive - active: dynamic [DONE]
        active to passive - passive: dynamic [DONE]
        passive to active - active: check (when switch player, reduce the newly activated player's speed)
        passive to active - passive: dynamic [DONE]
    */

    private Rigidbody2D _rigidbody;

    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private bool _isActive = false;
    private bool _isMoving = false;

    private Vector2 _prevPos;
    private Player _attachedPlayer;
    private Player _resetPrevPlayer;

    [SerializeField] private Vector2 _moveVector;
    [SerializeField] private float _moveToSpeed;
    [SerializeField] private float _moveBackSpeed;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();

        _startPosition = transform.position;
        _targetPosition = _startPosition + _moveVector;

        _prevPos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!IsDuplicate())
        {
            if (_resetPrevPlayer != null)
            {
                _resetPrevPlayer.SetSpeedY(0);
                _resetPrevPlayer = null;
            }

            bool debug = false;
            if (_isMoving)
            {

                if (_isActive)
                {
                    float distance = ((Vector2)transform.position - _startPosition).magnitude;
                    if (distance >= _moveVector.magnitude)
                    {
                        _isMoving = false;
                        _rigidbody.velocity = Vector2.zero;
                        _rigidbody.MovePosition(_targetPosition);
                    }
                }
                else
                {
                    float distance = ((Vector2)transform.position - _targetPosition).magnitude;
                    if (distance >= _moveVector.magnitude)
                    {
                        _isMoving = false;
                        _rigidbody.velocity = Vector2.zero;
                        _rigidbody.MovePosition(_startPosition);
                    }
                }

                if (!_isMoving && _moveVector.y != 0 && _attachedPlayer != null)
                {
                    Debug.Log("WOW");
                    _attachedPlayer.SetSpeedY(0);
                    PhysicsManager.QueuePlayerToSetSpeedY(_attachedPlayer);
                    _resetPrevPlayer = _attachedPlayer;
                    debug = true;
                }
            }

            if (_attachedPlayer != null)
            {
                float dx = transform.position.x - _prevPos.x;
                _attachedPlayer.transform.Translate(dx, 0, 0);
            }

            _prevPos = transform.position;

            _physicsSecondary.GetComponent<Rigidbody2D>().MovePosition(_rigidbody.position);
            if (false && debug) Debug.Break();
        }
    }

    /// <summary>
    /// Move the platform to target position.
    /// </summary>
    public void Activate()
    {
        _isActive = true;
        _isMoving = true;
        _rigidbody.velocity = _moveVector.normalized * _moveToSpeed;

        if (_attachedPlayer != null && _rigidbody.velocity.y < 0)
        {
            Debug.Log("HAH");
            _attachedPlayer.SetSpeedY(0);
            PhysicsManager.QueuePlayerToSetSpeedY(_attachedPlayer);
            StartCoroutine(ResetPlayerAtNextFixedUpdate(_attachedPlayer));
        }
    }

    /// <summary>
    /// Move the platform to start position.
    /// </summary>
    public void Deactivate()
    {
        _isActive = false;
        _isMoving = true;
        _rigidbody.velocity = -_moveVector.normalized * _moveBackSpeed;

        if (_attachedPlayer != null && _rigidbody.velocity.y < 0)
        {
            Debug.Log("HAH");
            _attachedPlayer.SetSpeedY(0);
            PhysicsManager.QueuePlayerToSetSpeedY(_attachedPlayer);
            StartCoroutine(ResetPlayerAtNextFixedUpdate(_attachedPlayer));
        }
    }

    IEnumerator ResetPlayerAtNextFixedUpdate(Player player)
    {
        yield return new WaitForFixedUpdate();
        _resetPrevPlayer = player;
    }

    [System.Obsolete("No need to use currently")]
    public Vector2 GetSpeed()
    {
        if (!IsDuplicate()) return _physicsPrimary.GetComponent<MovablePlatform>().GetSpeed();
        else if (_isMoving)
        {
            float physicsSimulationTimeScale = (_physicsType == _gameManager.GetActiveType() ? 0.1f : 1f);
            if (_isActive)
            {
                Debug.Log("Returned X speed: " + (_targetPosition.normalized.x * _moveToSpeed) + "; duplicate: " + IsDuplicate());
                return _targetPosition.normalized * _moveToSpeed * physicsSimulationTimeScale;
            }
            else
            {
                Debug.Log("Returned X speed: " + (-_targetPosition.normalized.x * _moveToSpeed) + "; duplicate: " + IsDuplicate());
                return -_targetPosition.normalized * _moveBackSpeed * physicsSimulationTimeScale;
            }
        }
        else return Vector2.zero;
    }

    public void AttachPlayer(Player player)
    {
        if (IsDuplicate()) ((MovablePlatform)_physicsPrimary).AttachPlayer(player);
        else _attachedPlayer = player;
    }
}
