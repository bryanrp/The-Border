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
    [SerializeField] private Text _mapText;

    [SerializeField] private Text _doneTimerText;
    [SerializeField] private Text _doneDeathText;
    [SerializeField] private Text _doneRestartText;

    [SerializeField] private GameObject _skipLevel;

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
            _levelText.text = "Lv " + (GameManager.Instance.CurrentLevel() + 1) + " / " + GameManager.Instance.TotalLevels();
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
            _mapText.gameObject.SetActive(false);
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
        _mapText.gameObject.SetActive(isMap);
        _levelText.gameObject.SetActive(!isMap);
    }

    public void SetLevelText(int level, int totalLevel)
    {
        _levelText.text = "Lv " + level + " / " + totalLevel;
    }

    public void SetGameDone()
    {
        _levelText.gameObject.SetActive(false);
        _timerText.gameObject.SetActive(false);
        _doneMenu.SetActive(true);
        _doneTimerText.text = _timerText.text;
        _doneDeathText.text = GameManager.Instance.DeathCounter.ToString("D2");
        _doneRestartText.text = (GameManager.Instance.RestartCounter - GameManager.Instance.DeathCounter).ToString("D2");
    }

    public void SetSkipLevel(bool isShow)
    {
        _skipLevel.SetActive(isShow);
    }

    private void UpdateTimer()
    {
        float timer = GameManager.Instance.Timer;
        int minute = Mathf.FloorToInt(timer / 60);
        timer -= minute * 60;
        int second = Mathf.FloorToInt(timer);
        timer -= second;
        int milisecond = Mathf.FloorToInt(timer * 100);

        _timerText.text = minute.ToString("D2") + " : " + second.ToString("D2") + " : " + milisecond.ToString("D2");
    }
}
