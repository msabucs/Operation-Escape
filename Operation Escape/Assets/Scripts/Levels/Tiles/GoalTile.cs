using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalTile : MonoBehaviour {
    [HideInInspector] public int sceneIndex;
    [HideInInspector] public GameObject player;
    public SpriteRenderer sp;
    public Sprite origSprite, newSprite;
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

        sp = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
    }

    // Display stepped goal tile
    void OnTriggerEnter2D(Collider2D col) {

        if(col.gameObject == player) {  

            // Level 0
            if(sceneIndex == 2) {
                
                if(dialogueManager.dialogueCounter == 13) {
                    dialogueManager.dialogueCounter++;
                }
            }

            sp.sprite = newSprite;
            gameManager.moves--;

            if(gameManager.currentNumber == gameManager.goalNumber) {
                gameManager.isEqual = true;
            }
        }
    }

    // Display original goal tile
    void OnTriggerExit2D(Collider2D col) {

        if(col.gameObject == player) {
            sp.sprite = origSprite;
        }
    }
}
