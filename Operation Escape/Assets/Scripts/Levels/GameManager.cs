using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [HideInInspector] public Vector3 initialPlayerPos, initialPlayerMPPos;
    [HideInInspector] public int goalNumber, initialMoves, sceneIndex, randomLevel, chosenVariant;
    [HideInInspector] public float currentNumber, initialNumber, startTime, elapsedTime, time;
    [HideInInspector] public bool isEqual, isRestartPressed, isGamePaused;
    [HideInInspector] public Text txtCurrentNumber, txtGoal, txtGoalNumber, txtMoves, txtMovesNumber, txtRoom, txtRoomNumber;
    public List<int> goalNumberList;
    public List<float> currentNumberList;
    public int moves, roomNumber;
    public GameObject player, playerMP, panelPause, panelInGame;
    public Button btnNext;
    public Animator animDialogue;
    Object[] btnNextSprites;
    Image imgBtnNext;
    PlayerMovement playerMovement;
    NumberScreen numScreen;
    GridManager gridManager;

    void Start() {

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;       

        numScreen = GameObject.Find("Number Screen").GetComponent<NumberScreen>();
        playerMovement = player.GetComponent<PlayerMovement>();
        
        // Change level objective depending on the level variant
        if(sceneIndex != 2) {

            gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
            chosenVariant = gridManager.randomVariant;
            
            for(int x = 0; x <= 2; x++) {
                if(x == chosenVariant) {
                    currentNumber = currentNumberList[x];
                    goalNumber = goalNumberList[x];
                }
            }
        }

        btnNextSprites = Resources.LoadAll("Sprites/Buttons/HintNextLevelButtons");

        imgBtnNext = btnNext.GetComponent<Image>();

        txtCurrentNumber = GameObject.Find("TextCurrentNumber").GetComponent<Text>();
        txtGoal = GameObject.Find("TextGoal").GetComponent<Text>();
        txtGoalNumber = GameObject.Find("TextGoalNumber").GetComponent<Text>();
        txtMoves = GameObject.Find("TextMoves").GetComponent<Text>();
        txtMovesNumber = GameObject.Find("TextMovesNumber").GetComponent<Text>();
        txtRoom = GameObject.Find("TextRoom").GetComponent<Text>();
        txtRoomNumber = GameObject.Find("TextRoomNumber").GetComponent<Text>();

        // Get the initial position of the player and move point
        initialPlayerPos = player.transform.position;
        initialPlayerMPPos = playerMP.transform.position;

        // Get the initial number of moves and number
        initialMoves = moves;
        initialNumber = currentNumber;

        txtGoalNumber.text = goalNumber.ToString();

        if(GameObject.Find("DialogueBox") != null) {

            if(FindObjectOfType<LevelCompleteChecker>().isRandom == false && sceneIndex != 12)
                animDialogue.enabled = true;
        }

        MovesLeft();
        CurrentNumber();
    }

    void Update() {

        // Innate game timer
        time += Time.deltaTime;

        // Make next level button interactable when level is complete
        if(txtCurrentNumber.text == "ESCAPED") {
            btnNext.interactable = true;
            imgBtnNext.sprite = (Sprite)btnNextSprites[3];
        }
        else {
            btnNext.interactable = false;
            imgBtnNext.sprite = (Sprite)btnNextSprites[5];
        }

        MovesLeft();
        CurrentNumber();
    }

    // Change the current number
    void CurrentNumber() {

        txtCurrentNumber.text = currentNumber.ToString();

        // Change current number to paused and disable player movement
        if(isGamePaused) {
            txtCurrentNumber.text = "PAUSED";
            playerMovement.enabled = false;
        }
        else {
            txtCurrentNumber.text = currentNumber.ToString();
            playerMovement.enabled = true;
        }
        
        // Change current number when escaped or trapped
        if(isEqual){
            txtCurrentNumber.text = "ESCAPED";
        }
        else if(moves <= 0) {

            if(isEqual) {
                txtCurrentNumber.text = "ESCAPED";
            } else {
                txtCurrentNumber.text = "TRAPPED";
            }
        }
    }

    // Change the moves number
    void MovesLeft() {

        txtMovesNumber.text = moves.ToString();
        
        // Disable player movement when escaped
        if(moves <= 0 || isEqual) {

            if(playerMovement.isMoving == false)
                playerMovement.enabled = false;
            
            if(sceneIndex == 2)
                FindObjectOfType<LevelCompleteChecker>().isTutorialDone = true;

            FindObjectOfType<LevelCompleteChecker>().LevelChecker();
        }

        // Reset moves number and enable player movement when level restarts
        if(isRestartPressed) {
            playerMovement.enabled = true;

            if(moves <= 0 || isEqual) {
                isRestartPressed = false;
            }
        }
    }

    // Pause level
    public void PauseClicked() {

        isGamePaused = true;

        panelInGame.SetActive(false);
        panelPause.SetActive(true);

        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    // Resume level
    public void ResumeClicked() {

        isGamePaused = false;

        panelInGame.SetActive(true);
        panelPause.SetActive(false);

        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    // Restart level
    public void RestartClicked() {

        // Reset game timer to 0
        time = 0;

        // Reset player position
        player.transform.position = initialPlayerPos;
        playerMP.transform.position = initialPlayerMPPos;

        moves = initialMoves;        
        
        isEqual = false;
        isRestartPressed = true;

        currentNumber = initialNumber;
        txtCurrentNumber.text = currentNumber.ToString();

        numScreen.isDone = false;

        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    // Go to next level
    public void NextLevel() {

        // Random next level if you select random levels
        if(FindObjectOfType<LevelCompleteChecker>().isRandom) {

            randomLevel = Random.Range(1, 11);
            
            // Reroll if random level is same with current levels
            if(randomLevel == roomNumber) {

                while(randomLevel == roomNumber) {
                    randomLevel = Random.Range(1, 11);
                }
                SceneManager.LoadScene("Level " +randomLevel);              
            }
            else {
                SceneManager.LoadScene("Level " +randomLevel);
            }
        }
        else {

            // All levels except level 1o
            if(!(sceneIndex == 12))
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            else {

                animDialogue.enabled = true;
                FindObjectOfType<DialogueTrigger>().StartDialogue();
            }
        }

        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    // Go to main menu
    public void HomeClicked() {

        SceneManager.LoadScene(0);
        FindObjectOfType<LevelCompleteChecker>().isRandom = false;
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    // Go to main menu after completing level 10
    public void BackToMainMenuClicked() {
        SceneManager.LoadScene(0);
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
}
