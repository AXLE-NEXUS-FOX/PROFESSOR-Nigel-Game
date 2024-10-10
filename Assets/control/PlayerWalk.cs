using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalk : MonoBehaviour
{
    public float moveSpeed = 5f;        // Horizontal movement speed
    public float jumpForce = 15f;       // Jump force for vertical movement
    //public Animator animator;           // Animator for controlling animations

    private Rigidbody2D rb;             // Rigidbody2D component
    private bool isFacingRight = true;  // Track whether the player is facing right
    private bool isGrounded;            // Check if the player is grounded
    private bool canDoubleJump;         // Check if the player can double jump
    private bool isJumping;             // Check if the player is currently jumping

    private Transform groundCheck;      // Ground check position
    public float groundCheckRadius = 0.2f; // Radius for checking ground
    private LayerMask groundLayer;      // Layer used to detect ground

    void Start()
    {
        // Automatically get references to Rigidbody2D, groundCheck, and groundLayer
        rb = GetComponent<Rigidbody2D>();                  // Get the Rigidbody2D component of the player.
        groundCheck = transform.Find("GroundCheck");       // Find the ground check position (assumes a child object named "GroundCheck").
        groundLayer = LayerMask.GetMask("Ground");         // Set the ground layer (replace "Ground" with the actual layer name).

        rb.freezeRotation = true; // Lock the character's rotation.
    }

    void Update()
    {
        // Get the horizontal input (A/D or Left/Right arrow keys)
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Handle character flip when moving left or right
        if ((horizontalInput < 0 && isFacingRight) || (horizontalInput > 0 && !isFacingRight))
        {
            FlipCharacter();
        }

        // Check if the player is grounded and handle jumping logic
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Jump input using the Space Bar (or "Jump" button in Input Manager)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
                canDoubleJump = true; // Allow double jump after the first jump
            }
            else if (canDoubleJump)
            {
                Jump();
                canDoubleJump = false; // Disable double jump after second jump
            }
        }

        // Control the animation based on whether the player is jumping or not
        if (!isGrounded)
        {
            isJumping = true;
        }
        else
        {
            isJumping = false;
        }

        // Update the Animator
        //animator.SetBool("isJumping", isJumping); // Set jumping state
        //animator.SetFloat("speed", Mathf.Abs(rb.velocity.x)); // Set speed based on horizontal velocity
    }

    void FixedUpdate()
    {
        // Get the horizontal input and apply horizontal movement
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
    }

    void Jump()
    {
        // Apply vertical force to jump
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        // Debug to check if the jump function is being called and the velocity is being set
        Debug.Log("Jump executed. Velocity: " + rb.velocity);
    }

    private void FlipCharacter()
    {
        // Toggle the facing direction flag
        isFacingRight = !isFacingRight;

        // Invert the X scale to flip the character horizontally
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
