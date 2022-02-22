using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    protected static GameManager _gameManager;
    protected PhysicsObject _physicsPrimary;
    protected PhysicsObject _physicsSecondary;
    [SerializeField] protected int _physicsType = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        if (_gameManager == null) _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (!IsDuplicate())
        {
            if (_physicsType == 1)
            {
                transform.SetParent(null);
                _gameManager.MoveGameObjectToScene(gameObject, 1);
            }

            _physicsSecondary = Instantiate(gameObject).GetComponent<PhysicsObject>();
            _physicsSecondary.transform.position = transform.position;
            _physicsSecondary.SetDuplicate(this);
            _gameManager.MoveGameObjectToScene(_physicsSecondary.gameObject, 1 - _physicsType);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected bool IsDuplicate()
    {
        return _physicsPrimary != null;
    }

    public void SetDuplicate(PhysicsObject primary)
    {
        _physicsPrimary = primary;
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        if (!IsDuplicate()) _physicsSecondary.SetActive(value);
    }
}
