using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private const float _smoothTime = 0.1f;

    private static GameManager _gameManager;

    private Vector3 _target;
    private Vector2 _velocity = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        if (_gameManager == null) _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        _target = _gameManager.GetCameraPosInLevel();
    }

    // Update is called once per frame
    void Update()
    {
        _target = _gameManager.GetCameraPosInLevel();
        transform.position = Vector2.SmoothDamp(transform.position, _target, ref _velocity, _smoothTime);
        transform.Translate(Vector3.back * 10);
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
