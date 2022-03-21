using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //Serialized Params
    [Header("In-Game Params")]
    [SerializeField][Range(0,10)] float moveSpeed;
    [SerializeField] int damage;
    [SerializeField] float waitTime;
    [SerializeField] int health = 2;
    [SerializeField] float pushbackForce;

    // Cached references
    float moveDirection;
    Rigidbody2D enemyRB;
    float currentMoveSpeed;

    Animator animator;
    [Header("Collision Params")]
    [SerializeField] BoxCollider2D groundCollider;
    [SerializeField] LayerMask ground;

    private void Awake()
    {
        enemyRB = GetComponent<Rigidbody2D>();
        moveDirection = 1;
        currentMoveSpeed = moveSpeed;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetInteger("animState", enemyRB.velocity.x == 0 ? 0 : 1);
        CheckForPlayer();
    }

    private void CheckForPlayer()
    {
        Debug.DrawRay(new Vector2(transform.position.x, transform.position.y + 1), 5 * moveDirection * Vector2.right);
        RaycastHit2D raycastHit2D = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 1), 5 * moveDirection * Vector2.right);
        if(raycastHit2D.collider.gameObject.layer == 3)
        {
            Debug.Log("Player Found");
        }        
    }

    private void FixedUpdate()
    {
        enemyRB.velocity = Vector2.right * moveDirection * currentMoveSpeed;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 3)
        {
            currentMoveSpeed = 0;
            Invoke("Turn", waitTime);
        }
    }

    private void Turn()
    {
        moveDirection = groundCollider.IsTouchingLayers(ground) ? moveDirection * 1 : moveDirection * -1;
        transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        currentMoveSpeed = moveSpeed;
    }

    public void TakeDamage(float hitSourceDirection)
    {
        health--;
        if (health != 0)
        {
            currentMoveSpeed = 0;
            animator.SetTrigger("takeDamage");
            enemyRB.AddForce(new Vector2(pushbackForce * hitSourceDirection, 0));
            Invoke("ResetSpeed", 0.4f);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void ResetSpeed()
    {
        currentMoveSpeed = moveSpeed;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(new Vector2(transform.position.x, transform.position.y + 1), Vector2.right * moveDirection * 5);
    }
}
