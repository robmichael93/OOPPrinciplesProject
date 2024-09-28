using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] public int row;
    [SerializeField] public int column;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
    }

    // ABSTRACTION: calls the processing function in the MainManager when the crate is
    // clicked on
    private void OnMouseDown()
    {
        if (gameManager.isGameActive)
        {
            gameManager.SelectSquare(this);
        }
    }
}
