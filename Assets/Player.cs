using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player : MonoBehaviour
{
    private Rigidbody2D _rb;
    private Animator _animator;
    

    
    [Header("Attack details")]
    [SerializeField] private float attackRadius;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private LayerMask whatIsEnemy;

    [Header("Movement details")] [SerializeField]
    private float moveSpeed = 3.5f;

    [SerializeField] private float jumpForce = 8;
    private float _xInput;
    private bool _facingRight = true;
    private bool _canMove = true;
    private bool _canJump = true;

    [Header("Collision details")] [SerializeField]
    private float groundCheckDistance;

    [SerializeField] private LayerMask whatIsGround;
    private bool _isGrounded;

    public void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponentInChildren<Animator>();
    }

    public void Update()
    {
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleAnimations();
        HandleFlip();
    }

    public void DamageEnemies()
    {
        Collider2D[] enemyColliders= Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsEnemy);

        foreach (Collider2D enemy in enemyColliders)
        {
            enemy.GetComponent<Enemy>().TakeDamage(10);
        }
    }

    public void EnableMovementAndJump(bool enable)
    {
        _canMove = enable;
        _canJump = enable;
    }

    private void HandleInput()
    {
        _xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.K))
        {
            TryToJump();
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            TryToAttack();
        }
    }

    private void TryToAttack()
    {
        if (_isGrounded)
        {
            _animator.SetTrigger("attack");
        }
    }

    private void HandleCollision()
    {
        _isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void HandleAnimations()
    {
        _animator.SetFloat("xVelocity", _rb.linearVelocityX);
        _animator.SetBool("isGrounded", _isGrounded);
        _animator.SetFloat("yVelocity", _rb.linearVelocityY);
    }

    private void HandleFlip()
    {
        if (_rb.linearVelocity.x > 0 && !_facingRight || _rb.linearVelocity.x < 0 && _facingRight)
        {
            Flip();
        }
    }

    private void TryToJump()
    {
        if (_isGrounded && _canJump)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocityX, jumpForce);
        }
    }

    private void HandleMovement()
    {
        _rb.linearVelocity = _canMove ? new Vector2(_xInput * moveSpeed, _rb.linearVelocity.y) : new Vector2(0,_rb.linearVelocity.y);
    }

    private void Flip()
    {
        transform.Rotate(0, 180, 0);
        _facingRight = !_facingRight;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        Gizmos.DrawWireSphere(attackPoint.position,attackRadius);
    }
}