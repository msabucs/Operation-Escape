using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NormalTile : MonoBehaviour {
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

    // Display stepped normal tile
    void OnTriggerEnter2D(Collider2D col) {

        if(col.gameObject == player) {

            sp.sprite = newSprite;

            // Don't consume move on the first load and restarts of the level
            if(gameManager.time > 0.05f)
                gameManager.moves--;
        }
    }

    // Display original normal tile
    void OnTriggerExit2D(Collider2D col) {

        if(col.gameObject == player) {
            sp.sprite = origSprite;
        }
    }
}
