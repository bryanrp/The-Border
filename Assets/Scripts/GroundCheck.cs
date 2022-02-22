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
        _player.CanJump = (_numberOfTrigger > 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Checker"))
        {
            /*
            if (collision.CompareTag("MovablePlatform"))
            {
                collision.GetComponentInParent<MovablePlatform>().AttachPlayer(_player);
            }
            else
            {
                _player.Attach(collision.gameObject);
            }
            */

            
            if (collision.CompareTag("Moving Platform"))
            {
                // _player.Attach(collision.gameObject);
            }
            else if (collision.CompareTag("MovablePlatform"))
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
            /*
            if (collision.CompareTag("MovablePlatform"))
            {
                collision.GetComponentInParent<MovablePlatform>().AttachPlayer(null);
            }
            else
            {
                _player.Detach();
            }
            */

            
            if (collision.CompareTag("Moving Platform"))
            {
                // _player.Detach();
            }
            else if (collision.CompareTag("MovablePlatform"))
            {
                collision.GetComponentInParent<MovablePlatform>().AttachPlayer(null);
                _player.Switch();
                _player.IsAttached = false;
            }
            
            _numberOfTrigger--;
        }
    }

    /*
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MovablePlatform"))
        {
            _player.AddSpeedX(collision.GetComponentInParent<MovablePlatform>().GetSpeed().x);
        }
    }
    */
}
