using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngameUI : MonoBehaviour
{
    public static IngameUI Instance { get; private set; }

    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _doneMenu;
    [SerializeField] private Text _levelText;
    [SerializeField] private Text _timerText;
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
            _levelText.text = "Level " + 1;
        }

        if (_timerText == null)
        {
            Debug.LogWarning("TimerText is not assigned");
        }
        else
        {
            UpdateTimer();
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
        UpdateTimer();
    }

    public void SetGamePause(bool isPause)
    {
        _pauseMenu.SetActive(isPause);
    }

    public void SetGameMap(bool isMap)
    {
        _mapText.SetActive(isMap);
        _levelText.gameObject.SetActive(!isMap);
    }

    public void SetLevelText(int level)
    {
        _levelText.text = "Level " + level;
    }

    private void UpdateTimer()
    {
        float timer = GameManager.Instance.Timer;
        int minute = Mathf.FloorToInt(timer / 60);
        timer -= minute * 60;
        int second = Mathf.FloorToInt(timer);
        timer -= second;
        int milisecond = Mathf.FloorToInt(timer * 100);

        _timerText.text = minute.ToString("D3") + " : " + second.ToString("D2") + " : " + milisecond.ToString("D2");
    }
}
