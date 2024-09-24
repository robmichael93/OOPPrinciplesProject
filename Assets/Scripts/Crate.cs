using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] public int row;
    [SerializeField] public int column;
    // Start is called before the first frame update
    public Crate(int row, int column)
    {
        this.row = row;
        this.column = column;
    }

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            string foodType = gameManager.GetBoard()[row, column].foodType;
            SpawnFoodObject(foodType);
            Destroy(gameObject);
        }
    }

    private void SpawnFoodObject(string foodType)
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
        Instantiate(gameManager.foodPrefabs[prefabIndex], gameObject.transform.position, gameManager.foodPrefabs[prefabIndex].gameObject.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
