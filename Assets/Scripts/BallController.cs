using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public Rigidbody rb;
    public float speed = 15;

    public int minSwipeRecognition;

    private bool isTraveling;
    private Vector3 travelDirection;

    private Vector2 swipePosLastFrame;
    private Vector2 swipePosCurrentFrame;
    private Vector2 currentSwipe;

    private Vector3 nextCollisionPosition;

    private Color solveColor;

    private void Start()
    {
        solveColor = Random.ColorHSV(.5f, 1); // Only take pretty light colors
        GetComponent<MeshRenderer>().material.color = solveColor;
        minSwipeRecognition = Screen.height / 10;
    }

    private void FixedUpdate()
    {
        // Set the ball's speed when it should travel
        if (isTraveling)
        {
            rb.velocity = travelDirection * speed;
        }

        // Paint the ground
        Collider[] hitColliders = Physics.OverlapSphere(transform.position - (Vector3.up / 2), .05f);
        int i = 0;
        while (i < hitColliders.Length)
        {
            GroundPiece ground = hitColliders[i].transform.GetComponent<GroundPiece>();

            if (ground && !ground.isColored)
            {
                ground.Colored(solveColor);
            }

            i++;
        }

        // Check if we have reached our destination
        if (nextCollisionPosition != Vector3.zero)
        {
            if (Vector3.Distance(transform.position, nextCollisionPosition) < 1)
            {
                isTraveling = false;
                travelDirection = Vector3.zero;
                nextCollisionPosition = Vector3.zero;
            }
        }

        if (isTraveling)
            return;

        // Swipe mechanism
        if (Input.touchCount > 0)
        {
            //Where is the first finger touch?
            Touch touch = Input.GetTouch(0);

            // Initialize swipe start position
            if (touch.phase == TouchPhase.Began)
            {
                swipePosLastFrame = touch.position; // Set the starting swipe position
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                // Calculate the swipe direction
                swipePosCurrentFrame = touch.position;
                currentSwipe = swipePosCurrentFrame - swipePosLastFrame;

                if (currentSwipe.sqrMagnitude < minSwipeRecognition) // Minium amount of swipe recognition
                    return;

                currentSwipe.Normalize(); // Normalize it to only get the direction not the distance (would fake the ball's speed)

                // Up/Down swipe
                if (Mathf.Abs(currentSwipe.x) < 0.8f)
                {
                    SetDestination(currentSwipe.y > 0 ? Vector3.forward : Vector3.back);
                }

                // Left/Right swipe
                if (Mathf.Abs(currentSwipe.y) < 0.8f)
                {
                    SetDestination(currentSwipe.x > 0 ? Vector3.right : Vector3.left);
                }

                swipePosLastFrame = swipePosCurrentFrame; // Update last frame position
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                // Reset swipe data
                swipePosLastFrame = Vector2.zero;
                currentSwipe = Vector2.zero;
            }
        }
    }

    private void SetDestination(Vector3 direction)
    {
        travelDirection = direction;

        // Check with which object we will collide
        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction, out hit, 100f))
        {
            nextCollisionPosition = hit.point;
        }

        isTraveling = true;
    }
}
