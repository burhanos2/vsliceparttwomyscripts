using UnityEngine;

[RequireComponent(typeof(Raycasts), typeof(PlayerInput), typeof(PlayerInfo))]
public class Movement : MonoBehaviour {

    private PlayerInput handler;
    private PlayerInfo playerInfo;
    private Rigidbody2D rb2d;
    private Raycasts rayCasts;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private Facing facing = Facing.Neutral;
    private bool canWallJump = false;


    // isjumping
    private bool isJumping = false;
    public bool Jumping { get { return isJumping; } }

    //variables
    private readonly float fallMultiplier = 6.7f, 
                           lowJumpMultiplier = 5f, 
                           jumpForce = 12.3f, 
                           velocityMinimum = 10;

    private void Awake()
    {
        handler = GetComponent<PlayerInput>();
        playerInfo = GetComponent<PlayerInfo>();
        rb2d = GetComponent<Rigidbody2D>();
        rayCasts = GetComponent<Raycasts>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        DoJump();
        CheckStates();
        ExecuteWallJump();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        DoFall();
    }

    private void CheckStates()
    {
        // check if player is not moving
        if (handler.Xaxis > -0.1f && handler.Xaxis < 0.1f)
        {
            animator.SetBool("IsMoving", false);
            facing = Facing.Neutral;
            if(rayCasts.Grounded)
            {
                rb2d.velocity = new Vector2(0, rb2d.velocity.y);
            }
        }

        if (handler.Xaxis != 0)
        {
            if (facing == Facing.Left)
            {
                spriteRenderer.flipX = true;
            }
            else if (facing == Facing.Right)
            {
                spriteRenderer.flipX = false;
            }
        }

        // check if player is touching a wall
        if (!rayCasts.DoGroundCheck()) {
            animator.SetBool("IsGrounded", false);
            animator.SetBool("InAir", true);
            if (rayCasts.DoWallCheck() != DirectionOfWall.Nothing)
            {
                animator.SetBool("IsWallJumping", true);
                canWallJump = true;
            }
        }

        if (rayCasts.DoWallCheck() == DirectionOfWall.Nothing)
        {
            animator.SetBool("IsWallJumping", false);
            canWallJump = false;
        }

        // check if player is grounded
        if (rayCasts.DoGroundCheck())
        {
            animator.SetBool("InAir", false);
            animator.SetBool("IsGrounded", true);
        }

    }

    private void MovePlayer()
    {
        if (handler.Xaxis < 0)
        {
            facing = Facing.Left;

            animator.SetBool("IsMoving", true);
            rb2d.AddForce(new Vector2(-playerInfo.Speed / 10, 0), ForceMode2D.Impulse);
        }
        if (handler.Xaxis > 0)
        {
            facing = Facing.Right;
            animator.SetBool("IsMoving", true);
            rb2d.AddForce(new Vector2(playerInfo.Speed / 10, 0), ForceMode2D.Impulse);
        }
    }

    private void DoJump()
    {
        //check if input goes down while grounded
        if (rayCasts.Grounded && handler.GetJumpButtonDown())
        {
            animator.SetBool("IsJumping", true);
            // play sound here
            isJumping = true;
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        }

        //check if input is released (up) or if no input
        if (handler.GetJumpButtonUp() || !handler.GetJumpButton())
        {
            animator.SetBool("IsJumping", false);
            isJumping = false;
        }
    }

    private void DoFall()
    {
        if (!rayCasts.Grounded)
        {
            // this changes the gravity to make the player jump low
            if (rb2d.velocity.y < velocityMinimum)
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb2d.velocity.y > velocityMinimum && !handler.GetJumpButton())
            {
                rb2d.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }

    private void ExecuteWallJump()
    {
        if (canWallJump) {
            if (rayCasts.DoWallCheck() == DirectionOfWall.Left)
            {
                if (handler.GetJumpButtonDown() && !rayCasts.Grounded)
                {
                    animator.SetBool("IsWallJumping", false);
                    canWallJump = false;
                    rb2d.velocity = new Vector2(jumpForce, jumpForce );
                }
            }
        else if (rayCasts.DoWallCheck() == DirectionOfWall.Right)
            {
                if (handler.GetJumpButtonDown() && !rayCasts.Grounded)
                {
                    animator.SetBool("IsWallJumping", false);
                    canWallJump = false;
                    rb2d.velocity = new Vector2(-jumpForce, jumpForce );
                }
            } 
        }
    }
}
