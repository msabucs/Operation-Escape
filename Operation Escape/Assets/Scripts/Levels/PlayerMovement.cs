using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {
    [HideInInspector] public int sceneIndex, swipeDist = 300;
    [HideInInspector] public bool isMoving, fingerDown;
    [HideInInspector] public float animX, animY;
    [HideInInspector] public Vector2 startTouchPos;
    public float moveSpeed = 5f;
    public Transform movePoint;
    public LayerMask obstacleLayer;
    public Animator animator;
    public CircleCollider2D circleCol;
    GameManager gameManager;
    DialogueManager dialogueManager;

    void Start() {

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
            fingerDown = true;
        }

        if(fingerDown) {

            // Move left
            if(Input.touches[0].position.x <= startTouchPos.x - swipeDist) {

                fingerDown = false;
                StartCoroutine(MovePlayer(new Vector3(-1, 0, 0)));
            }
            // Move right
            else if(Input.touches[0].position.x >= startTouchPos.x + swipeDist) {

                fingerDown = false;
                StartCoroutine(MovePlayer(new Vector3(1, 0, 0)));
            }
            // Move down
            else if(Input.touches[0].position.y <= startTouchPos.y  - swipeDist) {

                fingerDown = false;
                StartCoroutine(MovePlayer(new Vector3(0, -1, 0)));
            }
            // Move up
            else if(Input.touches[0].position.y >= startTouchPos.y + swipeDist) {
                
                fingerDown = false;
                StartCoroutine(MovePlayer(new Vector3(0, 1, 0)));
            }
        }
        
        if(fingerDown && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Ended) {
            fingerDown = false;
        }
    }

    IEnumerator MovePlayer(Vector3 direction) {

        isMoving = true;
        Vector3 newPosition = movePoint.position + direction;

        animX = direction.x;
        animY = direction.y;
        
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