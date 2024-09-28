using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.VisualScripting;
using System.Runtime.InteropServices.WindowsRuntime;

public class GameManager : MonoBehaviour
{
<<<<<<< Updated upstream
    public TextMeshProUGUI attemptsText;
    public TextMeshProUGUI matchesText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI gameOverText;
    public GameObject titleScreen;
    public Button restartButton; 

    public List<GameObject> foodPrefabs;
    public GameObject boxPrefab;
    private int matchesMade;
    private float time;
    public bool isGameActive;

    private float spaceBetweenSquares = 2.5f; 
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square
=======
    [SerializeField] private MainGameManager mainGameManager;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] public TextMeshProUGUI attemptsText;
    [SerializeField] public TextMeshProUGUI matchesText;
    [SerializeField] public TextMeshProUGUI timerText;
    [SerializeField] public TextMeshProUGUI fastestTimeText;
    [SerializeField] public TextMeshProUGUI leastMovesText;
    [SerializeField] public List<GameObject> foodPrefabs;
    [SerializeField] public AudioSource gameAudio;
    [SerializeField] public AudioClip restartButtonSound;
    [SerializeField] public AudioClip menuButtonSound;
    [SerializeField] public GameObject boxPrefab;
    [SerializeField] private Crate firstPickedCrate;
    [SerializeField] private GameObject firstFood;
    [SerializeField] private Crate secondPickedCrate;
    [SerializeField] private GameObject secondFood;
    [SerializeField] private float time;
    [SerializeField] private float spaceBetweenSquares = 2.5f; 
    [SerializeField] private float minValueX = -3.75f; //  x value of the center of the left-most square
    [SerializeField] private float minValueY = -3.75f; //  y value of the center of the bottom-most square
    [SerializeField] private int matchesMade;
