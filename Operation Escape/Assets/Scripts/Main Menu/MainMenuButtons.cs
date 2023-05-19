using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour {
   public Text txtName, txtInfo;
   public GameObject panelMain, panelTutorial, panelOptions, panelHelp, panelCredits, tiles;
   public Animator animator;
   public Button[] btnHelp;
   [TextArea(3,6)] public string[] arrayName, arrayInfo;
   
   void Start() {

      btnHelp[0].onClick.AddListener(NumberTileClicked);
      btnHelp[1].onClick.AddListener(GoalTileClicked);
      btnHelp[2].onClick.AddListener(SwitchTileClicked);
      btnHelp[3].onClick.AddListener(AddClicked);
      btnHelp[4].onClick.AddListener(SubClicked);
      btnHelp[5].onClick.AddListener(MultiClicked);
      btnHelp[6].onClick.AddListener(DivClicked);
   }
   
   void Update() {

      if(panelMain.activeInHierarchy == true){
         animator.enabled = true;
         animator.Play("OpEscape");
      }
      else {
         animator.enabled = false;
      }
   }
   
   // Go to level 0 or ask player if they want tutorial
   public void GoToPlay() {

      if(FindObjectOfType<LevelCompleteChecker>().isTutorialDone == true) {
         panelMain.SetActive(false);
         panelTutorial.SetActive(true);
      } 
      else {
         SceneManager.LoadScene(2);
      }
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Go to level select
   public void GoToLoadLevel() {

      SceneManager.LoadScene(1);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Exit game
   public void ExitGame() {

      Application.Quit();
      //UnityEditor.EditorApplication.isPlaying = false;
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }
   
   // Go to options
   public void GoToOptions() {

      panelMain.SetActive(false);
      panelOptions.SetActive(true);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Go to credits from options
   public void GoToCredits() {

      panelCredits.SetActive(true);
      panelOptions.SetActive(false);
      tiles.SetActive(false);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Go to help from options
   public void GoToHelp() {

      panelHelp.SetActive(true);
      panelOptions.SetActive(false);
      tiles.SetActive(false);
      txtName.text = "";
      txtInfo.text = "Tap the tiles\nor operators\n";
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Go to tutorial again if yes
   public void TutorialToLevel0() {

      SceneManager.LoadScene(2);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Go to level 1 if no
   public void TutorialToLevel1() {

      SceneManager.LoadScene(3);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Go to main menu from tutorial
   public void TutorialToMain() {

      panelMain.SetActive(true);
      panelTutorial.SetActive(false);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }
   
   // Go to main menu from options
   public void OptionsToMain() {

      panelMain.SetActive(true);
      panelOptions.SetActive(false);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }


   // Go to options from help
   public void CreditsToOptions() {

      panelCredits.SetActive(false);
      panelOptions.SetActive(true);
      tiles.SetActive(true);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // Go to options from help
   public void HelpToOptions() {

      panelHelp.SetActive(false);
      panelOptions.SetActive(true);
      tiles.SetActive(true);
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   // HELP BUTTONS INFO
   public void NumberTileClicked() {

      txtName.text = arrayName[0];
      txtInfo.text = arrayInfo[0];
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   public void GoalTileClicked() {

      txtName.text = arrayName[1];
      txtInfo.text = arrayInfo[1];
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   public void SwitchTileClicked() {

      txtName.text = arrayName[2];
      txtInfo.text = arrayInfo[2];
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   public void AddClicked() {

      txtName.text = arrayName[3];
      txtInfo.text = arrayInfo[3];
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   public void SubClicked() {

      txtName.text = arrayName[4];
      txtInfo.text = arrayInfo[4];
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   public void MultiClicked() {

      txtName.text = arrayName[5];
      txtInfo.text = arrayInfo[5];
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }

   public void DivClicked() {

      txtName.text = arrayName[6];
      txtInfo.text = arrayInfo[6];
      FindObjectOfType<AudioManager>().Play("ButtonClick");
   }
}

