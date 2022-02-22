using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterManager : MonoBehaviour
{
    [SerializeField] private Transform[] _cameraPos;
    [SerializeField] private Transform[] _player0PosStart;
    [SerializeField] private Transform[] _player0PosEnd;
    [SerializeField] private Transform[] _player1PosStart;
    [SerializeField] private Transform[] _player1PosEnd;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
