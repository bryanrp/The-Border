using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float _smoothTime = 0.1f;

    private Vector3 _target;
    private Vector3 _velocity = Vector3.zero;

    private Player[] _players;
    private float _cameraHeight, _cameraWidth;
    private float _additionalAllowedDistance = 1f, _minDistToMoveX = 5f, _minDistToMoveY = 3f;

    // Start is called before the first frame update
    void Start()
    {
        _target = GameManager.Instance.GetCameraPosInLevel();
        
        _players = new Player[2];
        _players[0] = GameManager.Instance.GetPlayer(0);
        _players[1] = GameManager.Instance.GetPlayer(1);
        _cameraHeight = 2 * GetComponent<Camera>().orthographicSize;
        _cameraWidth = _cameraHeight * GetComponent<Camera>().aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsCameraMovable())
        {
            MoveCamera();
        }
        else
        {
            _target = GameManager.Instance.GetCameraPosInLevel();
            _target.z = -10f;
        }

        transform.position = Vector3.SmoothDamp(transform.position, _target, ref _velocity, _smoothTime);
    }

    private void MoveCamera()
    {
        float dx = Mathf.Abs(_players[0].transform.position.x - _players[1].transform.position.x);
        float dy = Mathf.Abs(_players[0].transform.position.y - _players[1].transform.position.y);
        if (dx > _cameraWidth + _additionalAllowedDistance || dy > _cameraHeight + _additionalAllowedDistance)
        {
            StartCoroutine(GameManager.Instance.GameOver(_players[0]));
        }

        float left, right, bottom, top;
        left = transform.position.x - _cameraWidth / 2;
        right = left + _cameraWidth;
        bottom = transform.position.y - _cameraHeight / 2;
        top = bottom + _cameraHeight;

        _target = transform.position;

        // 1. camera must be in the middle of both Player (both Player on the edge)
        // 2. one Player is on the edge, move to the Player that closest to edge
        // 3. no need to move
        if (dx > _cameraWidth - 2 * _minDistToMoveX)
        {
            _target.x = Mathf.Min(_players[0].transform.position.x, _players[1].transform.position.x) + dx / 2;
        }
        else
        {
            float leftDis, rightDis, dxa, dxb, moveTo;

            leftDis = _players[0].transform.position.x - left;
            rightDis = _players[0].transform.position.x - right;
            if (leftDis < -rightDis) dxa = leftDis;
            else dxa = rightDis;

            leftDis = _players[1].transform.position.x - left;
            rightDis = _players[1].transform.position.x - right;
            if (leftDis < -rightDis) dxb = leftDis;
            else dxb = rightDis;

            moveTo = (Mathf.Abs(dxa) < Mathf.Abs(dxb) ? dxa : dxb);
            if (Mathf.Abs(moveTo) < _minDistToMoveX)
            {
                // move the camera so that the player is not close to the edge
                if (moveTo < 0)
                {
                    _target.x += _minDistToMoveX + moveTo;
                }
                else
                {
                    _target.x -= _minDistToMoveX - moveTo;
                }
            }
        }

        if (dy > _cameraHeight - 2 * _minDistToMoveY)
        {
            _target.y = Mathf.Min(_players[0].transform.position.y, _players[1].transform.position.y) + dy / 2;
        }
        else
        {
            float bottomDis, topDis, dya, dyb, moveTo;

            bottomDis = _players[0].transform.position.y - bottom;
            topDis = _players[0].transform.position.y - top;
            if (bottomDis < -topDis) dya = bottomDis;
            else dya = topDis;

            bottomDis = _players[1].transform.position.y - bottom;
            topDis = _players[1].transform.position.y - top;
            if (bottomDis < -topDis) dyb = bottomDis;
            else dyb = topDis;

            moveTo = (Mathf.Abs(dya) < Mathf.Abs(dyb) ? dya : dyb);
            if (Mathf.Abs(moveTo) < _minDistToMoveY)
            {
                // move the camera so that the player is not close to the edge
                if (moveTo < 0)
                {
                    _target.y += _minDistToMoveY + moveTo;
                }
                else
                {
                    _target.y -= _minDistToMoveY - moveTo;
                }
            }
        }
    }

    public IEnumerator ShakeCamera(float duration = 0.1f, float magnitude = 0.1f)
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;
        float elapsed = 0;

        while (elapsed < duration)
        {
            float dx = Random.Range(-magnitude, magnitude);
            float dy = Random.Range(-magnitude, magnitude);
            transform.position = new Vector3(x + dx, y + dy, z);

            yield return null;

            elapsed += Time.deltaTime;
        }

        transform.position = new Vector3(x, y, z);
    }
}
