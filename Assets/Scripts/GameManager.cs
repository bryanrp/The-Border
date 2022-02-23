using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private enum GameState
    {
        Play,
        Pause,
        Map,
        Restart,
        Over
    }

    private GameState _gameState;

    public static GameManager Instance { get; private set; }

    private static SceneLoader _sceneLoader;
    private static SFXManager _sfxManager;
    private static CameraManager _cameraManager;
    private static ChapterManager _chapterManager;

    public Player[] _players;
    private int _activeType = 0;
    private int _numberOfFinished = 0;

    private GameObject _pauseMenu;
    private GameObject _doneMenu;
    private GameObject _levelText;
    private GameObject _mapText;

    private float _playerDeadAnimationTime = 0.5f;

    private CutsceneManager _cutsceneManager;
    public bool Cutscene = false;

    private int _prevLevel = -1;
    private int _currentLevel = 0;
    [SerializeField] private int _startLevel = 0;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // QualitySettings.vSyncCount = 0;
        // Application.targetFrameRate = 30;
        _gameState = GameState.Play;

        AwakePhysicsManager();

        _sceneLoader = GetComponent<SceneLoader>();
        _sfxManager = GetComponent<SFXManager>();
        _cameraManager = Camera.main.GetComponent<CameraManager>();
        _chapterManager = GetComponent<ChapterManager>();

        Player playerA = GameObject.Find("PlayerA").GetComponent<Player>();
        Player playerB = GameObject.Find("PlayerB").GetComponent<Player>();
        _players = new Player[] { playerA, playerB };

        _pauseMenu = GameObject.Find("Pause Menu");
        _doneMenu = GameObject.Find("Done Menu");
        _levelText = GameObject.Find("Level Text");
        _mapText = GameObject.Find("Map Text");
        if (_pauseMenu != null) _pauseMenu.SetActive(false);
        if (_doneMenu != null) _doneMenu.SetActive(false);
        if (_levelText != null) _levelText.GetComponent<Text>().text = "Level " + _sceneLoader.GetActiveScene();
        if (_mapText != null) _mapText.SetActive(false);

        if (Cutscene) _cutsceneManager = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();

        _currentLevel = _startLevel;
    }

    private void AwakePhysicsManager()
    {
        GetComponent<PhysicsManager>().GameManagerAwake();
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameState == GameState.Play && _numberOfFinished >= 2) GameDone();
        if (_pauseMenu != null && _gameState == GameState.Play && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Cutscene) ToggleGamePause();
            else PlayPause();
        }
        if (_mapText != null && _gameState == GameState.Play && !Cutscene && Input.GetKeyDown(KeyCode.LeftShift)) ToggleGameMap();
        if (_gameState == GameState.Play && Input.GetKeyDown(KeyCode.S)) SwitchPlayer();
        if (_mapText != null && _gameState != GameState.Over && Input.GetKeyDown(KeyCode.R)) RestartChapter();
    }

    private void SwitchPlayer()
    {
        _activeType = 1 - _activeType;
        for (int i = 0; i < 2; i++)
        {
            _players[i].Switch();
        }
        _sfxManager.PlaySwitch();
    }

    private void GameDone()
    {
        _gameState = GameState.Over;
        if (!Cutscene) StartCoroutine(GameDoneRoutine());
        else _cutsceneManager.StartSequence();
    }

    private IEnumerator GameDoneRoutine()
    {
        PlayWin();
        yield return new WaitForSeconds(1);
        _sceneLoader.GoToNextScene();
    }

    private void ToggleGameMap()
    {
        if (_gameState == GameState.Play)
        {
            _gameState = GameState.Map;
            _levelText.SetActive(false);
            _mapText.SetActive(true);
        }
        else
        {
            _gameState = GameState.Play;
            _levelText.SetActive(true);
            _mapText.SetActive(false);
        }
    }
    
    /// <summary>
    /// Toggle game to pause or not.
    /// </summary>
    public void ToggleGamePause()
    {
        if (_gameState == GameState.Play)
        {
            _gameState = GameState.Pause;
            _pauseMenu.SetActive(true);
        }
        else
        {
            _gameState = GameState.Pause;
            _pauseMenu.SetActive(false);
        }
        PlayPause();
    }

    /// <summary>
    /// Restart the level.
    /// </summary>
    public void RestartChapter()
    {
        if (_gameState == GameState.Play)
        {
            _gameState = GameState.Over;
            _sceneLoader.RestartScene();
        }
    }

    /// <summary>
    /// Back to main menu.
    /// </summary>
    public void BackToMain()
    {
        _gameState = GameState.Over;
        _sceneLoader.GoToScene(0);
        PlayQuit();
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void QuitGame()
    {
        PlayQuit();
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public IEnumerator GameOver(Player player)
    {
        if (_gameState == GameState.Play)
        {
            _gameState = GameState.Over;

            StartCoroutine(_cameraManager.ShakeCamera());
            player.PlayDeathParticle();
            PlayLose();

            yield return new WaitForSeconds(_playerDeadAnimationTime);
            // _sceneLoader.RestartScene();
            StartCoroutine(_sceneLoader.RunAnimation(1));

            yield return new WaitForSeconds(0.35f);
            _gameState = GameState.Restart;
            for (int i = 0; i < 2; i++)
            {
                _players[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                if (_prevLevel < _currentLevel) _players[i].transform.position = _chapterManager.GetPlayerPos(_currentLevel, i, true);
                else _players[i].transform.position = _chapterManager.GetPlayerPos(_currentLevel, i, false);
            }

            yield return new WaitForSeconds(1);
            _gameState = GameState.Play;
        }
    }

    public void CutsceneDone()
    {
        Cutscene = false;
        _gameState = GameState.Play;
        _numberOfFinished = 0;
    }

    /// <summary>
    /// Get the camera position in the given level. If level is unspecified, then return the camera position in the current level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public Vector3 GetCameraPosInLevel(int level = -1)
    {
        if (level < 0) level = _currentLevel;
        // return _chapterManager.GetCameraPosInLevel(level);
        return _chapterManager.GetCameraPos(level);
    }

    public bool IsGamePlaying()
    {
        return _gameState == GameState.Play;
    }

    public bool IsGameOver()
    {
        return _gameState == GameState.Over;
    }

    public bool IsGameRestart()
    {
        return _gameState == GameState.Restart;
    }

    public bool IsGamePause()
    {
        return _gameState == GameState.Pause;
    }

    public bool IsMapOpen()
    {
        return _gameState == GameState.Map;
    }

    public int GetActiveType()
    {
        if (_gameState != GameState.Play) return -1;
        else return _activeType;
    }

    public Player GetPlayer(int type = -1)
    {
        if (type < 0) type = _activeType;
        return _players[type];
    }

    /// <summary>
    /// Change the currentLevel to the given level.
    /// </summary>
    /// <param name="level"></param>
    public void ChangeToLevel(int level)
    {
        if (level > _currentLevel)
        {
            for (int i = 0; i < 2; i++)
            {
                // _players[i].MovePosition(_playerPosAtLevelStart[level][i]);
                // _players[i].transform.position = _chapterManager.GetPlayerPosAtLevelStart(level, i);
                _players[i].transform.position = _chapterManager.GetPlayerPos(level, i, true);
            }
        }
        else if (level < _currentLevel)
        {
            for (int i = 0; i < 2; i++)
            {
                // _players[i].MovePosition(_playerPosAtLevelEnd[level][i]);
                // _players[i].transform.position = _chapterManager.GetPlayerPosAtLevelEnd(level, i);
                _players[i].transform.position = _chapterManager.GetPlayerPos(level, i, false);
            }
        }

        _prevLevel = _currentLevel;
        _currentLevel = level;
    }

    /// <summary>
    /// Return the currentLevel.
    /// </summary>
    /// <returns></returns>
    public int CurrentLevel()
    {
        return _currentLevel;
    }

    public void PlayJump()
    {
        _sfxManager.PlayJump();
    }

    public void PlayTargetIn()
    {
        _sfxManager.PlayTargetIn();
    }

    public void PlayTargetOut()
    {
        _sfxManager.PlayTargetOut();
    }

    public void PlayTargetSwitch()
    {
        _sfxManager.PlayTargetSwitch();
    }

    public void PlayExplode()
    {
        _sfxManager.PlayExplode();
    }

    public void PlayLose()
    {
        _sfxManager.PlayLose();
    }

    public void PlayWin()
    {
        _sfxManager.PlayWin();
    }

    public void PlayPause()
    {
        _sfxManager.PlayPause();
    }

    public void PlayStart()
    {
        _sfxManager.PlayStart();
    }

    public void PlayQuit()
    {
        _sfxManager.PlayQuit();
    }
}
