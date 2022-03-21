using UnityEngine;

public class Hero : MonoBehaviour
{
    #region references
    // Serialized references
    [Header("Movement & Health")]    
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] float pushbackForce = 200f;

    [Header("Attack Attributes")]
    [SerializeField] float attackCD;
    [SerializeField] float swordCOlliderTimer;

    [Space]
    [SerializeField] BoxCollider2D swordCollider;

    // cached references
    SessionManager sessionManager;
    CapsuleCollider2D capsuleCollider2D;
    new Rigidbody2D rigidbody2D;
    BoxCollider2D boxCollider2D;
    Animator animator;
    int attackAnimationIndex = 0;
    float xInput;
    string[] attackAnimations = { "attack", "attack2" };
    bool isGrounded;
    bool isFalling;
    bool canMove = true;
    bool canAttack = true;
    #endregion

    void Awake()
    {
        sessionManager = FindObjectOfType<SessionManager>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        swordCollider.enabled = false;
    }


    void Update()
    {
        ChekCharacterState();
    }

    void ChekCharacterState()
    {
        isGrounded = boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Foreground"));
        isFalling = rigidbody2D.velocity.y < -0.2f;
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rigidbody2D.AddForce(new Vector2(0, jumpForce));            
        }
        
        else if (Input.GetButtonDown("Attack") && canAttack)
        {
            Attack();
        }

        xInput = canMove ? Input.GetAxisRaw("Horizontal") : 0;
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            transform.localScale = new Vector2(Input.GetAxisRaw("Horizontal"), 1);
            xInput *= moveSpeed;
        }

        UpdateAnimation();
    }

    void Attack()
    {

        // Cycle between attack animations
        if (attackAnimationIndex < attackAnimations.Length - 1)
        {
            animator.SetTrigger(attackAnimations[attackAnimationIndex]);
            attackAnimationIndex++;
        }

        else
        {
            animator.SetTrigger(attackAnimations[attackAnimationIndex]);
        }
        swordCollider.enabled = true;
        Invoke("ResetColider", swordCOlliderTimer);
        Invoke("ResetAttackTimer", attackCD);
    }


    void ResetAttackTimer()
    {
        canAttack = true;
        attackAnimationIndex = 0;
    }

    void ResetColider()
    {
        swordCollider.enabled = false;
    }
    void UpdateAnimation()
    {
        animator.SetBool("isRunning", xInput != 0 && isGrounded);
        animator.SetBool("isGrounded", isGrounded);
        animator.SetBool("isFalling", isFalling);
    }

    void FixedUpdate()
    {
        rigidbody2D.velocity = new Vector2(xInput, rigidbody2D.velocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            Debug.Log("Enemy hit");
            collision.GetComponent<Enemy>().TakeDamage(transform.localScale.x);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            animator.SetTrigger("Hurt");
            rigidbody2D.AddForce(- Mathf.Sign(transform.localScale.x) * pushbackForce * Vector2.right);
            canMove = false;            
            sessionManager.TakeDamage();
            Invoke("RsetMoveBool", 0.5f);
        }        
    }
    void RsetMoveBool()
    {
        canMove = true;
    }

    public void Die()
    {
        animator.SetTrigger("Die");        
    } 
}
