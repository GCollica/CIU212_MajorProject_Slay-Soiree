using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Pathfinding;

public class BasicEnemy1_AI : MonoBehaviour
{
    private BasicEnemy1 basicEnemy1Script;

    //Data references for state machine components of the code.
    public enum AIState {Idle, FindingTarget, PursuingTarget, AttackSequence, ExecutingAttacks};
    public AIState currentAIState = AIState.Idle;

    //Data references for multi-use components of the code.
    private Rigidbody2D rigidBody;
    private Transform targetTransform;

    //Date references for idle state of the code.
    private float idleDelayTimer = 0f;
    private float idleDelayLength = 1f;

    //Data references for pathfinding components of the code.
    private Seeker seeker;
    private Path currentPath;

    private bool creatingPath = false;
    private bool reachedEndOfPath = false;

    private int currentWaypoint = 0;

    //public float movementSpeed = 10f;
    private float nextWaypointDistance = 3f;
    private float updatePathTimer = 0f;
    private float updatePathInterval = .25f;

    //Data references for attacking components of the code.
    private GameObject attackParent;

    private Transform attackPos1;
    private Transform attackPos2;
    private Transform attackPos3;

    public List<GameObject> attackTargets;

    private GameObject[] totalPlayers;
    public RaycastHit2D[] raycastHits;

    private bool inAttackRange = false;
    private bool isAttacking = false;
    private bool attackCoolingDown = false;

    private float attackSequenceTimer = 0f;
    private float attackSequenceDuration = 1f;

    public float attackCoolDownTimer = 0f;
    public float attackCoolDownDuration = 3f;
    void Awake()
    {
        rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
        seeker = this.gameObject.GetComponent<Seeker>();
        basicEnemy1Script = this.gameObject.GetComponent<BasicEnemy1>();
        attackParent = FindChildGameObject(this.gameObject, "Attack_Direction");
        attackPos1 = FindChildGameObject(attackParent, "Attack_Pos1").transform;
        attackPos2 = FindChildGameObject(attackParent, "Attack_Pos2").transform;
        attackPos3 = FindChildGameObject(attackParent, "Attack_Pos3").transform;

    }
  
    void FixedUpdate()
    {
        switch (currentAIState)
        {
            case AIState.Idle:
                
                ClearPath();
                RunAttackCooldownTimer();

                if(idleDelayTimer < idleDelayLength)
                {
                    idleDelayTimer += Time.deltaTime;
                }
                else if(idleDelayTimer >= idleDelayLength)
                {
                    currentAIState = AIState.FindingTarget;
                }
                break;

            case AIState.FindingTarget:

                idleDelayTimer = 0f;
                InitialiseTargets();
                FindNearestTarget();
                RunAttackCooldownTimer();
                currentAIState = AIState.PursuingTarget;
                break;

            case AIState.PursuingTarget:

                SetFacingDirection();
                PursureTarget();
                RunAttackCooldownTimer();
                if(inAttackRange == true)
                {
                    currentAIState = AIState.AttackSequence;
                }
                break;

            case AIState.AttackSequence:

                isAttacking = true;
                RunAttackCooldownTimer();

                if (inAttackRange == false)
                {
                    currentAIState = AIState.FindingTarget;
                }

                if(inAttackRange == true && attackCoolingDown == false)
                {
                    SetAttackDirection();
                    attackSequenceTimer += Time.deltaTime;

                    if (attackSequenceTimer < attackSequenceDuration)
                    {
                        AttackRaycast(2);
                        currentAIState = AIState.ExecutingAttacks;
                    }
                }
                break;

            case AIState.ExecutingAttacks:
                
                if(isAttacking == true)
                {
                    ExecuteAttacks();
                    attackTargets.Clear();
                    attackSequenceTimer = 0f;
                    attackCoolDownTimer = 0f;
                    attackCoolingDown = true;
                    isAttacking = false;
                }

                if (inAttackRange == false)
                {
                    currentAIState = AIState.FindingTarget;
                }
                else
                {
                    currentAIState = AIState.AttackSequence;
                }
                break;

            default:
                break;
        }

    }

    //Initialises totalPlayers array.
    private void InitialiseTargets()
    {
        totalPlayers = GameObject.FindGameObjectsWithTag("Player");
    }

