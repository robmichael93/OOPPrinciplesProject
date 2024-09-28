using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MainGameManager : MonoBehaviour
{
    public static MainGameManager Instance;
    [SerializeField] public string playerName = "";
    [SerializeField] public string fastestPlayerName = "";
    [SerializeField] public int fastestTime = 0;
    [SerializeField] public string leastMovesPlayerName = "";
    [SerializeField] public int leastMoves;

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public string fastestPlayerName;
        public int fastestTime;
        public string leastMovesPlayerName;
        public int leastMoves;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadStats();
    }

    public void SaveStats()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.fastestPlayerName = fastestPlayerName;
        data.fastestTime = fastestTime;
        data.leastMovesPlayerName = leastMovesPlayerName;
        data.leastMoves = leastMoves;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadStats()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            playerName = data.playerName;
            fastestPlayerName = data.fastestPlayerName;
            fastestTime = data.fastestTime;
            leastMovesPlayerName = data.leastMovesPlayerName;
            leastMoves = data.leastMoves;
        }
    }
}
