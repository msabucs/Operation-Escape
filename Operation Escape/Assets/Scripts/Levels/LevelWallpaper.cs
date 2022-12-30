using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelWallpaper : MonoBehaviour {
    [HideInInspector] public int sceneIndex, currentOp, helpOp = 0;
    [HideInInspector] public bool isAdd, isSub, isMulti, isDiv;
    public SpriteRenderer sp;
    public Animator animator;
    public Button[] btnMainMenu;
    Object[] wallpaperSprites;
    OperatorButtons opButtons;
    
    void Start() {

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        sp = GetComponent<SpriteRenderer>();
        wallpaperSprites = Resources.LoadAll("Sprites/LevelWallpaper");

        // Main Menu
        if(sceneIndex == 0) {

            animator.enabled = false;
            sp.sprite = (Sprite)wallpaperSprites[29];

            btnMainMenu[0].onClick.AddListener(AddClicked);
            btnMainMenu[1].onClick.AddListener(SubClicked);
            btnMainMenu[2].onClick.AddListener(MultiClicked);
            btnMainMenu[3].onClick.AddListener(DivClicked);
            btnMainMenu[4].onClick.AddListener(BackToSettingsClicked);
        }

        // Level Select
        else if(sceneIndex == 1) {

            animator.enabled = false;
            sp.sprite = (Sprite)wallpaperSprites[29];
        }

        // Levels
        else {

            opButtons = GameObject.Find("Game Manager").GetComponent<OperatorButtons>();

            isAdd = opButtons.isAddDefault;
            isSub = opButtons.isSubDefault;
            isMulti = opButtons.isMultiDefault;
            isDiv = opButtons.isDivDefault;

            // Addition by default
            if(isAdd) {
                animator.Play("DefaultToAdd");
            }
            // Subtraction by default
            else if(isSub) {
                animator.Play("DefaultToSub");
            }
            // Multiplication by default
            else if(isMulti) {
                animator.Play("DefaultToMulti");
            }
            // Division by default
            else if(isDiv) {
                animator.Play("DefaultToDiv");
            }
        }
    }

    void Update() { 

        // Main Menu and Level Select
        if(sceneIndex == 0 || sceneIndex == 1) {
            return;
        }

        // Levels
        else {

            currentOp = opButtons.opNumber;

            // Addition to Subtraction
            if(isAdd && currentOp == 2) {
                animator.enabled = true;
                animator.Play("AddToSub");
                isAdd = false;
                isSub = true;
            }
            // Addition to Multiplication
            if(isAdd && currentOp == 3) {
                animator.enabled = true;
                animator.Play("AddToMulti");
                isAdd = false;
                isMulti = true;
            }
            // Addition to Division
            if(isAdd && currentOp == 4) {
                animator.enabled = true;
                animator.Play("AddToDiv");
                isAdd = false;
                isDiv = true;
            }
            // Subtraction to Addition
            if(isSub && currentOp == 1) {
                animator.enabled = true;
                animator.Play("SubToAdd");
                isSub = false;
                isAdd = true;
            }
            // Subtraction to Multipliction
            if(isSub && currentOp == 3) {
                animator.enabled = true;
                animator.Play("SubToMulti");
                isSub = false;
                isMulti = true;
            }
            // Subtraction to Division
            if(isSub && currentOp == 4) {
                animator.enabled = true;
                animator.Play("SubToDiv");
                isSub = false;
                isDiv = true;
            }
            // Multiplication to Addition
            if(isMulti && currentOp == 1) {
                animator.enabled = true;
                animator.Play("MultiToAdd");
                isMulti = false;
                isAdd = true;
            }
            // Multiplication to Subtraction
            if(isMulti && currentOp == 2) {
                animator.enabled = true;
                animator.Play("MultiToSub");
                isMulti = false;
                isSub = true;
            }
            // Multiplication to Division
            if(isMulti && currentOp == 4) {
                animator.enabled = true;
                animator.Play("MultiToDiv");
                isMulti = false;
                isDiv = true;
            }
            // Division to Addition
            if(isDiv && currentOp == 1) {
                animator.enabled = true;
                animator.Play("DivToAdd");
                isDiv = false;
                isAdd = true;
            }
            // Division to Subtraction
            if(isDiv && currentOp == 2) {
                animator.enabled = true;
                animator.Play("DivToSub");
                isDiv = false;
                isSub = true;
            }
            // Division to Multiplication
            if(isDiv && currentOp == 3) {
                animator.enabled = true;
                animator.Play("DivToMulti");
                isDiv = false;
                isMulti = true;
            }
        }
    }

    // Change wallpaper when paused
    public void PauseClicked() {

        if(isAdd) {
            animator.enabled = true;
            animator.Play("AddToDefault");
        }
        else if(isSub) {
            animator.enabled = true;
            animator.Play("SubToDefault");
        }
        else if(isMulti) {
            animator.enabled = true;
            animator.Play("MultiToDefault");
        }
        else if(isDiv) {
            animator.enabled = true;
            animator.Play("DivToDefault");
        }
    }

    // Change wallpaper when resumed
    public void ResumeClicked() {

        if(isAdd) {
            animator.enabled = true;
            animator.Play("DefaultToAdd");
        }
        else if(isSub) {
            animator.enabled = true;
            animator.Play("DefaultToSub");
        }
        else if(isMulti) {
            animator.enabled = true;
            animator.Play("DefaultToMulti");
        }
        else if(isDiv) {
            animator.enabled = true;
            animator.Play("DefaultToDiv");
        }
    }

    // HELP OPERATOR BUTTON FOR MAIN MENU
    void AddClicked() {

        if(helpOp == 0) {
            animator.enabled = true;
            animator.Play("DefaultToAdd");
        }
        else if(helpOp == 2) {
            animator.enabled = true;
            animator.Play("SubToAdd");
        }
        else if(helpOp == 3) {
            animator.enabled = true;
            animator.Play("MultiToAdd");
        }
        else if(helpOp == 4) {
            animator.enabled = true;
            animator.Play("DivToAdd");
        }

        helpOp = 1;
    }

    void SubClicked() {

        if(helpOp == 0) {
            animator.enabled = true;
            animator.Play("DefaultToSub");
        }
        else if(helpOp == 1) {
            animator.enabled = true;
            animator.Play("AddToSub");
        }
        else if(helpOp == 3) {
            animator.enabled = true;
            animator.Play("MultiToSub");
        }
        else if(helpOp == 4) {
            animator.enabled = true;
            animator.Play("DivToSub");
        }

        helpOp = 2;
    }

    void MultiClicked() {

        if(helpOp == 0) {
            animator.enabled = true;
            animator.Play("DefaultToMulti");
        }
        else if(helpOp == 1) {
            animator.enabled = true;
            animator.Play("AddToMulti");
        }
        else if(helpOp == 2) {
            animator.enabled = true;
            animator.Play("SubToMulti");
        }
        else if(helpOp == 4) {
            animator.enabled = true;
            animator.Play("DivToMulti");
        }

        helpOp = 3;
    }

    void DivClicked() {

        if(helpOp == 0) {
            animator.enabled = true;
            animator.Play("DefaultToDiv");
        }
        else if(helpOp == 1) {
            animator.enabled = true;
            animator.Play("AddToDiv");
        }
        else if(helpOp == 2) {
            animator.enabled = true;
            animator.Play("SubToDiv");
        }
        else if(helpOp == 3) {
            animator.enabled = true;
            animator.Play("MultiToDiv");
        }

        helpOp = 4;
    }

    void BackToSettingsClicked() {

        if(helpOp == 1) {
            animator.enabled = true;
            animator.Play("AddToDefault");
        }
        else if(helpOp == 2) {
            animator.enabled = true;
            animator.Play("SubToDefault");
        }
        else if(helpOp == 3) {
            animator.enabled = true;
            animator.Play("MultiToDefault");
        }
        else if(helpOp == 4) {
            animator.enabled = true;
            animator.Play("DivToDefault");
        }

        helpOp = 0;
    }
}
