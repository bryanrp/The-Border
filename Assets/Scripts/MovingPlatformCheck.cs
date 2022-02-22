using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformCheck : MonoBehaviour
{
    private MovingPlatform platform;
    [SerializeField] private int from;

    // Start is called before the first frame update
    void Start()
    {
        platform = GetComponentInParent<MovingPlatform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            platform.Attach(collision.GetComponent<Player>(), from);
        }
    }
}
