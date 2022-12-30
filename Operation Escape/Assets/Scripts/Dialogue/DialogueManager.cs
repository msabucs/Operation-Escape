using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
    [HideInInspector] public int dialogueCounter = 0;
    [HideInInspector] public bool isLeft;
    [HideInInspector] public string sentence;
    [HideInInspector] public Button btnContinue;
    private Queue<string> sentences;
    public Text txtDialogue;
    public Animator animator;

    void Start() {

        sentences = new Queue<string>();
        btnContinue = GameObject.Find("ButtonContinue").GetComponent<Button>();
    }

    void Update() {

        // Open dialogue box when stepping on number tile and goal tile
        if(dialogueCounter == 11 || dialogueCounter == 14) {

            btnContinue.interactable = true;
            animator.SetBool("isOpen", true);
            DisplayNextDialogue();
        }
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

    // Display text when player go to left after stepping on number tile
    public void DisplayDontGoLeft() {

        animator.SetBool("isOpen", true);
        btnContinue.interactable = true;
        isLeft = true;

        sentence = "We already got the number we need. We don't want to step on the number tile again.";
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));       
    }

    // Display next dialogue
    void DisplayNextDialogue(){
        
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
        
        sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));     
    }

    // Display text on the dialogue box
    IEnumerator TypeSentence(string sentence) {

        txtDialogue.text = "";

        // Display char of text one by one
        foreach (char l in sentence.ToCharArray()) {
            txtDialogue.text += l;
            yield return new WaitForSeconds(0.03f);
            FindObjectOfType<AudioManager>().Play("PlayerDialogue");  
        } 
    }

    // Close dialogue box
    void CloseDialogue() {
        animator.SetBool("isOpen", false);
    }
}
