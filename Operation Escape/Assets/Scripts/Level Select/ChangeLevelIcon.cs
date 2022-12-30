using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeLevelIcon : MonoBehaviour {
    [HideInInspector] public int listIndex;
    public Button[] btnLevels;
    public Sprite[] spriteLevelCompleted;
    public Image[] imgButtons;
    LevelCompleteChecker levelCompleteChecker;

    void Start() {

        levelCompleteChecker = GameObject.Find("Level Complete Checker").GetComponent<LevelCompleteChecker>();

        // Change level icons if they are now interactable
        for(int x = 0; x < 11; x++) {
            if(levelCompleteChecker.isLevelCompleted[x] == true) {
                imgButtons[x].sprite = (Sprite)spriteLevelCompleted[x];
                btnLevels[x].interactable = true;
            }
        }
    }
}
