using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PhysicsManager : MonoBehaviour
{
    /*
     * This is the example from https://forum.unity.com/threads/separating-physics-scenes.597697/ with slight modification
    
    PhysicsScene scene1Physics;
    PhysicsScene scene2Physics;
    float timer1 = 0;
    float timer2 = 0;

    void Start()
    {
        Physics.autoSimulation = false;

        //this floor remains in the default PhysicsScene, ~meaning none of the cubes will interact with it~
        GameObject floor1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor1.transform.localScale = new Vector3(10, 1, 10);
        floor1.transform.position = new Vector3(0, -0.5f, 1);

        //cubes 1 and 2 will be added to scene1Physics
        GameObject cube1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //cubes 3 and 4 will be added to scene2Physics
        GameObject cube3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        GameObject cube4 = GameObject.CreatePrimitive(PrimitiveType.Cube);

        cube1.transform.position = new Vector3(0, 8, -1);
        cube2.transform.position = new Vector3(0, 8, -2);
        //cubes 3 and 4 are directly above cubes 1 and 2
        cube3.transform.position = new Vector3(0, 8, 1);
        cube4.transform.position = new Vector3(0, 8, 2);

        cube1.AddComponent<Rigidbody>();
        cube2.AddComponent<Rigidbody>();
        cube3.AddComponent<Rigidbody>();
        cube4.AddComponent<Rigidbody>();

        //the LocalPhysicsMode is what create a new PhysicsScene separate from the default
        CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics3D);
        Scene scene1 = SceneManager.CreateScene("MyScene1", csp);
        scene1Physics = scene1.GetPhysicsScene();

        Scene scene2 = SceneManager.CreateScene("MyScene2", csp);
        scene2Physics = scene2.GetPhysicsScene();

        SceneManager.MoveGameObjectToScene(cube1, scene1);
        SceneManager.MoveGameObjectToScene(cube2, scene1);
        SceneManager.MoveGameObjectToScene(cube3, scene2);
        SceneManager.MoveGameObjectToScene(cube4, scene2);

        // creating floor for both physics scene
        GameObject floor2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
        floor2.transform.localScale = new Vector3(10, 1, 10);
        floor2.transform.position = new Vector3(0, -0.5f, 1);
        SceneManager.MoveGameObjectToScene(floor1, scene1);
        SceneManager.MoveGameObjectToScene(floor2, scene2);
    }
    */

    private const string _secondSceneName = "SecondaryScene";
    private string _firstSceneName;

    private GameManager _gameManager;
    private PhysicsScene2D[] _physicsScenes;
    private const float _physicsSlowedTimeScale = 0.1f;

    private static Queue<Player> _playerToSetSpeedY;

    /// <summary>
    /// Only call this function from GameManager.Awake().
    /// </summary>
    public void GameManagerAwake()
    {
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        Physics2D.simulationMode = SimulationMode2D.Script;

        _firstSceneName = SceneManager.GetActiveScene().name;

        CreateSceneParameters csp = new CreateSceneParameters(LocalPhysicsMode.Physics2D);

        _physicsScenes = new PhysicsScene2D[2];
        _physicsScenes[0] = SceneManager.GetActiveScene().GetPhysicsScene2D();
        _physicsScenes[1] = SceneManager.CreateScene(_secondSceneName, csp).GetPhysicsScene2D();

        _playerToSetSpeedY = new Queue<Player>();
    }

    void FixedUpdate()
    {
        int activeType = _gameManager.GetActiveType();

        if (activeType == 0)
        {
            for (int i = 0; i < 2; i++)
            {
                if (_physicsScenes[i] != null && _physicsScenes[i].IsValid())
                {
                    float physicsSceneTimeScale = 1;
                    if (activeType != i) physicsSceneTimeScale = _physicsSlowedTimeScale;
                    if (_gameManager.IsGameRestart()) physicsSceneTimeScale = 10;

                    // commenting the next line will stop the physics in scene[i]
                    _physicsScenes[i].Simulate(Time.fixedDeltaTime * physicsSceneTimeScale);
                }

                //if (_playerToSetSpeedY.Count > 0)
                //{
                //    Debug.Break();
                //}
                //while (_playerToSetSpeedY.Count > 0)
                //{
                //    Player player = _playerToSetSpeedY.Dequeue();
                //    Debug.Log("(0) Player speed: " + player.GetComponent<Rigidbody2D>().velocity.y);
                //    player.SetSpeedY(0);
                //}
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
                    if (_gameManager.IsGameRestart()) physicsSceneTimeScale = 10;

                    // commenting the next line will stop the physics in scene[i]
                    _physicsScenes[i].Simulate(Time.fixedDeltaTime * physicsSceneTimeScale);
                }

                //if (_playerToSetSpeedY.Count > 0)
                //{
                //    Debug.Break();
                //}
                //while (_playerToSetSpeedY.Count > 0)
                //{
                //    Player player = _playerToSetSpeedY.Dequeue();
                //    Debug.Log("(1) Player speed: " + player.GetComponent<Rigidbody2D>().velocity.y);
                //    player.SetSpeedY(0);
                //}
            }
        }
    }

    /*
    public void SwitchPlayer()
    {
        int activeType = _gameManager.GetActiveType();

        for (int i = 0; i < 2; i++)
        {
            if (_physicsScenes[i] != null && _physicsScenes[i].IsValid())
            {
                float physicsSceneTimeScale = 1;
                if (activeType != i) physicsSceneTimeScale = _physicsSlowedTimeScale;
                if (_gameManager.IsGameRestart()) physicsSceneTimeScale = 10;

                // commenting the next line will stop the physics in scene[i]
                _physicsScenes[i].Simulate(Time.fixedDeltaTime * physicsSceneTimeScale);
            }
        }
    }
    */

    /// <summary>
    /// Move the given gameObject to the given scene. Default scene type value is 0.
    /// </summary>
    /// <param name="gameObject"></param>
    /// <param name="type"></param>
    public void MoveGameObjectToScene(GameObject gameObject, int type = 0)
    {
        if (type == 0) SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(_firstSceneName));
        else SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName(_secondSceneName));
    }

    public static void QueuePlayerToSetSpeedY(Player player)
    {
        _playerToSetSpeedY.Enqueue(player);
    }
}