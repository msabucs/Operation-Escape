using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteChecker : MonoBehaviour {
    [HideInInspector] public int index, sceneIndex;
    [HideInInspector] public bool[] isLevelCompleted;
    [HideInInspector] public bool isRandom, isTutorialDone;
    [HideInInspector] public GameObject game;
    public static LevelCompleteChecker instance;
    GameManager gameManager;

    void Start() {

        isLevelCompleted  = new bool[11]; 
    }

    void Update() { 

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        game = GameObject.Find("Game Manager");

        if(game != null) {
            gameManager = game.GetComponent<GameManager>();
        }
        else {
            return;
        }  
    }

    void Awake() {

        // Check if there is only one LevelCompleteChecker on the scene
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    // Check if level is completed
    public void LevelChecker() {

        if(sceneIndex >= 2) {
            index = sceneIndex - 2;
            isLevelCompleted[index] = true;
        }
    }
}
