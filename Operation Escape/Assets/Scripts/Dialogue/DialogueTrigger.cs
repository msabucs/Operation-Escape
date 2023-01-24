using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour {
    [HideInInspector] public int sceneIndex;
    public Dialogue dialogue;

    void Start() {
        
        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        if(FindObjectOfType<LevelCompleteChecker>().isRandom == false) {
            
            if(sceneIndex != 12)
                StartDialogue();
        }
    }

    public void StartDialogue() {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }
}
