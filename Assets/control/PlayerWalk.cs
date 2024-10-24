using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    public float moveSpeed = 5f;           // Horizontal movement speed
    public float jumpForce = 15f;         // Jump force for vertical movement
    public Animator animator;             // Animator for controlling animations

    private Rigidbody2D rb;               // Rigidbody2D component
    private bool isFacingRight = true;    // Track whether the player is facing right
    private bool isGrounded;               // Check if the player is grounded
    private bool canDoubleJump;            // Check if the player can double jump
    private bool isJumping;               // Check if the player is currently jumping
    private bool isMoving;               // Check if the player is moving

    private Transform groundCheck;        // Ground check position
    public float groundCheckRadius = 0.2f; // Radius for checking ground
    private LayerMask groundLayer;        // Layer used to detect ground

    void Start()
    {
        // Automatically get references
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("GroundCheck");
        groundLayer = LayerMask.GetMask("Ground");

        rb.freezeRotation = true; // Lock the character's rotation
    }

    void Update()
    {
        // Get horizontal input
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Handle character flip
        if ((horizontalInput < 0 && isFacingRight) || (horizontalInput > 0 && !isFacingRight))
        {
            FlipCharacter();
        }

        // Check if grounded and handle jumping
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = true;
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false;
            }
        }

        // Update isMoving based on horizontal input
        isMoving = Mathf.Abs(horizontalInput) > 0.1f;

        // Update animation based on movement and jump state
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput)); // Set speed based on horizontal velocity
        animator.SetBool("IsJumping", !isGrounded); // Set jumping state based on grounded check
        animator.SetBool("IsMoving", isMoving); // Set moving state based on isMoving

        // Update animation on key press
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            animator.SetTrigger("IsMoving"); // Set a trigger for movement animation
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        // Apply vertical force
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        Debug.Log("Jump executed. Velocity: " + rb.velocity);
    }

    private void FlipCharacter()
    {
        // Toggle facing direction and update animation parameter
        isFacingRight = !isFacingRight;
        animator.SetTrigger("Flip"); // Use a trigger for flipping animation

        // Invert X scale to flip character (optional)
        // Vector3 scale = transform.localScale;
        // scale.x *= -1;
        // transform.localScale = scale;
    }
}