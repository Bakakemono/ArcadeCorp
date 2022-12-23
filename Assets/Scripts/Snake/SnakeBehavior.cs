using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBehavior : MonoBehaviour {
    Vector2 direction = Vector2.right;

    Vector3 lockedDirection = Vector3.zero;

    int frameBeforeNextMove = 0;
    int frameNeededToMove = 10;

    [Header("Snake parts param")]
    int snakeCurrentSize = 1;
    int snakeMaxSize = 100;
    int nmbAwaitingSegment = 0;
    Transform[] snakeSegments;
    [SerializeField]GameObject snakeSegmentPrefab;
    List<Vector3> newSegmentActivationPositions = new List<Vector3>();
    Vector3 lastOldPos;
    bool newSegmentWaiting = false;

    bool isSnaking = false;

    float timeBeforeNewSegment = 0.0f;
    float timerTotal = 0.5f;

    private void Start() {
        snakeSegments = new Transform[snakeMaxSize];
        snakeSegments[snakeCurrentSize - 1] = transform;
        frameBeforeNextMove = frameNeededToMove;
        timeBeforeNewSegment = timerTotal;
    }
    private void FixedUpdate() {
        SetLockedDirection();

        if(lockedDirection == Vector3.zero) {
            return;
        }
        else {
            isSnaking = true;
        }

        if(frameBeforeNextMove <= 0) {
            lastOldPos = snakeSegments[snakeCurrentSize - 1].position;
            for(int i = snakeCurrentSize - 1; i > 0; i--) {
                snakeSegments[i].position = snakeSegments[i - 1].position;
            }
            snakeSegments[0].position += lockedDirection;
            frameBeforeNextMove = frameNeededToMove;
        }
        frameBeforeNextMove--;

        if(newSegmentWaiting && newSegmentActivationPositions[0] == lastOldPos) {
            ActivateSegement();
        }
    }

    private void Update() {
        direction =
            new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
                );

        if(timeBeforeNewSegment <= 0) {
            AddNewSegment();
            timeBeforeNewSegment = timerTotal;
        }
        timeBeforeNewSegment -= Time.deltaTime;
    }

    private void SetLockedDirection() {
        if(direction.x > 0 && lockedDirection != Vector3.left) {
            lockedDirection = Vector3.right;
        }
        else if(direction.x < 0 && lockedDirection != Vector3.right) {
            lockedDirection = Vector3.left;
        }
        else if(direction.y > 0 && lockedDirection != Vector3.down) {
            lockedDirection = Vector3.up;
        }
        else if(direction.y < 0 && lockedDirection != Vector3.up) {
            lockedDirection = Vector3.down;
        }
    }

    private void AddNewSegment() {
        if(snakeCurrentSize + nmbAwaitingSegment > snakeMaxSize) {
            return;
        }

        newSegmentActivationPositions.Add(snakeSegments[0].position);

        snakeSegments[snakeCurrentSize + nmbAwaitingSegment] =
            GameObject.Instantiate(
                snakeSegmentPrefab,
                newSegmentActivationPositions[newSegmentActivationPositions.Count - 1],
                Quaternion.identity
                ).GetComponent<Transform>();

        newSegmentWaiting = true;
    }

    private void ActivateSegement() {
        if(!newSegmentWaiting)
            return;


        if(snakeCurrentSize < snakeMaxSize) {
            newSegmentActivationPositions.RemoveAt(0);
            snakeCurrentSize++;
        }

        if(newSegmentActivationPositions.Count == 0) {
            newSegmentWaiting = false;
        }
    }
}