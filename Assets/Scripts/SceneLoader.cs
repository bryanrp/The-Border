using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const float _animationTime = 1.0f;

    public static SceneLoader Instance { get; private set; }
    private Animator _animator;

    private void Awake()
    {
        Instance = this;
        _animator = GameObject.Find("Fade").GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetActiveScene()
    {
        return SceneManager.GetActiveScene().buildIndex;
    }

    public void RestartScene()
    {
        StartCoroutine(LoadScene(GetActiveScene()));
    }

    public void GoToScene(int sceneIndex)
    {
        StartCoroutine(LoadScene(sceneIndex));
    }

    public void GoToNextScene()
    {
        StartCoroutine(LoadScene((GetActiveScene() + 1) % SceneManager.sceneCountInBuildSettings));
    }

    public IEnumerator RunAnimation(float time = 1.0f)
    {
        _animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(time);

        _animator.SetTrigger("FadeOut");
    }

    IEnumerator LoadScene(int sceneIndex)
    {
        _animator.SetTrigger("FadeIn");

        yield return new WaitForSeconds(_animationTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
