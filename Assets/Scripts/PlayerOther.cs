using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOther : MonoBehaviour
{
    public Player _playerMain;
    public Rigidbody2D _rigidbody;

    [SerializeField] private PlayerChecker _left;
    [SerializeField] private PlayerChecker _right;
    [SerializeField] private PlayerChecker _down;
    [SerializeField] private PlayerChecker _up;

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
