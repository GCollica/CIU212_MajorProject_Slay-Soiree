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

    public GameObject attackParent;
    public GameObject attackCollider;
    public Transform attackStartPos;
    public Transform attackMidPos;
    public Transform attackEndPos;


    void Awake()
    {
        rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        seeker = this.gameObject.GetComponent<Seeker>();
        attackParent = FindChildGameObject(this.gameObject, "Attack_Direction");
        attackCollider = FindChildGameObject(attackParent, "Attack_Collider");
        attackStartPos = FindChildGameObject(attackParent, "Attack_StartPos").transform;
        attackMidPos = FindChildGameObject(attackParent, "Attack_MidPos").transform;
        attackEndPos = FindChildGameObject(attackParent, "Attack_EndPos").transform;
        ResetAttackPos();

    }

    
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
        else if (currentAIState == AIState.AttackingTarget)
        {
            SetAttackDirection();
        }
        else
        {
            return;
        }

    }

    //Function to run in update which creates a path for the ai to the target on an interval and moves the enemy along the path
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

    //Generates path towards the target postition
    private void GeneratePath()
    {
        creatingPath = true;
        seeker.StartPath(rigidBody.position, targetTransform.position, OnPathComplete);
        updatePathTimer = 0f;
    }

    //Checks to see if the path was created without an error, callback function for GeneratePath
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

    //Clears current path
    private void ClearPath()
    {
        if(currentPath != null)
        {
            currentPath = null;
        }
    }

    //Sets characters Facing Direction by flipping the sprite
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

    //Sets characters attack direction
    private void SetAttackDirection()
    {
        attackParent.transform.right = (targetTransform.transform.position - attackParent.transform.position);
        
    }

    //Small function to find a child Gameobject given the parent and childs name
    private GameObject FindChildGameObject(GameObject parent, string childName)
    {
        GameObject result;

        result = parent.transform.Find(childName).gameObject;

        return result;
    }

    private void Attack()
    {
        
    }

    private void ResetAttackPos()
    {
        attackCollider.transform.position = attackStartPos.position;
    }
}
