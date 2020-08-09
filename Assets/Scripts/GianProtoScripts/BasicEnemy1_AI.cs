using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class BasicEnemy1_AI : MonoBehaviour
{
    public enum AIState {Idle, PursuingTarget, AttackingTarget};
    public AIState currentAIState = AIState.Idle;

    private Rigidbody2D rigidBody;
    private Seeker seeker;

    public Transform targetTransform;
    public float movementSpeed = 10f;
    public float nextWaypointDistance = 3f;

    private Path currentPath;
    private int currentWaypoint = 0;
    private bool creatingPath = false;
    public bool reachedEndOfPath = false;

    private float updatePathTimer = 0f;
    private float updatePathInterval = .25f;


    void Awake()
    {
        rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        seeker = this.gameObject.GetComponent<Seeker>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentAIState == AIState.Idle)
        {
            ClearPath();
            return;
        }
        else if(currentAIState == AIState.PursuingTarget)
        {
            SetFacingDirection();
            PursureTarget();

        }
        else
        {
            return;
        }

    }

    private void PursureTarget()
    {
        if (currentPath == null && creatingPath == false || reachedEndOfPath == true)
        {
            GeneratePath();
        }

        if (currentPath == null && creatingPath == true)
            return;

        if(currentPath != null && creatingPath == false)
        {

            if(updatePathTimer < updatePathInterval)
            {
                updatePathTimer += Time.deltaTime;
            }

            if(updatePathTimer >= updatePathInterval)
            {
                GeneratePath();
            }

        }

        if (currentWaypoint >= currentPath.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 directionVector = ((Vector2)currentPath.vectorPath[currentWaypoint] - rigidBody.position).normalized;
        Vector2 force = directionVector * movementSpeed * Time.deltaTime;

        rigidBody.AddForce(force);

        float distance = Vector2.Distance(rigidBody.position, currentPath.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    private void GeneratePath()
    {
        creatingPath = true;
        seeker.StartPath(rigidBody.position, targetTransform.position, OnPathComplete);
        updatePathTimer = 0f;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
            creatingPath = false;

            Debug.Log("Created Path Successfully");
        }
    }

    private void ClearPath()
    {
        if(currentPath != null)
        {
            currentPath = null;
        }
    }

    private void SetFacingDirection()
    {
        if (rigidBody.velocity.x > 0)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = true;
        }
        else if (rigidBody.velocity.x <= 0)
        {
            this.gameObject.GetComponentInChildren<SpriteRenderer>().flipX = false;
        }
    }
}