>>>>>>> Stashed changes
    [SerializeField] private int numberOfFoodPairs;
    [SerializeField] private int numberOfRows;
    [SerializeField] private int numberOfColumns;
    [SerializeField] private int cookiePairs;
    [SerializeField] private int pizzaPairs;
    [SerializeField] private int sandwichPairs;
    [SerializeField] private int steakPairs;
    [SerializeField] private int maxPairsPerType;
    [SerializeField] private Crate firstPickedCrate;
    [SerializeField] private GameObject firstFood;
    [SerializeField] private bool firstPairPicked;
    [SerializeField] private string firstPairFoodType;
    [SerializeField] private Crate secondPickedCrate;
    [SerializeField] private GameObject secondFood;
    [SerializeField] private bool secondPairPicked;
    [SerializeField] private string secondPairFoodType;
    [SerializeField] private int numberOfAttemptedMatches;
    public enum occupancy
    {
        Empty,
        Occupied
    };

    // ENCAPSULATION: public getters and setters with backing fields and setter validations
    public struct squareState
    {
        public squareState(int row, int column)
        {
            m_rowIndex = row;
            m_columnIndex = column;
            m_foodType = "Cookie";
            m_occupiedState = occupancy.Empty;
        }

        private int m_rowIndex;
        public int rowIndex
        {
            get {return m_rowIndex;}
            set
            {
                if (value < 0 || value > 3)
                {
                    Debug.LogError("Row index must be between 0 and 3.");
                }
                else
                {
                    {
                        m_rowIndex = value;
                    }
                }
            }
        }
        private int m_columnIndex;
        public int columnIndex
        {
            get {return m_columnIndex;}
            set
            {
                if (value < 0 || value > 3)
                {
                    Debug.LogError("Column index must be between 0 and 3.");
                }
                else
                {
                    {
                        m_columnIndex = value;
                    }
                }
            }
        }
        
        private occupancy m_occupiedState;
        public occupancy occupiedState
        {
            get {return m_occupiedState;}
            set
            {
                if (value == occupancy.Empty || value == occupancy.Occupied)
                {
                    m_occupiedState = value;
                }
                else
                {
                    Debug.LogError("Occupancy state must be occupancy.Empty or occupancy.Occupied");
                }
            }

        }

        private string m_foodType;
        public string foodType
        {
            get {return m_foodType;}
            set
            {
                if (value.Equals("Cookie") || value.Equals("Pizza") || value.Equals("Sandwich") || value.Equals("Steak"))
                {
                    m_foodType = value;
                }
                else
                {
                    Debug.LogError("Food type not valid.  Type passed in was: " + value);
                }
            }
        }
    }
    [SerializeField] private squareState[,] boardSquares;

    public squareState[,] GetBoard()
    {
        return boardSquares;
    }
    
    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked
    public void StartGame()
    {
<<<<<<< Updated upstream
=======
        mainGameManager = MainGameManager.Instance;
        gameAudio = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<AudioSource>();
        gameAudio.Play();
>>>>>>> Stashed changes
        isGameActive = true;
        matchesMade = 0;
        numberOfAttemptedMatches = 0;
        time = 0;
        cookiePairs = pizzaPairs = sandwichPairs = steakPairs = 0;
        maxPairsPerType = 2;
        UpdateTime(time);
        titleScreen.SetActive(false);
        numberOfRows = 4;
        numberOfColumns = 4;
        numberOfFoodPairs = (numberOfRows * numberOfColumns) / 2;
        boardSquares = CreateSquareStates(numberOfRows, numberOfColumns);
        FillBoardWithFoodPairs(numberOfFoodPairs);
        SpawnCrates();
    }

    private void Update()
    {
        if (isGameActive)
        {
            CountupTimer();    
        }
    }

    // ABSTRACTION: function fills in the array of squareStates, initializing each one with a constructor
    private squareState[,] CreateSquareStates(int numRows, int numCols)
    {
        squareState[,] board = new squareState[numRows, numCols];
        for (int i = 0; i < numRows; i++)
        {
            for (int j = 0; j < numCols; j++)
            {
                board[i,j] = new squareState(i, j);
            }
        }
        return board;
    }

    // ABSTRACTION: function fills in pairs of foods in the squareStates, interating
    // by row and then by column, skipping occupied squares (already initialized with
    // a food type string and changing the occupancy enum to Occupied)
    private void FillBoardWithFoodPairs(int numberOfPairs)
    {
        int numberOfPairsToMake = numberOfPairs;
        for (int row = 0; row < numberOfRows && numberOfPairsToMake > 0; row++)
        {
            for (int column = 0; column < numberOfColumns && numberOfPairsToMake > 0; column++)
            {
                if (boardSquares[row, column].occupiedState == occupancy.Empty)
                {
                    // Find the first unoccupied square & put a food in it, marking it as occupied
                    bool availableFoodPair = false;
                    int foodTypeIndex = -1;
                    while (!availableFoodPair)
                    {
                        int potentialFoodTypeIndex = Random.Range(0, foodPrefabs.Count);
                        if (GetFoodPairCount(potentialFoodTypeIndex) < maxPairsPerType)
                        {
                            boardSquares[row, column].foodType = GetFoodDescriptionFromIndex(potentialFoodTypeIndex);
                            boardSquares[row, column].occupiedState = occupancy.Occupied;
                            IncrementFoodPairCount(potentialFoodTypeIndex);
                            foodTypeIndex = potentialFoodTypeIndex;
                            availableFoodPair = true;
                        }
                    }

                    // Find a random unoccupied square & put the same food in it, marking it as occupied
                    bool unoccupiedPairSquareFound = false;
                    while (!unoccupiedPairSquareFound)
                    {
                        int i = Random.Range(0, numberOfRows);
                        int j = Random.Range(0, numberOfColumns);
                        if (boardSquares[i, j].occupiedState == occupancy.Empty)
                        {
                            boardSquares[i, j].foodType = GetFoodDescriptionFromIndex(foodTypeIndex);
                            boardSquares[i, j].occupiedState = occupancy.Occupied;
                            unoccupiedPairSquareFound = true;
                            numberOfPairsToMake--;
                        }
                    };
                }
            }
        }
    }

    // ABSTRACTION: function that gets the current number of pairs for a food (by index)
    // to ensure that only the right amount of pairs are created
    private int GetFoodPairCount(int index)
    {
        switch(index)
        {
            case 0:
                return cookiePairs;
            case 1:
                return pizzaPairs;
            case 2:
                return sandwichPairs;
            case 3:
                return steakPairs;
        }
        return -1;
    }

    // ABSTRACTION: function that increments the pair count for a food (by index)
    // so that when checked by GetFoodPairCount and compared to maxPairsPerType
    // so that the right amount of pairs are created
    private void IncrementFoodPairCount(int index)
    {
        switch(index)
        {
            case 0:
                cookiePairs++;
                break;
            case 1:
                pizzaPairs++;
                break;
            case 2:
                sandwichPairs++;
                break;
            case 3:
                steakPairs++;
                break;
        }
    }

    // ABSTRACTION: using a index number, returns a string of the food name to be
    // added to the squareState for a give row/column index in the double array
    private string GetFoodDescriptionFromIndex(int index)
    {
        string newFoodType = "";
        switch (index)
        {
        case 0:
            newFoodType = "Cookie";
            break;
        case 1:
            newFoodType = "Pizza";
            break;
        case 2:
            newFoodType = "Sandwich";
            break;
        case 3:
            newFoodType = "Steak";
            break;
        default:
            Debug.LogError("Food index must be between 0 and 3; no food selected.");
            break;
        }
        return newFoodType;
    }

    // ABSTRACTION: function fills the board with instantiated Crate objects that can
    // be clicked on to reveal the hidden food
    void SpawnCrates()
    {
        for (int i = 0; i < numberOfRows; i++)
        {
            for (int j = 0; j < numberOfColumns; j++)
            {
                Crate newCrate = (Instantiate(boxPrefab, SpawnPosition(i, j), boxPrefab.transform.rotation)).GetComponent<Crate>();
                newCrate.row = i;
                newCrate.column = j;
            }
        }
    }

    // ABSTRACTION: computes the actual x, y coordinate pair based on the row and
    // column of a given square on the board for spawning a crate
    Vector3 SpawnPosition(int row, int column)
    {
        float spawnPosX = minValueX + (row * spaceBetweenSquares);
        float spawnPosY = minValueY + (column * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;
    }

    // ABSTRACTION: the main logic for selecting the first and then second crate
    // on the board, then comparing the foods using the foodType variable in the
    // squareState struct.  Runs coroutines to either update the number of matches
    // and delete both the foods and their crates (which are hidden when clicked on),
    // or un-hides the crates and deletes the food objects.  Coroutines handle
    // further processing like updating the number of attempted matches and number of
    // actual matches on the screen, as well as resetting variables for the next
    // match attempt.
    public void SelectSquare(Crate crate)
    {
        if (!firstPairPicked)
        {
            squareState firstPickedSquare = boardSquares[crate.row, crate.column];
            firstPairFoodType = firstPickedSquare.foodType;
            firstPickedCrate = crate;
            firstPickedCrate.gameObject.SetActive(false);
            firstFood = SpawnFoodObject(crate, firstPairFoodType);
            Food firstFoodScript = firstFood.GetComponent<Food>();
            firstFoodScript.Reveal();
            firstFoodScript.PlaySound();
            firstPairPicked = true;
        }
        else if (firstPairPicked && !secondPairPicked)
        {
            squareState secondPickedSquare = boardSquares[crate.row, crate.column];
            secondPairFoodType = secondPickedSquare.foodType;
            secondPickedCrate = crate;
            secondPickedCrate.gameObject.SetActive(false);
            secondFood = SpawnFoodObject(crate, secondPairFoodType);
            Food secondFoodScript = secondFood.GetComponent<Food>();
            secondFoodScript.Reveal();
            secondFoodScript.PlaySound();
            secondPairPicked = true;
            numberOfAttemptedMatches++;
            if (firstPairPicked && secondPairPicked)
            {
                if (firstPairFoodType == secondPairFoodType)
                {
                    StartCoroutine(RemoveObjectsAndUpdateMatches());
                }
                else
                {
                    StartCoroutine(ResetCrates());
                }
            }
        }
    }

    // ABSTRACTION: handles removing the crate and food objects after a delay,
    // then calls further abstracted functions for updating the number of matches
    // and attempts on the screen.  Also resets the variables needed in SelectSquare.
    IEnumerator RemoveObjectsAndUpdateMatches()
    {
        yield return new WaitForSeconds(2);
        Destroy(firstPickedCrate.gameObject);
        Destroy(secondPickedCrate.gameObject);
        Destroy(firstFood.gameObject);
        Destroy(secondFood.gameObject);
        UpdateMatches();
        UpdateAttempts();
        ResetPickingVariables();
    }

    // ABSTRACTION: handles un-hiding the crates, deleting the food objects,
    // updating the number of match attempts on the screen, and resetting the 
    // variables used in SelectSquare.
    IEnumerator ResetCrates()
    {
        yield return new WaitForSeconds(2);
        firstPickedCrate.gameObject.SetActive(true);
        secondPickedCrate.gameObject.SetActive(true);
        Destroy(firstFood.gameObject);
        Destroy(secondFood.gameObject);
        UpdateAttempts();
        ResetPickingVariables();
    }

    // ABSTRACTION: resets the variables used in SelectSquare
    private void ResetPickingVariables()
    {
        firstPairPicked = false;
        secondPairPicked = false;
        firstPickedCrate = null;
        secondPickedCrate = null;
        firstFood = null;
        firstPairFoodType = "";
        secondFood = null;
        secondPairFoodType = "";
    }

    // ABSTRACTION: uses a Crate to get its x,y coordinates for spawning children of
    // the Food class from the foodPrefabs List, using the foodType string
    private GameObject SpawnFoodObject(Crate crate, string foodType)
    {
        int prefabIndex = 0;
        switch(foodType)
        {
            case "Cookie":
                prefabIndex = 0;
                break;
            case "Pizza":
                prefabIndex = 1;
                break;
            case "Sandwich":
                prefabIndex = 2;
                break;
            case "Steak":
                prefabIndex = 3;
                break;
            default:
                break;            
        }
        return Instantiate(foodPrefabs[prefabIndex], crate.gameObject.transform.position, foodPrefabs[prefabIndex].gameObject.transform.rotation);
    }

    // ABSTRACTION: updates the number of attempted matches on the screen
    void UpdateAttempts()
    {
        attemptsText.text = "Attempts: " + numberOfAttemptedMatches;
    }

    // ABSTRACTION: updates the number of matches on the screen.  Checks to see if all of
    // the matches have been made, calling GameOver if appropriate.
    public void UpdateMatches()
    {
        matchesMade++;
        matchesText.text = "Matches: " + matchesMade;
        if(matchesMade == numberOfFoodPairs)
        {
            GameOver();
        }
    }

    // ABSTRACTION: updates the running time on the screen
    void UpdateTime(float currentTime)
    {
        timerText.text = "Time: " + Mathf.FloorToInt(currentTime % 60);
    }

    // ABSTRACTION: checks to see if there is a new fastest time and least number
    // of moves (matches), updating the variables in the MainGameManager.  Saves
    // data to the save game file via the MainGameManager and shows the game over screen.
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
<<<<<<< Updated upstream
=======
        // gameAudio.Stop();
        gameOverScreen.gameObject.SetActive(true);
>>>>>>> Stashed changes
    }

    // ABSTRACTION: basic elapsed time timer
    void CountupTimer()
    {
        time += Time.deltaTime;
        UpdateTime(time);
    }

    // ABSTRACTION: restarts the game when the Restart button is clicked
    public void RestartGame()
    {
        gameAudio.PlayOneShot(restartButtonSound, 4);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

<<<<<<< Updated upstream
=======
    // ABSTRACTION: returns to the main menu when the Menu button is clicked
    public void ReturnToMenu()
    {
        gameAudio.PlayOneShot(menuButtonSound, 4);
        SceneManager.LoadScene(0);
    }
>>>>>>> Stashed changes
}
