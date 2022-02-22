using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    protected PhysicsObject _physicsPrimary;
    protected PhysicsObject _physicsSecondary;
    [SerializeField] protected int _physicsType = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        if (!IsDuplicate())
        {
            if (_physicsType == 1)
            {
                transform.SetParent(null);
                PhysicsManager.Instance.MoveGameObjectToScene(gameObject, 1);
            }

            _physicsSecondary = Instantiate(gameObject).GetComponent<PhysicsObject>();
            _physicsSecondary.transform.position = transform.position;
            _physicsSecondary.SetDuplicate(this);
            PhysicsManager.Instance.MoveGameObjectToScene(_physicsSecondary.gameObject, 1 - _physicsType);
        }
        else
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = false;
            }
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