    //Finds current closest target to this enemy.
    private void FindNearestTarget()
    {
        float closestDistance = (totalPlayers[0].transform.position - this.gameObject.transform.position).magnitude;
        GameObject closestPlayer = totalPlayers[0];

        foreach (GameObject player in totalPlayers)
        {
            if((player.transform.position - this.gameObject.transform.position).magnitude < closestDistance)
            {
                closestPlayer = player;
            }
        }

        targetTransform = closestPlayer.transform;
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
        Vector2 force = directionVector * basicEnemy1Script.basicEnemyClass.currentMovementSpeed * Time.deltaTime;

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

            //Debug.Log("Created Path Successfully");
        }
    }

    //Clears current path.
    private void ClearPath()
    {
        if(currentPath != null)
        {
            currentPath = null;
        }
    }

    //Sets characters Facing Direction by flipping the sprite.
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

    //Sets characters attack direction.
    private void SetAttackDirection()
    {
        attackParent.transform.right = (targetTransform.transform.position - attackParent.transform.position);
        
    }

    //Small function to find a child Gameobject given the parent and childs name.
    private GameObject FindChildGameObject(GameObject parent, string childName)
    {
        GameObject result;

        result = parent.transform.Find(childName).gameObject;

        return result;
    }

    //Turns on and off circlecasts used for attack sequence, attackPos input is used for the sequential nature of the check.
    private void AttackRaycast(int attackPos)
    {
        switch (attackPos)
        {
            case 1:
                
                raycastHits = Physics2D.CircleCastAll(attackPos1.position, 1f, (this.transform.position - attackPos1.position));

                if (raycastHits.Length > 0)
                {
                    foreach (RaycastHit2D hit in raycastHits)
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            //Debug.Log("Player " + hit.collider.gameObject.name.ToString() + " Collider Hit");

                            if (attackTargets.Contains(hit.collider.gameObject) != true)
                            {
                                attackTargets.Add(hit.collider.gameObject);
                            }
                        }
                    }

                    Array.Clear(raycastHits, 0, raycastHits.Length);
                }
                break;

            case 2:

                raycastHits = Physics2D.CircleCastAll(attackPos2.position, 1f, (this.transform.position - attackPos2.position));

                if (raycastHits.Length > 0)
                {
                    foreach (RaycastHit2D hit in raycastHits)
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            //Debug.Log("Player " + hit.collider.gameObject.name.ToString() + " Collider Hit");

                            if (attackTargets.Contains(hit.collider.gameObject) != true)
                            {
                                attackTargets.Add(hit.collider.gameObject);
                            }
                        }
                    }

                    Array.Clear(raycastHits, 0, raycastHits.Length);
                }
                break;

            case 3:

                raycastHits = Physics2D.CircleCastAll(attackPos3.position, 1f, (this.transform.position - attackPos3.position));

                if (raycastHits.Length > 0)
                {
                    foreach (RaycastHit2D hit in raycastHits)
                    {
                        if (hit.collider.CompareTag("Player"))
                        {
                            //Debug.Log("Player " + hit.collider.gameObject.name.ToString() + " Collider Hit");

                            if (attackTargets.Contains(hit.collider.gameObject) != true)
                            {
                                attackTargets.Add(hit.collider.gameObject);
                            }
                        }
                    }

                    Array.Clear(raycastHits, 0, raycastHits.Length);
                }
                break;

            default:
                break;

        }
    }

    //Executes attacks based on targets chosen from the AttackRaycast function. *Needs check as it throws an error*
    private void ExecuteAttacks()
    {
        if(attackTargets != null)
        {
            if (attackTargets.Count > 0)
            {                
                foreach (GameObject target in attackTargets)
                {
                    Debug.Log("Executed attack on " + target.name);
                    target.GetComponent<PlayerStats>().TakeDamage(basicEnemy1Script.basicEnemyClass.currentDamage);
                    //Perform Attacks.
                    //attackTargets.Remove(target);
                }
            }
        }      
    }

    private void RunAttackCooldownTimer()
    {
        if(attackCoolingDown == true)
        {
            if(attackCoolDownTimer < attackCoolDownDuration)
            {
                attackCoolDownTimer += Time.deltaTime;
            }
            if(attackCoolDownTimer >= attackCoolDownDuration)
            {
                attackCoolingDown = false;
            }
        }
    }

    //Basic check for whether the enemy is within attack range of the current target.
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.CompareTag("Player"))
        {
            if(collider.gameObject == targetTransform.gameObject)
            {
                inAttackRange = true;
            }
        }
    }
    
    //Basic check for whether the enemy has left attack range of the current target.
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            if (collider.gameObject == targetTransform.gameObject)
            {
                inAttackRange = false;
            }
        }
    }

    public void Knockback()
    {
        Vector2 directionVector = (targetTransform.position - this.gameObject.transform.position).normalized;
        Vector2 force = -directionVector * 20f * Time.deltaTime;
        rigidBody.AddForce(force);
    }
}
