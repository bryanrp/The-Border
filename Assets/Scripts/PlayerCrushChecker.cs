using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrushChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") && !collision.CompareTag("Checker"))
        {
            Player player = gameObject.GetComponentInParent<Player>();
            if (player == null) throw new System.InvalidOperationException("Parent GameObject does not have Player script attached");
            StartCoroutine(GameManager.Instance.GameOver(player));
        }
    }
}
