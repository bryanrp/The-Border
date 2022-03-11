using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : PhysicsObject
{
    /*
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
        passive to active - active: check (when switch player, reduce the newly activated player's speed) [DONE, but Player0 sometimes keeps its velocity for a frame]
        passive to active - passive: dynamic [DONE]
    */

    private Rigidbody2D _rigidbody;

    private Vector2 _startPosition;
    private Vector2 _targetPosition;
    private bool _isActive = false;
    private bool _isMoving = false;
    private int _numberOfActivate = 0;

    private Vector2 _prevPos;
    private Player _attachedPlayer;
    private Player _resetPrevPlayer;

    [SerializeField] private Vector2 _moveVector;
    [SerializeField] private float _moveToSpeed;
    [SerializeField] private float _moveBackSpeed;

    [SerializeField] private List<SpriteRenderer> _arrowSpriteRenderers;
    [SerializeField] private Material _glowWhite;
    [SerializeField] private Material _spriteLitDefault;

    [Tooltip("Leave size to 0 to draw line on all platforms")]
    [SerializeField] private List<Transform> _drawLineOnSpecificPlatforms;
    private static Color _lineColor = Color.white;
    private static float _lineWidth = 0.03f;
    private static float _lineZPos = 0.01f;

    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        _rigidbody = GetComponent<Rigidbody2D>();

        _startPosition = transform.position;
        _targetPosition = _startPosition + _moveVector;

        _prevPos = transform.position;

        if (!IsDuplicate())
        {
            DrawLineOnPlatform();
        }
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
                    _attachedPlayer.SetSpeedY(0);
                    _resetPrevPlayer = _attachedPlayer;
                }
            }

            if (_attachedPlayer != null)
            {
                float dx = transform.position.x - _prevPos.x;
                _attachedPlayer.transform.Translate(dx, 0, 0);
            }

            _prevPos = transform.position;

            _physicsSecondary.GetComponent<Rigidbody2D>().MovePosition(_rigidbody.position);
        }
    }

    /// <summary>
    /// Move the platform to target position.
    /// </summary>
    public void Activate()
    {
        if (_numberOfActivate++ == 0)
        {
            _isActive = true;
            _isMoving = true;
            _rigidbody.velocity = _moveVector.normalized * _moveToSpeed;

            if (_attachedPlayer != null && _rigidbody.velocity.y < 0)
            {
                _attachedPlayer.SetSpeedY(0);
                StartCoroutine(ResetPlayerAtNextFixedUpdate(_attachedPlayer));
            }

            SetArrowMaterial(true);
        }
    }

    /// <summary>
    /// Move the platform to start position.
    /// </summary>
    public void Deactivate()
    {
        if (--_numberOfActivate == 0)
        {
            _isActive = false;
            _isMoving = true;
            _rigidbody.velocity = -_moveVector.normalized * _moveBackSpeed;

            if (_attachedPlayer != null && _rigidbody.velocity.y < 0)
            {
                _attachedPlayer.SetSpeedY(0);
                StartCoroutine(ResetPlayerAtNextFixedUpdate(_attachedPlayer));
            }

            SetArrowMaterial(false);
        }
    }

    public void AttachPlayer(Player player)
    {
        if (IsDuplicate()) ((MovablePlatform)_physicsPrimary).AttachPlayer(player);
        else _attachedPlayer = player;
    }

    private IEnumerator ResetPlayerAtNextFixedUpdate(Player player)
    {
        yield return new WaitForFixedUpdate();
        _resetPrevPlayer = player;
    }

    private void SetArrowMaterial(bool isGlow)
    {
        foreach (SpriteRenderer spriteRenderer in _arrowSpriteRenderers)
        {
            spriteRenderer.material = (isGlow ? _glowWhite : _spriteLitDefault);
        }
    }

    private void DrawLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject lineObject = new GameObject();
        lineObject.transform.position = new Vector3(startPos.x, startPos.y, 0.01f);
        lineObject.AddComponent<LineRenderer>();
        LineRenderer line = lineObject.GetComponent<LineRenderer>();
        line.material = _spriteLitDefault;
        line.startColor = _lineColor;
        line.endColor = _lineColor;
        line.startWidth = _lineWidth;
        line.endWidth = _lineWidth;
        line.SetPosition(0, startPos);
        line.SetPosition(1, endPos);
    }

    private void DrawLineOnPlatform()
    {
        if (_drawLineOnSpecificPlatforms.Count == 0)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("MovablePlatform"))
                {
                    Vector3 startPos = child.position;
                    Vector3 endPos = startPos + (Vector3)_moveVector;
                    startPos.z = endPos.z = _lineZPos;
                    DrawLine(startPos, endPos);
                }
            }
        }
        else
        {
            foreach (Transform platform in _drawLineOnSpecificPlatforms)
            {
                Vector3 startPos = platform.position;
                Vector3 endPos = startPos + (Vector3)_moveVector;
                startPos.z = endPos.z = _lineZPos;
                DrawLine(startPos, endPos);
            }
        }
    }
}
