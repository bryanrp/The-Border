using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    private Player _player;
    private int _numberOfTrigger = 0;

    // Start is called before the first frame update
    void Start()
    {
        _player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Checker"))
        {
            if (collision.CompareTag("MovablePlatform"))
            {
                collision.GetComponentInParent<MovablePlatform>().AttachPlayer(_player);
                _player.IsAttached = true;
            }
            
            _numberOfTrigger++;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Checker"))
        {            
            if (collision.CompareTag("MovablePlatform"))
            {
                collision.GetComponentInParent<MovablePlatform>().AttachPlayer(null);
                _player.IsAttached = false;
            }
            
            _numberOfTrigger--;
        }
    }

    public bool CanPlayerJump()
    {
        return (_numberOfTrigger > 0);
    }
}
