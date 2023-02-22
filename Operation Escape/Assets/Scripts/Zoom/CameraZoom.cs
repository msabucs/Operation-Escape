using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour {

    public SpriteRenderer bg;
    
    void Start() {
        
        float screenRatio = (float)Screen.width / (float)Screen.height;
        float targetRatio = bg.bounds.size.x / bg.bounds.size.y;

        if(screenRatio >= targetRatio) {
            Camera.main.orthographicSize = bg.bounds.size.y / 2;
        }
        else {
            
            float diffInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = bg.bounds.size.y / 2 * diffInSize;
        }
    }
    
}
