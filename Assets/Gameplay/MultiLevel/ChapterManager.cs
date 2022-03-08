using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    [SerializeField] private Transform _cameraPosParent;
    [SerializeField] private Transform _player0PosStartParent;
    [SerializeField] private Transform _player0PosEndParent;
    [SerializeField] private Transform _player1PosStartParent;
    [SerializeField] private Transform _player1PosEndParent;
    [SerializeField] private List<int> _levelsWithMovableCamera;

    private List<Transform> _cameraPos;
    private List<Transform> _player0PosStart;
    private List<Transform> _player0PosEnd;
    private List<Transform> _player1PosStart;
    private List<Transform> _player1PosEnd;

    private void Awake()
    {
        _cameraPos = GetSortedChildren(_cameraPosParent);
        _player0PosStart = GetSortedChildren(_player0PosStartParent);
        _player0PosEnd = GetSortedChildren(_player0PosEndParent);
        _player1PosStart = GetSortedChildren(_player1PosStartParent);
        _player1PosEnd = GetSortedChildren(_player1PosEndParent);
        _levelsWithMovableCamera.Sort();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static int CompareTransformByName(Transform x, Transform y)
    {
        // correct format: "Name Without Open Parantheses (X)", where X is an integer with any digit. (X) must be in the end.
        // "Name Only" will precedes all other name.
        int xPos = x.name.IndexOf('(');
        int yPos = y.name.IndexOf('(');
        if (xPos < 0)
        {
            if (yPos < 0)
            {
                Debug.LogWarning("There are two LevelPosition's GameObject's name that is not numbered or not formatted correctly");
                return 0;
            }
            else return -1;
        }
        else if (yPos < 0)
        {
            return 1;
        }
        else
        {
            xPos++; yPos++;
            int xNum, yNum;
            if (!int.TryParse(x.name.Substring(xPos, x.name.Length - xPos - 1), out xNum))
            {
                Debug.LogWarning("LevelPosition's GameObject's name is not formatted correctly");
                return 0;
            }
            if (!int.TryParse(y.name.Substring(yPos, y.name.Length - yPos - 1), out yNum))
            {
                Debug.LogWarning("LevelPosition's GameObject's name is not formatted correctly");
                return 0;
            }
            return xNum.CompareTo(yNum);
        }
    }

    private static List<Transform> GetSortedChildren(Transform parent)
    {
        List<Transform> sortedChildren = new List<Transform>();
        foreach (Transform child in parent)
        {
            sortedChildren.Add(child);
        }

        sortedChildren.Sort(CompareTransformByName);
        return sortedChildren;
    }

    public Vector2 GetPlayerPos(int level, int playerType, bool isAtStart)
    {
        if (playerType == 0)
        {
            if (isAtStart) return _player0PosStart[level].position;
            else return _player0PosEnd[level].position;
        }
        else
        {
            if (isAtStart) return _player1PosStart[level].position;
            else return _player1PosEnd[level].position;
        }
    }

    public Vector3 GetCameraPos(int level)
    {
        return _cameraPos[level].position;
    }

    public bool IsCameraMovable(int level)
    {
        return (_levelsWithMovableCamera.BinarySearch(level) >= 0);
    }
}
