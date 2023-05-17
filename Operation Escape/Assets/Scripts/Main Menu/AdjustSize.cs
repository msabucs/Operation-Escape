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
        //Debug.Log(aspectRatio);

        if(aspectRatio >= 2.222f) {

            //Debug.Log("9:20");
            this.transform.localScale = new Vector3(0.8f, 1, 1);
        }
        else if(aspectRatio >= 2.166f) {

            //Debug.Log("9:19.5");
            this.transform.localScale = new Vector3(0.83f, 1, 1);
        }
        else if(aspectRatio >= 2.115f) {

            //Debug.Log("9:19");
            this.transform.localScale = new Vector3(0.85f, 1, 1);
        }
        /*else if(aspectRatio >= 2.074f) {

            Debug.Log("9:18.7");
            this.transform.localScale = new Vector3(0.87f, 1, 1);
        }*/
        else if(aspectRatio >= 2.055f) {

            //Debug.Log("9:18.5");
            this.transform.localScale = new Vector3(0.87f, 1, 1);
        }
        else if(aspectRatio >= 2.003f) {

            //Debug.Log("9:18");
            this.transform.localScale = new Vector3(0.9f, 1, 1);            
        }
        else if(aspectRatio >= 1.903f) {

            //Debug.Log("10:19");
            this.transform.localScale = new Vector3(0.95f, 1, 1);            
        }
        else if (aspectRatio >= 1.777f){

            //Debug.Log("9:16");
        }
        else if (aspectRatio >= 1.668f){

            //Debug.Log("3:5");
            this.transform.localScale = new Vector3(1.07f, 1, 1);
        }
        else if(aspectRatio >= 1.602f) {

            //Debug.Log("10:16");
            this.transform.localScale = new Vector3(1.13f, 1, 1);
        }
        else if(aspectRatio >= 1.501f) {

            Debug.Log("2:3");
            this.transform.localScale = new Vector3(1.2f, 1, 1);
        }
        else if(aspectRatio >= 1.333f) {

            Debug.Log("3:4");
            this.transform.localScale = new Vector3(1.35f, 1, 1);
        }
    }
}
