using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

// ABSTRACTION: all of this class's functions handle common game functions, like
// filling in data upon start, starting the game in the Main scene, exiting the game,
// and capturing the player's name when it is typed into the input field.
public class MenuUIHandler : MonoBehaviour
{
    [SerializeField] private MainGameManager mainGameManager;
    [SerializeField] public TMP_InputField nameInput;
    [SerializeField] public TextMeshProUGUI fastestTimeInfo;
    [SerializeField] public TextMeshProUGUI leastMovesInfo;
    [SerializeField] public AudioSource menuAudio;
    [SerializeField] public AudioClip startButtonSound;
    [SerializeField] public AudioClip quitButtonSound;
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
        menuAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
    }

    public void StartNew()
    {
        menuAudio.PlayOneShot(startButtonSound, 1);
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
        menuAudio.PlayOneShot(quitButtonSound, 1);
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
