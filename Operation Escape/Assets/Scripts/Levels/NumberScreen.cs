using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberScreen : MonoBehaviour {
    [HideInInspector] public bool isDone = false;
    public SpriteRenderer sp;
    public Animator animator;
    public Sprite defaultScreen;
    GameManager gameManager;

    void Start() {

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        animator.enabled = false;
    }

    void Update() {

        // Play when level escaped
        if(gameManager.txtCurrentNumber.text == "ESCAPED") {
            animator.enabled = true;
            animator.Play("WinScreen");

            if(isDone == false){
                isDone = true;
                FindObjectOfType<AudioManager>().Play("Escaped");
            }
        }
        // Play when level trapped
        else if(gameManager.txtCurrentNumber.text == "TRAPPED") {
            animator.enabled = true;
            animator.Play("LoseScreen");
            
            if(isDone == false){
                isDone = true;
                FindObjectOfType<AudioManager>().Play("Trapped");
            }
        }
        else {
            animator.enabled = false;
            sp.sprite = (Sprite)defaultScreen;
        }
    }
}
