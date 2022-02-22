using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ChapterManagerOld : MonoBehaviour
{
    private const string path = "Gameplay/MultiLevel/ChapterData.json";
    private static JsonSerializerSettings _jsonSettings;

    private ChapterData _chapterData;

    private void Awake()
    {
        if (_jsonSettings == null)
        {
            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.Formatting = Formatting.Indented;
        }

        ReadChapterData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ReadChapterData()
    {
        System.IO.StreamReader reader = new System.IO.StreamReader(Application.dataPath + '/' + path);
        string json = reader.ReadToEnd();
        reader.Close();
        _chapterData = JsonConvert.DeserializeObject<ChapterData>(json, _jsonSettings);
    }

    private void WriteChapterData()
    {
        string json = JsonConvert.SerializeObject(_chapterData, _jsonSettings);
        System.IO.StreamWriter writer = new System.IO.StreamWriter(Application.dataPath + '/' + path);
        writer.Write(json);
        writer.Close();
    }

    /// <summary>
    /// Get the camera position in the given level
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public Vector3 GetCameraPosInLevel(int level)
    {
        return _chapterData._cameraPosInLevel[level];
    }

    /// <summary>
    /// Get the player start position in the given level and player type
    /// </summary>
    /// <param name="level"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Vector2 GetPlayerPosAtLevelStart(int level, int type)
    {
        return _chapterData._playerPosAtLevelStart[level][type];
    }

    /// <summary>
    /// Get the player end position in the given level and player type
    /// </summary>
    /// <param name="level"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Vector2 GetPlayerPosAtLevelEnd(int level, int type)
    {
        return _chapterData._playerPosAtLevelEnd[level][type];
    }
}

[System.Serializable]
public class ChapterData
{
    public Vector3[] _cameraPosInLevel;
    public Vector2[][] _playerPosAtLevelStart;
    public Vector2[][] _playerPosAtLevelEnd;
}