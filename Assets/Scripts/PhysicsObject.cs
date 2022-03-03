using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsObject : MonoBehaviour
{
    protected PhysicsObject _physicsPrimary;
    protected PhysicsObject _physicsSecondary;
    [SerializeField] protected bool _duplicateAtStart = true;
    [SerializeField] protected int _physicsType = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        if (!_duplicateAtStart) return;

        if (!IsDuplicate())
        {
            if (_physicsType == 1)
            {
                transform.SetParent(null);
                PhysicsManager.Instance.MoveGameObjectToScene(gameObject, 1);
            }
            
            SetPrimary(Instantiate(gameObject).GetComponent<PhysicsObject>());
            _physicsSecondary.transform.position = transform.position;
            _physicsSecondary.SetSecondary(this);
            PhysicsManager.Instance.MoveGameObjectToScene(_physicsSecondary.gameObject, 1 - _physicsType);

            ConnectChildWithPhysicsObject();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ConnectChildWithPhysicsObject()
    {
        List<PhysicsObject> primaryChild = new List<PhysicsObject>();
        foreach (Transform child in transform) {
            PhysicsObject childPhysicsObject = child.GetComponent<PhysicsObject>();
            if (childPhysicsObject != null) primaryChild.Add(childPhysicsObject);
        }

        List<PhysicsObject> secondaryChild = new List<PhysicsObject>();
        foreach (Transform child in _physicsSecondary.transform)
        {
            PhysicsObject childPhysicsObject = child.GetComponent<PhysicsObject>();
            if (childPhysicsObject != null) secondaryChild.Add(childPhysicsObject);
        }

        if (primaryChild.Count != secondaryChild.Count) Debug.LogError("primaryChild.Count differs from secondaryChild.Count: " + primaryChild.Count + " vs " + secondaryChild.Count);
        for (int i = 0; i < primaryChild.Count; i++)
        {
            bool found = false;
            for (int j = 0; j < secondaryChild.Count; j++)
            {
                if (primaryChild[i].name == secondaryChild[j].name)
                {
                    primaryChild[i].SetPrimary(secondaryChild[j]);
                    secondaryChild[j].SetSecondary(primaryChild[i]);
                    found = true;
                    break;
                }
            }
            if (!found) Debug.LogError("Child with name: " + primaryChild[i].name + " does not have a pair");
        }
    }

    protected bool IsDuplicate()
    {
        return _physicsPrimary != null;
    }

    public void SetSecondary(PhysicsObject primary)
    {
        if (_physicsPrimary != null) Debug.LogWarning("Reassigning physicsObject to a PhysicsObject that already have a _physicsPrimary");
        _physicsPrimary = primary;

        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
    }

    public void SetPrimary(PhysicsObject secondary)
    {
        if (_physicsSecondary != null) Debug.LogWarning("Reassigning physicsObject to a PhysicsObject that already have a _physicsSecondary");
        _physicsSecondary = secondary;
        
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }
    }

    public void SetActive(bool value)
    {
        gameObject.SetActive(value);
        if (!IsDuplicate()) _physicsSecondary.SetActive(value);
    }
}
