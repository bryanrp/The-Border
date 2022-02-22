using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private static GameManager gameManager;
    private Light lightDone;
    [SerializeField] private int targetType;

    // Start is called before the first frame update
    void Start()
    {
        if (gameManager == null) gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        lightDone = transform.Find("Light").GetComponent<Light>();
        lightDone.intensity = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            if (!gameManager.Cutscene && collision.GetComponent<Player>().Type == targetType)
            {
                gameManager.UpdateNumberOfDone(1);
                lightDone.intensity = 1;
                gameManager.PlayTargetIn();
            }
            else if (gameManager.Cutscene && collision.GetComponent<Player>().Type != targetType)
            {
                gameManager.UpdateNumberOfDone(1);
                gameManager.PlayTargetSwitch();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!gameManager.Cutscene && collision.GetComponent<Player>().Type == targetType)
            {
                gameManager.UpdateNumberOfDone(-1);
                lightDone.intensity = 0;
                gameManager.PlayTargetOut();
            }
            else if (gameManager.Cutscene && collision.GetComponent<Player>().Type != targetType)
            {
                gameManager.UpdateNumberOfDone(-1);
                gameManager.PlayTargetSwitch();
            }
        }
    }
    */
}
