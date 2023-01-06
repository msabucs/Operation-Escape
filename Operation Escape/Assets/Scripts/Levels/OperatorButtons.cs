using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OperatorButtons : MonoBehaviour {
    [HideInInspector] public int opNumber;
    public Button buttonAdd, buttonSub, buttonMulti, buttonDiv;
    public bool isAddDefault, isSubDefault, isMultiDefault, isDivDefault;
    GameManager gameManager;

    void Start() {

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        
        // Set level to addition mode
        if(isAddDefault) {

            opNumber = 1;
            buttonAdd.interactable = false;
            StartCoroutine(DefaultOperator(opNumber));
        }
        // Set level to subtraction mode
        else if(isSubDefault) {

            opNumber = 2;
            buttonSub.interactable = false;
            StartCoroutine(DefaultOperator(opNumber));
        }
        // Set level to multiplication mode
        else if(isMultiDefault) {

            opNumber = 3;
            buttonMulti.interactable = false;
            StartCoroutine(DefaultOperator(opNumber));
        }
        // Set level to division mode
        else if(isDivDefault) {

            opNumber = 4;
            buttonDiv.interactable = false;
            StartCoroutine(DefaultOperator(opNumber));
        }
    }
    
    void Update() {

        if(gameManager.moves <= 0 || gameManager.isEqual) {

            buttonAdd.interactable = false;
            buttonSub.interactable = false;
            buttonMulti.interactable = false;
            buttonDiv.interactable = false;
        }

    }

    // Adding a short delay before getting current operator
    IEnumerator DefaultOperator(int opNumber) {

        yield return new WaitForSeconds(0.25f);
        FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
    }

    // Set level to addition mode
    public void AddMode() {

        opNumber = 1;
        buttonAdd.interactable = false;
        buttonSub.interactable = true;
        buttonMulti.interactable = true;
        buttonDiv.interactable = true;

        FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        gameManager.moves--;

        if(gameManager.moves < 0) {
            gameManager.moves = 0;
        }

        FindObjectOfType<AudioManager>().Play("AddMode");
    }

    // Set level to subtraction mode
    public void SubMode() {

        opNumber = 2;
        buttonAdd.interactable = true;
        buttonSub.interactable = false;
        buttonMulti.interactable = true;
        buttonDiv.interactable = true;

        FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        gameManager.moves--;

        if(gameManager.moves < 0) {
            gameManager.moves = 0;
        }

        FindObjectOfType<AudioManager>().Play("SubMode");
    }

    // Set level to multiplication mode
    public void MultiMode() {

        opNumber = 3;
        buttonAdd.interactable = true;
        buttonSub.interactable = true;
        buttonMulti.interactable = false;
        buttonDiv.interactable = true;

        FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        gameManager.moves--;

        if(gameManager.moves < 0) {
            gameManager.moves = 0;
        }
        
        FindObjectOfType<AudioManager>().Play("MultiMode");
    }

    // Set level to division mode
    public void DivMode() {

        opNumber = 4;
        buttonAdd.interactable = true;
        buttonSub.interactable = true;
        buttonMulti.interactable = true;
        buttonDiv.interactable = false;

        FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        gameManager.moves--;

        if(gameManager.moves < 0) {
            gameManager.moves = 0;
        }

        FindObjectOfType<AudioManager>().Play("DivMode");
    }

    // Reset operator to default when level restarts
    public void RestartClicked() {

        if(isAddDefault) {
            opNumber = 1;
            buttonAdd.interactable = false;
            buttonSub.interactable = true;
            buttonMulti.interactable = true;
            buttonDiv.interactable = true;
            FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        }
        else if(isSubDefault) {
            opNumber = 2;
            buttonAdd.interactable = true;
            buttonSub.interactable = false;
            buttonMulti.interactable = true;
            buttonDiv.interactable = true;
            FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        }
        else if(isMultiDefault) {
            opNumber = 3;
            buttonAdd.interactable = true;
            buttonSub.interactable = true;
            buttonMulti.interactable = false;
            buttonDiv.interactable = true;
            FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        }
        else if(isDivDefault) {
            opNumber = 4;
            buttonAdd.interactable = true;
            buttonSub.interactable = true;
            buttonMulti.interactable = true;
            buttonDiv.interactable = false;
            FindObjectOfType<NumberTile>().CurrentOperator(opNumber);
        }
    }
}
