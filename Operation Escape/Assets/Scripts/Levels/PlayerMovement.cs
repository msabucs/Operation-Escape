using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [HideInInspector] public int sceneIndex, swipeDist = 50;
    [HideInInspector] public bool isMoving, fingerDown;
    [HideInInspector] public float animX, animY, aspectRatio, scaleValue, columnPosValue;
    [HideInInspector] public float[] columnPosList = new float[5];
    [HideInInspector] public Vector2 startTouchPos;
    [HideInInspector] public Vector3 newPosition, initialPos;
    public int columnIndex;
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask obstacleLayer;
    public Animator animator;
    public CircleCollider2D circleCol;
    GameManager gameManager;
    DialogueManager dialogueManager;

    void Start() {

        Adjust();

        Scene scene = SceneManager.GetActiveScene();
        sceneIndex = scene.buildIndex;

        // Level 0
        if(sceneIndex == 2) {
            dialogueManager = GameObject.Find("Dialogue Manager").GetComponent<DialogueManager>();
        }

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        circleCol = GetComponent<CircleCollider2D>();

        // Separate player and move point
        movePoint.parent = null;

        animator.enabled = false;
    }

    void Adjust() {

        aspectRatio = (float)Screen.height / (float)Screen.width;

        if(aspectRatio >= 2.223f) {

            //Debug.Log("9:20");
            scaleValue = 0.8f;
        }
        else if(aspectRatio >= 2.166f) {

            //Debug.Log("9:19.5");
            scaleValue = 0.83f;
        }
        else if(aspectRatio >= 2.11f) {

            //Debug.Log("9:19");
            scaleValue = 0.85f;
        }
        /*else if(aspectRatio >= 2.074f) {

            Debug.Log("9:18.7");
            scaleValue = 0.87f;
        }*/
        else if(aspectRatio >= 2.055f) {

            //Debug.Log("9:18.5");
            scaleValue = 0.87f;
        }
        else if(aspectRatio >= 2f) {

            //Debug.Log("9:18");
            scaleValue = 0.9f;          
        }
        else if(aspectRatio >= 1.903f) {

            //Debug.Log("10:19");
            scaleValue = 0.95f;      
        }
        else if(aspectRatio >= 1.777f){

            //Debug.Log("9:16");
            scaleValue = 1f;
        }
        else if (aspectRatio >= 1.668f){

            //Debug.Log("3:5");
            scaleValue = 1.07f;
        }
        else if(aspectRatio >= 1.602f) {

            //Debug.Log("10:16");
            scaleValue = 1.13f;
        }
        else if(aspectRatio >= 1.501f) {

            //Debug.Log("2:3");
            scaleValue = 1.2f;
        }
        else if(aspectRatio >= 1.333f) {

            //Debug.Log("3:4");
            scaleValue = 1.35f;
        }

        this.transform.localScale = new Vector3(1.5f * scaleValue, 1.5f, 1);
        columnPosValue -= scaleValue * 2;

        for(int x = 0; x < columnPosList.Length; x++) {
            
            columnPosList[x] = columnPosValue;
            columnPosValue += scaleValue;
        }

        initialPos = new Vector3 (columnPosList[columnIndex], transform.position.y, transform.position.z);
        transform.position = initialPos;
    }

    void Update() {

        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);

        // PC CONTROLLER
        /* Check if distance between player and move point is less than or equal to 0.5
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.5f && !isMoving && gameManager.moves > 0) {

            // Move left and right
            if (Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f ) {                
                StartCoroutine(MovePlayer(new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0)));
            }

            // Move up and down & No diagonal movement
            else if (Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f) {
                StartCoroutine(MovePlayer(new Vector3(0, Input.GetAxisRaw("Vertical"), 0)));
            }
        }*/

        // MOBILE CONTROLLER
        if(fingerDown == false && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began) {

            startTouchPos = Input.touches[0].position;
            startTouchPos = Input.mousePosition;
            fingerDown = true;
        }

        if(fingerDown) {

            if(Input.touches[0].position.y >= startTouchPos.y + swipeDist) {

                fingerDown = false;
                //Debug.Log("Up");
                StartCoroutine(MovePlayer(new Vector3(0, 1, 0)));
            }
            else if(Input.touches[0].position.x <= startTouchPos.x - swipeDist) {

                fingerDown = false;
                //Debug.Log("Left");
                StartCoroutine(MovePlayer(new Vector3(-1, 0, 0)));
            }
            else if(Input.touches[0].position.x >= startTouchPos.x + swipeDist) {

                fingerDown = false;
                //Debug.Log("Right");
                StartCoroutine(MovePlayer(new Vector3(1, 0, 0)));
            }
            else if(Input.touches[0].position.y <= startTouchPos.y - swipeDist) {
                
                fingerDown = false;
                //Debug.Log("Down");
                StartCoroutine(MovePlayer(new Vector3(0, -1, 0)));
            }
        }
        
        if(fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) {
            fingerDown = false;
        }
    }

    IEnumerator MovePlayer(Vector3 direction) {

        isMoving = true;      

        animX = direction.x;
        animY = direction.y;

        if(scaleValue != 1) {

            if(direction.x == 1f) direction.x -= (1 - scaleValue);
            else if(direction.x == -1f) direction.x += (1 - scaleValue);
        }

        newPosition = movePoint.position + direction; 
        
        // Play animation left
        if(animX == -1) {
            animator.enabled = true;
            animator.Play("MoveToLeft");
        }

        // Play animation right
        else if(animX == 1) {
            animator.enabled = true;
            animator.Play("MoveToRight");            
        }

        // Play animation up
        else if(animY == 1) {
            animator.enabled = true;
            animator.Play("MoveUp");
        }

        // Play animation down
        else if(animY == -1) {
            animator.enabled = true;
            animator.Play("MoveDown");
        }

        // Adjust OverlapCircle slightly to the bottom
        Vector3 tempPoint = new Vector3(newPosition.x, newPosition.y - 0.5f, newPosition.z);

        // Check if player has no collision with tiles with obstacleLayer
        if (!Physics2D.OverlapCircle(tempPoint, 0.2f, obstacleLayer)) {
                        
            // Only for level 0 where player can't go to left after stepping on number tile
            if(sceneIndex == 2) {
                if(dialogueManager.dialogueCounter > 12 && animX == -1) {
                    animator.enabled = false;
                    FindObjectOfType<DialogueManager>().DisplayDontGoLeft();
                }
                else {
                    movePoint.position = newPosition;
                    FindObjectOfType<AudioManager>().Play("PlayerStep");
                }
            }
            // Set player position to move point position
            else {
                movePoint.position = newPosition;
                FindObjectOfType<AudioManager>().Play("PlayerStep");
            }
        }
        else {
            FindObjectOfType<AudioManager>().Play("WallBump");
            Debug.Log("Can't go there!");
        }

        // Delay before moving the player
        yield return new WaitForSeconds(0.3f);
        isMoving = false;
    }
    
    // Player face down after level restarts
    public void RestartClicked() {
        animator.Play("FaceDown");
    }
}