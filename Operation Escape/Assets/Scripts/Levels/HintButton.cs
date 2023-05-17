using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour {

    [HideInInspector] public int nextNum, hintsLeft;
    [HideInInspector] public List<int> hintNumbers;
    [HideInInspector] public NumberTile hintedNumTile;
    public Button btnHint;
    public Sprite btnHintInactive;
    public Queue<int> hintQueue;
    EquationFinder eqFinder;
    
    void Start() {

        eqFinder = GameObject.Find("Game Manager").GetComponent<EquationFinder>();
        hintQueue = new Queue<int>();
        StartCoroutine(GetHintNumbers());
    }

    void Update() {

        if(hintsLeft <= 0) {
            btnHint.interactable = false;
            btnHint.image.sprite = btnHintInactive;
        }
    }

    // Adding a short delay before getting hint numbers
    IEnumerator GetHintNumbers() {

        yield return new WaitForSeconds(0.25f);
        hintNumbers = eqFinder.finalEquation;
        hintsLeft = hintNumbers.Count - 1;

        foreach(int n in hintNumbers)
            hintQueue.Enqueue(n);        
    }

    public void Hint() {

        if(hintsLeft > 0) {

            if(hintQueue.Count != 0) {

                nextNum = hintQueue.Dequeue();
                hintedNumTile = GameObject.Find("Number Tile " +nextNum).GetComponent<NumberTile>();
                StartCoroutine(hintedNumTile.HintedNumber());
                hintsLeft--;
                FindObjectOfType<AudioManager>().Play("HintClicked");
            }
            else {
                FindObjectOfType<AudioManager>().Play("WallBump");
            }
        }
    }
}
