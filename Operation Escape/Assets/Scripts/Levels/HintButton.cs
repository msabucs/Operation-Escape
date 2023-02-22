using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour {

    [HideInInspector] public int nextNum, chosenVariant;
    [HideInInspector] public NumberTile hintedNumTile;
    public int hintsLeft;
    public Button btnHint;
    public Sprite btnHintInactive;
    public List<int> hintNumList0, hintNumList1, hintNumList2;
    public Queue<int> hintQueue;
    GridManager gridManager;
    
    void Start() {

        gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
        chosenVariant = gridManager.randomVariant;
        hintQueue = new Queue<int>();

        if(chosenVariant == 0) {

            foreach(int n in hintNumList0) {
                hintQueue.Enqueue(n);
            }
        }
        else if(chosenVariant == 1)  {

            foreach(int n in hintNumList1) {
                hintQueue.Enqueue(n);
            }
        }
        else if(chosenVariant == 2)  {

            foreach(int n in hintNumList2) {
                hintQueue.Enqueue(n);
            }
        }
    }

    void Update() {

        if(hintsLeft <= 0) {

            btnHint.interactable = false;
            btnHint.image.sprite = btnHintInactive;
        }
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
