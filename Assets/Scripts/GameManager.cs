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

    private void FillBoardWithFoodPairs(int numberOfPairs)
    {
        int numberOfPairsToMake = numberOfPairs;
        // Debug.Log("Number of pairs to make: " + numberOfPairsToMake);
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

    // Generate a spawn position based on the row and column indices
    Vector3 SpawnPosition(int row, int column)
    {
        float spawnPosX = minValueX + (row * spaceBetweenSquares);
        float spawnPosY = minValueY + (column * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;
    }

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

    void UpdateAttempts()
    {
        attemptsText.text = "Attempts: " + numberOfAttemptedMatches;
    }

    // Update number of matches made when second crate is selected and if it is a match to the first crate
    public void UpdateMatches()
    {
        matchesMade++;
        matchesText.text = "Matches: " + matchesMade;
        if(matchesMade == numberOfFoodPairs)
        {
            GameOver();
        }
    }

    void UpdateTime(float currentTime)
    {
        timerText.text = "Time: " + Mathf.FloorToInt(currentTime % 60);
    }

    // Stop game, bring up game over text and restart button
    public void GameOver()
    {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    void CountupTimer()
    {
        time += Time.deltaTime;
        UpdateTime(time);
    }

    // Restart game by reloading the scene
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
