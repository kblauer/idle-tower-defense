using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour {
    [SerializeField] private GameObject path;
    [SerializeField] private float moveSpeed = 2f;

    private Animator animator;
    private SpriteRenderer sprite;
    private Rigidbody2D rigid2D;

    public bool controlled = false; 
    private List<Vector3> pathToFollow = new List<Vector3>();
    private int currentPathIndex = -1;
    private Vector3 lastMove = new Vector3();

    private enum MovementState {idle, running, attacking}
    private MovementState movementState = MovementState.idle;

    // Start is called before the first frame update
    void Start() {
        // add children of path to the pathToFollow
        foreach (Transform pathObject in path.GetComponentInChildren<Transform>()) {
            //Debug.Log(pathObject.position);
            pathToFollow.Add(pathObject.position);
        }

        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
        rigid2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void FixedUpdate() {
        if (controlled) {
            moveAlongPath();
        }
    }

    private void moveAlongPath() {
        // if path index is -1, place object at first point
        if (currentPathIndex == -1) {
            transform.position = pathToFollow[0];
            currentPathIndex = 0;
        } else {

            // if we have no more waypoints, stop the movement
            if (currentPathIndex < pathToFollow.Count) {
                // start moving from current point to next
                Vector3 currentMove = Vector3.MoveTowards(transform.position, pathToFollow[currentPathIndex], Time.deltaTime * moveSpeed);
                //transform.position = currentMove;
                // using the movePosition method ensures collision still happens
                rigid2D.MovePosition(currentMove);
                movementState = MovementState.running;

                // if we are at the current waypoint, change the current index to next
                if (Vector3.Distance(pathToFollow[currentPathIndex], transform.position) < .1f) {
                    currentPathIndex++;
                }

                // if we are moving left, flip sprite
                Vector3 moveDiff = currentMove - lastMove;
                if (moveDiff.x < 0) {
                    sprite.flipX = true;
                }
                lastMove = currentMove;
            } else {
                // stop movement, set to idle
                //movementState = MovementState.idle;

                // destroy and increment num enemies missed
                GameObject controllerObj = GameObject.Find("GameController");
                GameController controller = controllerObj.GetComponent<GameController>();

                controller.EnemyMissed();
                Destroy(gameObject);
            }
        }
        animator.SetInteger("movementState", (int)movementState);
    }
}
