using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MoveState {  WALK, LADDER }

[RequireComponent(typeof(Rigidbody2D))]
public class Moveable : MonoBehaviour
{
    [Header("Components")]
    
    internal Rigidbody2D rb;

    internal MoveState moveState = MoveState.WALK;

    internal Animator animator;

    [Header("Movement Variables")]
    
    [SerializeField] private float moveSpeed = 5f;

    private float moveMultipler = 10f;

    private float ladderMultipler = 2f;

    private Vector2 moveVelocity = Vector2.zero;

    [Header("Jump Variables")]

    [SerializeField] private bool cooldown = false;

    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private LayerMask ceilingLayer;

    [SerializeField] private float jumpHeight = 5;

    [SerializeField] private float jumpMultiplier = 2f;

    [SerializeField] private float jumpHeightLimit = 2f;

    internal bool jumpLimit = false;

    internal bool jumping = false;
    
    private float baseJumpHeight = 5f;
    
    private float groundDisCheck = 0.7f;

    private float jumpLaunchPos;

    private float jumpCooldown = 0f;
    [SerializeField] private float jumpCooldownMax = 0.5f;

    internal bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        baseJumpHeight = jumpHeight;
    }

    private void Update()
    {
        isFalling = !IsGrounded();

        if (IsGrounded() && jumpLimit)
        {
            if (cooldown)
            {
                JumpCooldown();
            }
            else
            {
                jumpLimit = false;
            }
        }

        if (!IsGrounded() && IsCeiling())
        {
            jumpLimit = true;
        }

        animator.SetBool("falling", isFalling);
    }

    private void JumpCooldown()
    {
        jumpCooldown += Time.deltaTime;

        if (jumpCooldown >= jumpCooldownMax)
        {
            jumpLimit = false;
            jumpCooldown = 0f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(moveVelocity, ForceMode2D.Force);
    }

    public void Move(Vector2 direction)
    {
        moveVelocity = direction * moveSpeed * moveMultipler;
    }

    public void LadderMove(Vector2 direction)
    {
        moveVelocity = direction * moveSpeed * ladderMultipler;
    }

    public void Jump()
    {
        if (!jumpLimit)
        {
            jumping = true;

            if (IsGrounded())
            {
                jumpLaunchPos = transform.position.y;
            }

            if (transform.position.y < jumpLaunchPos + jumpHeightLimit)
            {
                rb.AddForce(Vector2.up * jumpHeight * jumpMultiplier * Time.deltaTime * 1000, ForceMode2D.Force);
            }
            else
            {
                jumpLimit = true;
            }
        }
    }

    public void GetOnLadder()
    {   
        moveState = MoveState.LADDER;

        rb.velocity = Vector2.zero;

        rb.gravityScale = 0;

        animator.SetBool("onLadder", true);
    }

    public void GetOffLadder()
    {
        moveState = MoveState.WALK;

        rb.gravityScale = 1;

        animator.SetBool("onLadder", false);
    }

    public RaycastHit2D IsCeiling()
    {
        return Physics2D.Raycast(transform.position, Vector2.up, groundDisCheck, ceilingLayer);
    }

    public RaycastHit2D IsGrounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, groundDisCheck, groundLayer);
    }
}
