using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PriestAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float chaseSpeed = 3f;
    public float detectionRange = 5f;
    public Vector2 patrolPointA;
    public Vector2 patrolPointB;
    public LayerMask playerLayer;
    public Collider2D[] playerColliders;

    private Transform targetPlayer;
    private Collider2D targetCollider;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 currentTarget;
    private bool chasing = false;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        currentTarget = patrolPointB;
    }

    void Update()
    {
        GetClosestVisiblePlayer();

        animator.SetBool("isChasing", chasing);

        if (chasing)
            Chase();
        else
            Patrol();

        AnimateAndFlip();
    }

    void Patrol()
    {

        if (Vector2.Distance(transform.position, patrolPointA) < 0.1f)
            currentTarget = patrolPointB;
        else if (Vector2.Distance(transform.position, patrolPointB) < 0.1f)
            currentTarget = patrolPointA;

        MoveTowards(currentTarget, patrolSpeed);
    }

    void Chase()
    {
        if (targetPlayer == null) return;

        float minX = Mathf.Min(patrolPointA.x, patrolPointB.x);
        float maxX = Mathf.Max(patrolPointA.x, patrolPointB.x);
        float clampedX = Mathf.Clamp(targetPlayer.position.x, minX, maxX);
        Vector2 targetPos = new Vector2(clampedX, transform.position.y);
        MoveTowards(targetPos, chaseSpeed);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        foreach (Collider2D playerCol in playerColliders)
        {
            if (other == playerCol)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                break;
            }
        }
    }

    void MoveTowards(Vector2 target, float speed)
    {
        Vector2 newPos = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
        rb.MovePosition(newPos);
    }

    void GetClosestVisiblePlayer()
    {
        Collider2D priestCollider = GetComponent<Collider2D>();
        if (priestCollider == null)
        {
            chasing = false;
            targetPlayer = null;
            targetCollider = null;
            return;
        }

        Transform newTarget = null;
        Collider2D newTargetCol = null;
        float minDist = detectionRange + 1;

        foreach (Collider2D playerCol in playerColliders)
        {
            if (playerCol == null) continue;

            Vector2 playerCenter = playerCol.bounds.center;
            Vector2 priestCenter = priestCollider.bounds.center;
            Vector2 offset = playerCenter - priestCenter;
            float dist = offset.magnitude;

            bool withinDetection = offset.y >= -1f && dist <= detectionRange;

            if (withinDetection && dist < minDist)
            {
                newTarget = playerCol.transform;
                newTargetCol = playerCol;
                minDist = dist;
            }
        }

        // If not detecting anyone new but currently chasing, check if the target is still in patrol bounds
        if (newTarget == null && chasing && targetPlayer != null)
        {
            float playerX = targetPlayer.position.x;
            float minX = Mathf.Min(patrolPointA.x, patrolPointB.x);
            float maxX = Mathf.Max(patrolPointA.x, patrolPointB.x);
            float playerY = targetPlayer.position.y;
            float priestY = transform.position.y;

            bool stillInPatrolBounds =
                playerX >= minX && playerX <= maxX &&
                playerY >= priestY - 1f; // still above or near

            if (stillInPatrolBounds)
            {
                newTarget = targetPlayer;
                newTargetCol = targetCollider;
            }
        }

        chasing = newTarget != null;
        targetPlayer = newTarget;
        targetCollider = newTargetCol;
    }

    void AnimateAndFlip()
    {
        float direction = chasing && targetPlayer != null
            ? targetPlayer.position.x - transform.position.x
            : currentTarget.x - transform.position.x;

        if (Mathf.Abs(direction) > 0.05f)
            spriteRenderer.flipX = direction < 0;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(patrolPointA, patrolPointB);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
