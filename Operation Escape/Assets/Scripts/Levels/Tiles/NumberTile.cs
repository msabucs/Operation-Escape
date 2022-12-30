using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NumberTile : MonoBehaviour {
    [HideInInspector] public GameObject player;
    [HideInInspector] public static int currentOpNum;
    [HideInInspector] public int sceneIndex, origSpriteFileNo, newSpriteFileNo;
    public int number; 
    public SpriteRenderer sp;
    Object[] numberTileSprites;
    GameManager gameManager;
    DialogueManager dialogueManager;

    void Start() {

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        // Level 0
        if(sceneIndex == 2) {
            dialogueManager = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        }

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        origSpriteFileNo = (number * 2) + 1;
        newSpriteFileNo = (number * 2) + 2;

        sp = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");

        numberTileSprites = Resources.LoadAll("Sprites/Tiles/NumberTile");   
        sp.sprite = (Sprite)numberTileSprites[origSpriteFileNo];
    }

    // Get the active current operator
    public void CurrentOperator(int opNum) {
        currentOpNum = opNum;
    }
    
    // Perform new equation based on the current operator
    private void NewEquation() {

        // Addition
        if(currentOpNum == 1) {
            gameManager.currentNumber += number;
        }
        // Subtraction
        else if(currentOpNum == 2) {
            gameManager.currentNumber -= number;
        }
        // Multiplication
        else if(currentOpNum == 3) {
            gameManager.currentNumber *= number;
        }
        // Division
        else if(currentOpNum == 4) {
            gameManager.currentNumber /= number;
        }
    }

    // Display stepped number tile
    void OnTriggerEnter2D(Collider2D col) {

        if(col.gameObject == player) {

            // Level 0
            if(sceneIndex == 2) {

                if(dialogueManager.dialogueCounter == 10) {
                    dialogueManager.dialogueCounter++;
                }
            }
        
            sp.sprite = (Sprite)numberTileSprites[newSpriteFileNo];
            gameManager.moves--;
            NewEquation();
            FindObjectOfType<AudioManager>().Play("NumberTile");
        }
    }

    // Display original number tile
    void OnTriggerExit2D(Collider2D col) {

        if(col.gameObject == player) {
            sp.sprite = (Sprite)numberTileSprites[origSpriteFileNo];
        }
    }
}
