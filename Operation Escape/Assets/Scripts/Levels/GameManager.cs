using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    [HideInInspector] public Vector3 initialPlayerPos, initialPlayerMPPos;
    [HideInInspector] public int goalNumber, initialMoves, sceneIndex, randomLevel, randomVariant;
    [HideInInspector] public float currentNumber, initialNumber;
    [HideInInspector] public bool isEqual, animDone, isRestartPressed, isGamePaused;
    [HideInInspector] public Text txtCurrentNumber, txtGoal, txtGoalNumber, txtMoves, txtMovesNumber, txtRoom, txtRoomNumber;
    [HideInInspector] public GameObject panelNewMechanic, panelEscaped;
    public List<int> goalNumberList;
    public List<float> currentNumberList;
    public List<GameObject> tileVariantList;
    public int moves, roomNumber;
    public GameObject player, playerMP, panelPause, panelInGame;
    public Button btnNext;
    Object[] btnNextSprites;
    Image btnNextImage;
    Animator animPanelNewMechanic, animPanelEscaped;
    PlayerMovement playerMovement;
    NumberScreen numScreen;
    DialogueManager dialogueManager;

    void Start() {

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        playerMovement = player.GetComponent<PlayerMovement>();

        numScreen = GameObject.Find("Number Screen").GetComponent<NumberScreen>();

        // Load a random variant for the level
        if(sceneIndex != 2) {
            randomVariant = Random.Range(0,3);
            for(int x = 0; x <= 2; x++) {
                if(x == randomVariant) {
                    currentNumber = currentNumberList[x];
                    goalNumber = goalNumberList[x];
                    tileVariantList[x].SetActive(true);
                }
            }
        }
        
        // Level 0
        if(sceneIndex == 2) {
            dialogueManager = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        }

        // Level 3, 5, 6, 7
        if(sceneIndex == 5 || sceneIndex == 7 || sceneIndex == 8 || sceneIndex == 9){

            // Do not trigger when in random level
            if(FindObjectOfType<LevelCompleteChecker>().isRandom != true) {
                panelNewMechanic = GameObject.Find("PanelNewMechanic");
                animPanelNewMechanic = GameObject.Find("PanelNewMechanic").GetComponent<Animator>();
                animPanelNewMechanic.Play("PanelMechIn");
                //FindObjectOfType<AudioManager>().Play("NewMechanic");
            }
            else {
                panelNewMechanic = GameObject.Find("PanelNewMechanic");
                animPanelNewMechanic = GameObject.Find("PanelNewMechanic").GetComponent<Animator>();
                //animPanelNewMechanic.Play("PanelMechIn");
                panelNewMechanic.SetActive(false);
            }
        }

        // Level 10
        if(sceneIndex == 12) {
            panelEscaped = GameObject.Find("PanelEscaped");
            animPanelEscaped = GameObject.Find("PanelEscaped").GetComponent<Animator>();
            panelEscaped.SetActive(false);
        }

        btnNextSprites = Resources.LoadAll("Sprites/Buttons/HintNextLevelButtons");

        btnNextImage = btnNext.GetComponent<Image>();

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

        GoalNumber();
        MovesLeft();
        CurrentNumber();
    }

    void Update() {

        // Make next level button interactable when level is complete
        if(txtCurrentNumber.text == "ESCAPED") {
            btnNext.interactable = true;
            btnNextImage.sprite = (Sprite)btnNextSprites[3];
        }
        else {
            btnNext.interactable = false;
            btnNextImage.sprite = (Sprite)btnNextSprites[5];
        }

        // Level 0
        if(sceneIndex == 2) {

            // Disable player movement when dialogue box is open
            if(dialogueManager.dialogueCounter > 9 && dialogueManager.dialogueCounter != 12 && dialogueManager.dialogueCounter != 15) {
                
                // Trigger when player go to left after stepping on number tile
                if(dialogueManager.dialogueCounter == 13) {

                    if(dialogueManager.isLeft == false) {
                        playerMovement.enabled = true;
                    }
                    else {
                        playerMovement.enabled = false;
                    }
                }      
            }
            else {
                if(animDone == false)
                    playerMovement.enabled = false;
            }
        }

        // Level 3, 5, 6, 7
        if(sceneIndex == 5 || sceneIndex == 7 || sceneIndex == 8 || sceneIndex == 9) {

            // Do not trigger tutorial when in random level
            if(FindObjectOfType<LevelCompleteChecker>().isRandom != true) {

                if(panelNewMechanic.activeInHierarchy) {
                    playerMovement.enabled = false;
                }
                else {
                    playerMovement.enabled = true;
                }
            }
        }
        
        // Check if player animation stops playing
        animDone = playerMovement.animDone;

        MovesLeft();
        CurrentNumber();
    }

    // Change the goal number
    void GoalNumber() {
        txtGoalNumber.text = goalNumber.ToString();
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

            if(animDone == false)
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

        // Reset player position
        player.transform.position = initialPlayerPos;
        playerMP.transform.position = initialPlayerMPPos;

        if(moves == initialMoves || moves == initialMoves + 1) {
            moves = initialMoves;
        }
        else {
            moves = initialMoves + 1;
        }

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
        if(FindObjectOfType<LevelCompleteChecker>().isRandom == true) {

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

            FindObjectOfType<AudioManager>().Play("ButtonClick");
        }
        else {

            // All levels except level 1o
            if(!(sceneIndex == 12)) {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                FindObjectOfType<AudioManager>().Play("ButtonClick");
            }
            else {
                panelEscaped.SetActive(true);
                animPanelEscaped.Play("Congrats");
            }
        }
    }

    // Go to main menu
    public void HomeClicked() {

        SceneManager.LoadScene(0);
        FindObjectOfType<LevelCompleteChecker>().isRandom = false;
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    // Display new gameplay mechanic
    public void PanelMechanicClicked() {

        //Level 3, 5, 6
        if(sceneIndex == 5 || sceneIndex == 7 || sceneIndex == 8) {
            
            // Do not trigger when in random level
            if(FindObjectOfType<LevelCompleteChecker>().isRandom != true) {

                panelNewMechanic.SetActive(false);
                FindObjectOfType<AudioManager>().Play("ButtonClick");
            }
        }
        //Level 7
        else if(sceneIndex == 9) {

            // Do not trigger when in random level
            if(FindObjectOfType<LevelCompleteChecker>().isRandom != true) {

                GameObject panelMechanic = GameObject.Find("PanelMechanic");
                GameObject panelMechanic1 = GameObject.Find("PanelMechanic (1)");
                panelMechanic.SetActive(false);
                animPanelNewMechanic.Play("PanelMech1In");
                FindObjectOfType<AudioManager>().Play("ButtonClick");
            }    
        }
    }

    // Display another gameplay mechanic
    public void PanelMechanic1Clicked() {

        //Level 7
        if(sceneIndex == 9) {
            panelNewMechanic.SetActive(false);
            FindObjectOfType<AudioManager>().Play("ButtonClick");
        }
    }

    // Go to main menu after completing level 10
    public void BackToMainMenuClicked() {
        SceneManager.LoadScene(0);
    }
}