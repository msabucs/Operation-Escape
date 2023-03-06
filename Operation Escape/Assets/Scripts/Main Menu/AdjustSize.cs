using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustSize : MonoBehaviour {

    [HideInInspector] public float aspectRatio;
    
    void Start() {

        Adjust();
    }
    void Adjust() {

        aspectRatio = (float)Screen.height / (float)Screen.width;

        if(aspectRatio >= 2.11f) {

            Debug.Log("9:19");
            this.transform.localScale = new Vector3(0.85f, 1, 1);
        }
        else if(aspectRatio >= 2.055f) {

            Debug.Log("9:18.5");
            this.transform.localScale = new Vector3(0.87f, 1, 1);
        }
        else if(aspectRatio >= 2f) {

            Debug.Log("9:18");
            this.transform.localScale = new Vector3(0.9f, 1, 1);            
        }
        else {
            Debug.Log("9:16");
        }
    }
}
