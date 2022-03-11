using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckerCrush : MonoBehaviour
{
    private Player _playerMain;

    // Start is called before the first frame update
    void Start()
    {
        _playerMain = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Checker"))
        {
            StartCoroutine(GameManager.Instance.GameOver(_playerMain));
        }
    }
}
