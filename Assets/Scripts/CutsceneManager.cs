using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    private GameManager gameManager;
    private CameraManager cameraManager;

    public GameObject top;
    public GameObject[] bottom;
    public GameObject border;

    public GameObject platform;
    public GameObject particle;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        cameraManager = GameObject.Find("Main Camera").GetComponent<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartSequence()
    {
        StartCoroutine(StartTop());
    }

    IEnumerator StartTop()
    {
        yield return new WaitForSeconds(1);

        top.SetActive(true);

        yield return new WaitForSeconds(3);

        StartCoroutine(StartBottom());
    }

    IEnumerator StartBottom()
    {
        for (int i=0; i<bottom.Length; i++)
        {
            bottom[i].SetActive(true);

            yield return new WaitForSeconds(3); // read

            bottom[i].GetComponent<Animator>().SetTrigger("FlyOut");
            if (i + 1 == bottom.Length) top.GetComponent<Animator>().SetTrigger("FlyOut");

            yield return new WaitForSeconds(2); // transition
        }

        StartCoroutine(StartBorder());
    }

    IEnumerator StartBorder()
    {
        border.SetActive(true);
        yield return new WaitForSeconds(2);

        StartCoroutine(ShakePlatform());
        yield return new WaitForSeconds(1);

        particle.GetComponent<ParticleSystem>().Play();
        StartCoroutine(cameraManager.ShakeCamera());
        platform.SetActive(false);
        // gameManager.PlayExplode();

        yield return new WaitForSeconds(1);
        // gameManager.CutsceneDone();
    }

    IEnumerator ShakePlatform()
    {
        float x = platform.transform.position.x;
        float y = platform.transform.position.y;
        float duration = 1;
        float magnitude = 0.1f;
        float elapsed = 0;
        while (elapsed < duration)
        {
            float dx = Random.Range(-magnitude, magnitude);
            float dy = Random.Range(-magnitude, magnitude);

            yield return null;

            elapsed += Time.deltaTime;
            platform.transform.position = new Vector3(x + dx, y + dy, 0);
        }
    }
}
