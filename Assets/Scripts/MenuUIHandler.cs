using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private MainGameManager mainGameManager;
    [SerializeField] public TMP_InputField nameInput;
    [SerializeField] public TextMeshProUGUI fastestTimeInfo;
    [SerializeField] public TextMeshProUGUI leastMovesInfo;
    // Start is called before the first frame update
    void Start()
    {
        mainGameManager = MainGameManager.Instance;
        fastestTimeInfo.text = "Fastest Time: " + mainGameManager.fastestTime;
        if (mainGameManager.fastestPlayerName != "")
        {
            fastestTimeInfo.text += " (" + mainGameManager.fastestPlayerName + ")";
        }
        leastMovesInfo.text = "Least Moves: " + mainGameManager.leastMoves;
        if (mainGameManager.leastMovesPlayerName != "")
        {
            leastMovesInfo.text += " (" + mainGameManager.leastMovesPlayerName + ")";
        }
        nameInput.text = mainGameManager.playerName;
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    public void NewPlayerName(string name)
    {
        MainGameManager.Instance.playerName = name;
    }
}
