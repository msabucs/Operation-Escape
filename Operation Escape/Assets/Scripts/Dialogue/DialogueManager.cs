using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour {
    [HideInInspector] public int sceneIndex, dialogueCounter = 0;
    [HideInInspector] public bool isLeft;
    [HideInInspector] public string sentence;
    [HideInInspector] public Button btnContinue;
    private Queue<string> sentences;
    public Text txtDialogue;
    public Animator animator;
    public PlayerMovement playerMovement;

    void Start() {

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        sentences = new Queue<string>();
        btnContinue = GameObject.Find("ButtonContinue").GetComponent<Button>();
    }

    void Update() {

        // FOR LEVEL 0
        if(sceneIndex == 2) {

            // Disable player movement when dialogue box is open
            if(dialogueCounter > 9 && dialogueCounter != 12 && dialogueCounter != 15) {

                // Trigger when player go to left after stepping on number tile
                if(dialogueCounter == 13) {

                    if(isLeft == false) {
                        playerMovement.enabled = true;
                    }
                    else {
                        playerMovement.enabled = false;
                    }
                }      
            }
            else {
                if(playerMovement.isMoving == false)
                    playerMovement.enabled = false;
            }

            // Open dialogue box when stepping on number tile and goal tile
            if(dialogueCounter == 11 || dialogueCounter == 14) {

                btnContinue.interactable = true;
                animator.SetBool("isOpen", true);
                DisplayNextDialogue();
            }
        }

        // FOR LEVEL 3, 5, 7
        if(sceneIndex == 5 || sceneIndex == 7 ||  sceneIndex == 9) {

            if(animator.GetBool("isOpen")) {
                playerMovement.enabled = false;
            }
            else {
                playerMovement.enabled = true;
            }
        }
    }

    // FOR LEVEL 0
    // Display text when player go to left after stepping on number tile
    public void DisplayDontGoLeft() {

        animator.SetBool("isOpen", true);
        btnContinue.interactable = true;
        isLeft = true;

        sentence = "We already got the number we need. We don't want to step on the <color=#a48b28>Number Tile</color> again.";
        txtDialogue.text = sentence;
        FindObjectOfType<AudioManager>().Play("PlayerDialogue");          
    }

    // Start dialogue
    public void StartDialogue(Dialogue dialogue) {

        animator.SetBool("isOpen", true);
        sentences.Clear();

        foreach (string s in dialogue.sentences) {
            sentences.Enqueue(s);
        }

        DisplayNextDialogue();
    }

    // Display next dialogue
    public void DisplayNextDialogue(){
        
        if(sceneIndex == 2) {

            if(isLeft == true) {

                isLeft = false;
                CloseDialogue();
                return;
            }
        
            dialogueCounter++;
                
            // Close dialogue box when no dialogues left
            // Close dialogue box before player moves
            if (sentences.Count == 0 || dialogueCounter == 10 || dialogueCounter == 13) {

                btnContinue.interactable = false;
                CloseDialogue();
                return;
            }
        }
        
        if(sentences.Count != 0) {

            sentence = sentences.Dequeue();
            txtDialogue.text = sentence;
            FindObjectOfType<AudioManager>().Play("PlayerDialogue");
        }
        else {
            CloseDialogue();
        }
    }

    // Close dialogue box
    void CloseDialogue() {

        // On level 10, go to main menu when dialogue closes
        if(sceneIndex == 12)
            SceneManager.LoadScene("Main Menu");
        else
            animator.SetBool("isOpen", false);
    }
}
