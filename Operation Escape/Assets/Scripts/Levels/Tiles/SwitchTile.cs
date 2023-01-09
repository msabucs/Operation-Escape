using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchTile : MonoBehaviour {
    [HideInInspector] public GameObject player;
    public SpriteRenderer sp;
    public Sprite origSprite, newSprite;
    GameManager gameManager; 

    void Start() { 

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
         
        sp = GetComponent<SpriteRenderer>();
        player = GameObject.Find("Player");
    }

    // Display stepped switch tile
    void OnTriggerEnter2D(Collider2D col) {

        if(col.gameObject == player) {
            
            sp.sprite = newSprite;
            gameManager.currentNumber *= -1;
            gameManager.moves--;
            FindObjectOfType<AudioManager>().Play("SwitchTile");
        }
    }

    // Display original switch tile
    void OnTriggerExit2D(Collider2D col) {
        
        if(col.gameObject == player) {
            sp.sprite = origSprite;
        }
    }
}
