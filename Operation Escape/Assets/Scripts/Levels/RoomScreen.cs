using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomScreen : MonoBehaviour {
    [HideInInspector] public bool isDone = false;
    [HideInInspector] public Image imgScreen;
    [HideInInspector] public Animator animator;
    public Sprite defaultScreen;
    GameManager gameManager;

    void Start() {

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        imgScreen = GetComponent<Image>();
        animator = GetComponent<Animator>();
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
            imgScreen.sprite = (Sprite)defaultScreen;
        }
    }
}
