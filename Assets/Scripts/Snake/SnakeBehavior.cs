using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBehavior : MonoBehaviour {
    Vector2 direction = Vector2.zero;

    Vector3 lockedDirection = Vector3.zero;

    int frameBeforeNextMove = 0;
    int frameNeededToMove = 10;

    [Header("Snake parts param")]
    int snakeCurrentSize = 0;
    int snakeMaxSize = 100;
    Transform[] snakeSegments;
    GameObject snakeSegmentPrefab;
    Vector3 newSegmentPos;
    bool newSegmentWaiting = false;

    bool isSnaking = false;

    private void Start() {
        snakeSegments = new Transform[snakeMaxSize];
        frameBeforeNextMove = frameNeededToMove;
    }
    private void FixedUpdate() {
        if(lockedDirection == Vector3.zero) {
            return;
        }
        else {
            isSnaking = true;
        }

        SetLockedDirection();

        if(frameBeforeNextMove <= 0) {
            transform.position += lockedDirection;
            frameBeforeNextMove = frameNeededToMove;
        }
        frameBeforeNextMove--;
    }

    private void Update() {
        direction =
            new Vector2(
                Input.GetAxis("Horizontal"),
                Input.GetAxis("Vertical")
                );
    }

    private void SetLockedDirection() {
        if(direction.x > 0) {
            lockedDirection = Vector3.right;
        }
        else if(direction.x < 0) {
            lockedDirection = Vector3.left;
        }
        else if(direction.y > 0) {
            lockedDirection = Vector3.up;
        }
        else if(direction.y < 0) {
            lockedDirection = Vector3.down;
        }
    }

    private void AddNewSegment() {
        newSegmentPos = transform.position;
        snakeSegments[snakeCurrentSize] =
            GameObject.Instantiate(
                snakeSegmentPrefab,
                newSegmentPos,
                Quaternion.identity
                ).GetComponent<Transform>();
        newSegmentWaiting = true;
    }
    private void ActivateSegement() {
        newSegmentWaiting = false;
    }
}