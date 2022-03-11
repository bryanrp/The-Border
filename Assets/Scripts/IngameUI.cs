using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    public static IngameUI Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _doneMenu;
    [SerializeField] private GameObject _levelText;
    [SerializeField] private GameObject _mapText;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        if (_pauseMenu == null)
        {
            Debug.LogWarning("PauseMenu is not assigned");
        }
        else
        {
            _pauseMenu.SetActive(GameManager.Instance.IsGamePause());
        }

        if (_doneMenu == null)
        {
            // currently not needed
        }
        else
        {
            // currently not needed
            _doneMenu.SetActive(false);
        }

        if (_levelText == null)
        {
            Debug.LogWarning("LevelText is not assigned");
        }
        else
        {
            _levelText.GetComponent<Text>().text = "Level " + 1;
        }

        if (_mapText == null)
        {
            // currently not needed
        }
        else
        {
            // currently not needed
            _mapText.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetGamePause(bool isPause)
    {
        _pauseMenu.SetActive(isPause);
    }

    public void SetGameMap(bool isMap)
    {
        _mapText.SetActive(isMap);
        _levelText.SetActive(!isMap);
    }

    public void SetLevelText(int level)
    {
        _levelText.GetComponent<Text>().text = "Level " + level;
    }
}
