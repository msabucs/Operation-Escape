using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintButton : MonoBehaviour {

    [HideInInspector] public int  nextNumIndex, chosenVariant;
    [HideInInspector] public NumberTile hintedNumTile;
    public int hintsLeft;
    public Button btnHint;
    public Sprite btnHintInactive;
    public List<int> hintNumList0, hintNumList1, hintNumList2;
    GridManager gridManager;
    
    void Start() {

        gridManager = GameObject.Find("Grid Manager").GetComponent<GridManager>();
        chosenVariant = gridManager.randomVariant;        
    }

    void Update() {

        if(hintsLeft <= 0) {

            btnHint.interactable = false;
            btnHint.image.sprite = btnHintInactive;
        }
    }

    public void Hint() {

        if(hintsLeft > 0) {

            if(nextNumIndex < hintNumList0.Count) {
                
                if(chosenVariant == 0) {

                    hintedNumTile = GameObject.Find("Number Tile " +hintNumList0[nextNumIndex]).GetComponent<NumberTile>();
                    StartCoroutine(hintedNumTile.HintedNumber());
                }
                else if(chosenVariant == 1) {

                    hintedNumTile = GameObject.Find("Number Tile " +hintNumList1[nextNumIndex]).GetComponent<NumberTile>();
                    StartCoroutine(hintedNumTile.HintedNumber());
                }
                else {

                    hintedNumTile = GameObject.Find("Number Tile " +hintNumList2[nextNumIndex]).GetComponent<NumberTile>();
                    StartCoroutine(hintedNumTile.HintedNumber());
                }

                hintsLeft--;
                FindObjectOfType<AudioManager>().Play("HintClicked");
            }
            else {
                FindObjectOfType<AudioManager>().Play("WallBump");
            }
        }
    }

    public void RestartClicked() {
        nextNumIndex = 0;
    }
}
