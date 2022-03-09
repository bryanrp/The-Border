using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEditor;

public class GameManager : MonoBehaviour
{
    private GameState _gameState;

    public static GameManager Instance { get; private set; }

    private static CameraManager _cameraManager;
    private bool _isCameraMovable = false;

    private static ChapterManager _chapterManager;
    private static UnityEngine.Rendering.Universal.Vignette _volumeProfileVignette;

    private int _prevLevel = -1;
    private int _currentLevel = 0;
    [SerializeField] private int _startLevel = 0;

    private Player[] _players;
    private int _activeType = 0;

    private float _playerDeadAnimationTime = 0.5f;

    [SerializeField] private AudioClip _clipStart;
    [SerializeField] private AudioClip _clipPause;
    [SerializeField] private AudioClip _clipWin;
    [SerializeField] private AudioClip _clipLose;
    [SerializeField] private AudioClip _clipQuit;
    [SerializeField] private AudioClip _clipSwitchPlayer0;
    [SerializeField] private AudioClip _clipSwitchPlayer1;

    private GameObject _pauseMenu;
    private GameObject _doneMenu;
    private GameObject _levelText;
    private GameObject _mapText;

    private CutsceneManager _cutsceneManager;
    public bool Cutscene = false;

    void Awake()
    {
        Instance = this;

        _gameState = GameState.Play;

        AwakePhysicsManager();

        _cameraManager = Camera.main.GetComponent<CameraManager>();
        _chapterManager = GetComponent<ChapterManager>();
        GameObject.Find("PostProcessing")?.GetComponent<Volume>()?.profile.TryGet(out _volumeProfileVignette);

        Player playerA = GameObject.Find("PlayerA").GetComponent<Player>();
        Player playerB = GameObject.Find("PlayerB").GetComponent<Player>();
        _players = new Player[] { playerA, playerB };
        
        _currentLevel = _startLevel;

        _pauseMenu = GameObject.Find("Pause Menu");
        _doneMenu = GameObject.Find("Done Menu");
        _levelText = GameObject.Find("Level Text");
        _mapText = GameObject.Find("Map Text");
        if (_pauseMenu != null) _pauseMenu.SetActive(false);
        if (_doneMenu != null) _doneMenu.SetActive(false);
        if (_levelText != null) _levelText.GetComponent<Text>().text = "Level " + (_currentLevel + 1);
        if (_mapText != null) _mapText.SetActive(false);

        if (Cutscene) _cutsceneManager = GameObject.Find("Cutscene Manager").GetComponent<CutsceneManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (_pauseMenu != null && IsGamePlaying() && Input.GetKeyDown(KeyCode.Escape))
        {
            if (!Cutscene) ToggleGamePause();
            else SFXManager.Instance.Play(_clipPause);
        }
        if (IsGamePlaying() && Input.GetKeyDown(KeyCode.S)) SwitchPlayer();
        if (_mapText != null && (IsGamePlaying() || IsGamePause()) && Input.GetKeyDown(KeyCode.R)) StartCoroutine(RestartLevel());
    }

    /// <summary>
    /// Restart level. Called by button.
    /// </summary>
    public void ButtonRestartLevel()
    {
        StartCoroutine(RestartLevel());
        _pauseMenu.SetActive(false);
    }

    /// <summary>
    /// Back to main menu. Called by button.
    /// </summary>
    public void ButtonBackToMain()
    {
        _gameState = GameState.Over;
        SceneLoader.Instance.GoToScene(0);
        SFXManager.Instance.Play(_clipQuit);
    }

    /// <summary>
    /// Quit the game
    /// </summary>
    public void ButtonQuitGame()
    {
        SFXManager.Instance.Play(_clipQuit);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public IEnumerator GameOver(Player player)
    {
        if (IsGamePlaying())
        {
            _gameState = GameState.Over;

            StartCoroutine(_cameraManager.ShakeCamera());
            player.PlayDeathParticle();
            SFXManager.Instance.Play(_clipLose);
            yield return new WaitForSeconds(_playerDeadAnimationTime);

            StartCoroutine(RestartLevel());
        }
    }

    /// <summary>
    /// Get the camera position in the given level. If level is unspecified, then return the camera position in the current level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public Vector3 GetCameraPosInLevel(int level = -1)
    {
        if (level < 0) level = _currentLevel;
        return _chapterManager.GetCameraPos(level);
    }

    public bool IsCameraMovable()
    {
        return _isCameraMovable;
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
    public IEnumerator ChangeToLevel(int level)
    {
        if (level > _currentLevel)
        {
            for (int i = 0; i < 2; i++)
            {
                _players[i].transform.position = _chapterManager.GetPlayerPos(level, i, true);
            }
        }
        else if (level < _currentLevel)
        {
            for (int i = 0; i < 2; i++)
            {
                _players[i].transform.position = _chapterManager.GetPlayerPos(level, i, false);
            }
        }

        _prevLevel = _currentLevel;
        _currentLevel = level;

        yield return new WaitForSeconds(0.1f);
        _isCameraMovable = _chapterManager.IsCameraMovable(_currentLevel);
    }

    /// <summary>
    /// Return the currentLevel.
    /// </summary>
    /// <returns></returns>
    public int CurrentLevel()
    {
        return _currentLevel;
    }

    private void SwitchPlayer()
    {
        _activeType = 1 - _activeType;
        for (int i = 0; i < 2; i++)
        {
            _players[i].Switch();
        }

        if (_activeType == 0)
        {
            if (_volumeProfileVignette != null)
            {
                _volumeProfileVignette.color.value = new Color32(255, 85, 102, 255);
            }
        }
        else
        {
            if (_volumeProfileVignette != null)
            {
                _volumeProfileVignette.color.value = new Color32(101, 86, 255, 255);
            }
        }

        SFXManager.Instance.Play((Random.Range(0, 2) == 0 ? _clipSwitchPlayer0 : _clipSwitchPlayer1));
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
    private void ToggleGamePause()
    {
        if (_gameState == GameState.Play)
        {
            _gameState = GameState.Pause;
            _pauseMenu.SetActive(true);
        }
        else
        {
            _gameState = GameState.Play;
            _pauseMenu.SetActive(false);
        }
        SFXManager.Instance.Play(_clipPause);
    }

    /// <summary>
    /// Restart the level.
    /// </summary>
    private IEnumerator RestartLevel()
    {
        if (IsGamePlaying() || IsGamePause() || IsGameOver())
        {
            _gameState = GameState.Pause;
            StartCoroutine(SceneLoader.Instance.RunAnimation(1));
            yield return new WaitForSeconds(0.35f);

            _gameState = GameState.Restart;
            _isCameraMovable = false;
            for (int i = 0; i < 2; i++)
            {
                _players[i].GetComponent<Rigidbody2D>().simulated = false;
                if (_prevLevel < _currentLevel) _players[i].transform.position = _chapterManager.GetPlayerPos(_currentLevel, i, true);
                else _players[i].transform.position = _chapterManager.GetPlayerPos(_currentLevel, i, false);
            }
            yield return new WaitForSeconds(1);

            _isCameraMovable = _chapterManager.IsCameraMovable(_currentLevel);
            for (int i = 0; i < 2; i++)
            {
                _players[i].GetComponent<Rigidbody2D>().simulated = true;
                _players[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            _gameState = GameState.Play;
        }
    }

    private void AwakePhysicsManager()
    {
        GetComponent<PhysicsManager>().GameManagerAwake();
    }

    private enum GameState
    {
        Play,
        Pause,
        Map,
        Restart,
        Over
    }
}
