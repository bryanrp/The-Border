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
        _rigidbody.simulated = !_playerMain.IsActive();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
