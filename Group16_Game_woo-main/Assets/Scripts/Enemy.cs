/** Contributors: Zackary Mowbray **/
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
public class Enemy : MonoBehaviour
{
    public enum Type {
        melee,
        ranged
    }

    [SerializeField] private float maxSpeed = 0.5f;
    //[SerializeField] private float minSpeed = 2.0f;
    [SerializeField] private float rotateSpeed = 360.0f;
    //[SerializeField] private float accelerationSpeed = 0.2f;
    //[SerializeField] private float deccelerationSpeed = 7.0f;
    [SerializeField] float rotationalDampener;
    [SerializeField] float Traction;

    [SerializeField] private float circleRadius = 10.0f;
    [SerializeField] float circleVariance;

    [SerializeField] float avoidanceStrength;
    [SerializeField] float avoidanceSize;

    // This controls how much the ship tries to stay on the circle line
    [SerializeField] float circleAdjustmentMultiplier;

    public TrickleSpawner spawner;
    public StatManager statScript;
    public CharacterController controller;

    public Type type;
    public GameObject target;

    private Rigidbody rb;
    private AIManager aiManager;

    void Start()
    {
        //for testing only rmove in final product
        target = GameManager.instance.player;
        if (type == Type.melee) circleRadius = 0;

        rb = GetComponent<Rigidbody>();
        aiManager = FindObjectOfType<AIManager>();
        if (aiManager == null) Debug.Log("Cannot Find AI Manager");
    }


    void FixedUpdate()
    {
        if(target != null)
        {
            Move();
            Attack();
        }
        else
        {
            target = GameManager.instance.player;
        }
    }

    //Zackary Mowbray
    private void Move()
    {
        if (target == null) return;

        // Get Play Pos
        Vector3 playerPos = target.transform.position;
        playerPos.y = 0f;

        // Get Enemy Pos
        Vector3 enemyPos = transform.position;
        enemyPos.y = 0f;

        // Get distance to player
        float distanceToPlayer = Vector3.Distance(playerPos, enemyPos);
        
        // Get direction to player
        Vector3 playerDir = playerPos - enemyPos;
        playerDir.Normalize();

        // Get perpendicular direction to player
        Vector3 perpendicularDirection = Vector3.Cross(playerDir, Vector3.up).normalized;

        // Get Obstacle Avoidance Behavior
        Vector3 AvoidanceForce = CalculateObstacleAvoidanceForce();

        if (distanceToPlayer > circleRadius + circleVariance)
        {
            // If far away from player go towards player
            TurnTowardDirection(playerDir + AvoidanceForce);
        }
        else if (distanceToPlayer < circleRadius - circleVariance)
        {
            // If too close to player move away
            TurnTowardDirection(-playerDir + AvoidanceForce);
        }
        else
        {
            // circle player
            // Adjust the turn based on how far out of the circle
            float adjustmentFactor = (distanceToPlayer - circleRadius + circleVariance) / circleVariance;
            adjustmentFactor *= circleAdjustmentMultiplier;
            Vector3 adjustedDirection = Vector3.Slerp(perpendicularDirection, playerDir, adjustmentFactor);

            // Turn toward the adjusted direction
            TurnTowardDirection(adjustedDirection + AvoidanceForce);

            //TurnTowardDirection(perpendicularDirection);
        }
        MoveForward();
    }

    private Vector3 CalculateObstacleAvoidanceForce()
    {
        Vector3 avoidanceForce = Vector3.zero;
        float avoidanceRadius = rb.velocity.magnitude + avoidanceSize;

        List<GameObject> obstacles = aiManager.FindNearbyProps(transform.position, avoidanceRadius);

        foreach (GameObject obstacle in obstacles)
        {
            float distanceToObstacle = Vector3.Distance(transform.position, obstacle.transform.position);

            Vector3 toObstacle = obstacle.transform.position - transform.position;

            Vector3 perpendicularToObstacle = Vector3.Cross(Vector3.up, toObstacle.normalized).normalized;

            avoidanceForce += perpendicularToObstacle / Mathf.Pow(distanceToObstacle, 2);

        }

        return avoidanceForce.normalized * avoidanceStrength;
    }
    private void TurnTowardDirection(Vector3 direction)
    {
        Vector3 torqueDirection = Vector3.Cross(transform.forward, direction.normalized);

        rb.AddTorque(torqueDirection * rotateSpeed * (statScript.GetHandling() / 100));

        // Rotate Damperner
        rb.angularVelocity *= Mathf.Pow(1.0f - rotationalDampener * Time.fixedDeltaTime, Time.fixedDeltaTime);
    }
    private void MoveForward()
    {
        rb.AddRelativeForce(Vector3.forward * statScript.GetAcceleration() * Time.fixedDeltaTime);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);

        // Traction
        rb.velocity = Vector3.Lerp(rb.velocity.normalized, transform.forward, Traction * Time.fixedDeltaTime) * rb.velocity.magnitude;
    }

    public void HandleDeath()
    {
        if (spawner != null)
        { 
            spawner.EnemyKilled();
        }
        Destroy(this.gameObject);
    }

    private void Attack()
    {
        switch (type)
        {
            case Type.ranged:
                float sideCheck = Vector3.Dot(Vector3.Normalize(transform.TransformDirection(Vector3.right)),
                    Vector3.Normalize(target.transform.position - transform.position));
                this.GetComponentInChildren<Boat>().ActivateAbilitiesByGroup((sideCheck > 0) ? 1:2);
                break;
            default: break;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            StatManager SM = other.transform.GetComponent<StatManager>();
            if (SM != null)
            {
                SM.IncrementHealth(-statScript.Damage);
            }
            //other.GetComponent<CharacterController>().Move(transform.forward * 30);
        }
    }
}

