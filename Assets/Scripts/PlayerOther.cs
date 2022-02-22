using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOther : MonoBehaviour
{
    public Player _playerMain;
    public Rigidbody2D _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnCollisionStay2D(Collision2D collision)
    {
        int contactCount = collision.contactCount;
        ContactPoint2D[] contactPoints = new ContactPoint2D[contactCount];
        collision.GetContacts(contactPoints);
        Vector2 impulse = Vector2.zero;
        for (int i = 0; i < contactCount; i++)
        {
            impulse += contactPoints[i].normal * contactPoints[i].normalImpulse;
        }
        impulse /= contactCount;

        _playerMain.AddForce(impulse, ForceMode2D.Impulse);

        Debug.Log("Added impulse: " + impulse);
        
        /*
        normal = normal.normalized;
        Vector2 contact = -normal;

        float contactAngle = Vector2.Angle(contact, _rigidbody.velocity) * Mathf.Deg2Rad;
        Vector2 normalVelocity = _rigidbody.velocity * Mathf.Cos(contactAngle);

        _rigidbody.velocity = _rigidbody.velocity - normalVelocity;

        Debug.Log("Contact count: " + contactCount);
        Debug.Log("Normal vector: " + normal);
        Debug.Log("Velocity: " + _rigidbody.velocity);
    }
    */
}
