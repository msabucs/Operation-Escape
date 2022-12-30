using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {
    [HideInInspector] public int randomLevel;
    public int level;
    
    // Load selected level
    public void GotoLevel() {
        
        SceneManager.LoadScene("Level " +level);
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }

    public void RandomLevel() {

        randomLevel = Random.Range(1, 11);
        SceneManager.LoadScene("Level " +randomLevel);
        FindObjectOfType<LevelCompleteChecker>().isRandom = true;
        FindObjectOfType<AudioManager>().Play("ButtonClick");
    }
}