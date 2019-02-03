using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Raycasts))]
public class Jump : MonoBehaviour
{
    //classes
    private PlayerInput jumpButt;
    private Rigidbody2D rb;
    private Raycasts groundCheck;

    // isjumping
    private bool isJumping = false;
    public bool Jumping { get { return isJumping; } }

    //variables

   // [Tooltip("lowJumpMultiplier should be less than fallMultiplier")][SerializeField][Range(0.0f, 20.0f)]
    private float fallMultiplier = 6.7f, lowJumpMultiplier = 5f;

   // [SerializeField] [Tooltip("make sure jumpForce is higher than velocityMinimum")] [Range(0.1f, 20.0f)]
    private float jumpForce = 12.3f;

  //  [SerializeField][Range(0.0f, 10.0f)]
    private float velocityMinimum = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        jumpButt = GetComponent<PlayerInput>();
        groundCheck = GetComponent<Raycasts>();
    }

    private void FixedUpdate()
    {
        DoJump();
        DoFall();
    }

    private void DoJump()
    {
        //check if input goes down while grounded
        if (groundCheck.Grounded && jumpButt.GetJumpButtonDown())
        {
            // play sound here
            isJumping = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
       
        //check if input is released (up) or if no input
        if (jumpButt.GetJumpButtonUp() || !jumpButt.GetJumpButton())
        {
            isJumping = false;
        }
    }

    private void DoFall()
    {
        if (!groundCheck.Grounded)
        {
            // this changes the gravity to make the player jump low
            if (rb.velocity.y < velocityMinimum)
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > velocityMinimum && !jumpButt.GetJumpButton())
            {
                rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        } 
    }
     
}