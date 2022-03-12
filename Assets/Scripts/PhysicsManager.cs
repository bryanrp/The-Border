using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PhysicsManager : MonoBehaviour
{
    /*
     * Inspired from https://forum.unity.com/threads/separating-physics-scenes.597697/
    */

    public static PhysicsManager Instance { get; private set; }

    private const string _secondSceneName = "SecondaryScene";
    private string _firstSceneName;

    private PhysicsScene2D[] _physicsScenes;
    private const float _physicsSlowedTimeScale = 0.1f;

    /// <summary>
    /// Only call this function from GameManager.Awake().
    /// </summary>
    public void GameManagerAwake()
    {
        Instance = this;

        Physics2D.simulationMode = SimulationMode2D.Script;

        _firstSceneName = SceneManager.GetActiveScene().name;

        CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics2D);

        _physicsScenes = new PhysicsScene2D[2];
        _physicsScenes[0] = SceneManager.GetActiveScene().GetPhysicsScene2D();
        _physicsScenes[1] = SceneManager.CreateScene(_secondSceneName, csp).GetPhysicsScene2D();
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.IsGamePause()) return;

        int activeType = GameManager.Instance.GetActiveType();

        if (activeType == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if (_physicsScenes[i] != null && _physicsScenes[i].IsValid())
                {
                    float physicsSceneTimeScale = 1;
                    if (activeType != i) physicsSceneTimeScale = _physicsSlowedTimeScale;
                    if (GameManager.Instance.IsGameRestart()) physicsSceneTimeScale = 10;

                    _physicsScenes[i].Simulate(Time.fixedDeltaTime * physicsSceneTimeScale);
                }
            }
        }
        else
        {
            for (int i = 1; i >= 0; i--)
            {
                if (_physicsScenes[i] != null && _physicsScenes[i].IsValid())
                {
                    float physicsSceneTimeScale = 1;
                    if (activeType != i) physicsSceneTimeScale = _physicsSlowedTimeScale;
                    if (GameManager.Instance.IsGameRestart()) physicsSceneTimeScale = 10;

                    _physicsScenes[i].Simulate(Time.fixedDeltaTime * physicsSceneTimeScale);
                }
            }
        }
    }

    /// <summary>
    /// Move the given gameObject to the given scene. Main scene's type value is 0.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="type"></param>
    public void MoveGameObjectToScene(GameObject gameObject, int type = 0)
    {
        if (type == 0)
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(_firstSceneName));
        }
        else
        {
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(_secondSceneName));
        }
    }
}