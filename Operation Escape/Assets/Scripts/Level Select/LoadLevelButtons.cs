using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevelButtons : MonoBehaviour {
    
    // Go to main menu from level select
    public void GoToMainMenu() {

        SceneManager.LoadScene(0);
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
}