using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

	// Player data reference
	public PlayerData Data;

	// Rigidbody of the player
	public Rigidbody2D RB { get; private set; }

	// Flags for different player states
	public bool IsFacingRight { get; private set; }
	public bool IsJumping { get; private set; }
	public bool IsWallJumping { get; private set; }
	public bool IsSliding { get; private set; }
	public bool IsRunning;
	public bool IsIdle;

	// Time tracking variables
	public float LastOnGroundTime { get; private set; }
	public float LastOnWallTime { get; private set; }
	public float LastOnWallRightTime { get; private set; }
	public float LastOnWallLeftTime { get; private set; }

	// Jump-related flags and variables
	private bool _isJumpCut;
	private bool _isJumpFalling;
	private float _wallJumpStartTime;
	private int _lastWallJumpDir;
	private Vector2 _moveInput;
	public float LastPressedJumpTime { get; private set; }

	// Serialized fields for ground and wall checks
	[SerializeField] private Transform _groundCheckPoint;
	[SerializeField] private Vector2 _groundCheckSize = new Vector2(0.49f, 0.03f);
	[SerializeField] private Transform _frontWallCheckPoint;
	[SerializeField] private Transform _backWallCheckPoint;
	[SerializeField] private Vector2 _wallCheckSize = new Vector2(0.5f, 1f);
	[SerializeField] Vector2 boxSize;
	[SerializeField] Transform groundCheck;
	[SerializeField] LayerMask groundLayer;
	[SerializeField] private LayerMask _groundLayer;

	// Flag indicating if the player is grounded
	public bool isGrounded = false;

	// Animator component for player animations
	public Animator PlayerANIM;

	//Function that runs when script it initialized
	//Sets up Rigidbody for the player aswell as its animator
    private void Awake()
	{
		RB = GetComponent<Rigidbody2D>();
		PlayerANIM = GetComponent<Animator>();
	}

	//Function that runs at the start of the script
	//Sets the gravity scale to the refrence player data
	//And sets the player to start facing right
	void Start()
	{
		SetGravityScale(Data.gravityScale);
		IsFacingRight = true;	
	}

	//Function that runs every frame
	//Decreases timers tracking various grounded and wall times
	//Captures player input for movement along the horizontal and vertical axes
	//Checks the player's movement direction based on input
    //Responds to player input for initiating and releasing a jump
    //Determine if the player is running or idle based on their velocity
    //Check various conditions related to the player's state when not jumping
	//Manages and updates flags for jumping and wall jumping states
	void Update()
	{
        LastOnGroundTime -= Time.deltaTime;
		LastOnWallTime -= Time.deltaTime;
		LastOnWallRightTime -= Time.deltaTime;
		LastOnWallLeftTime -= Time.deltaTime;

		LastPressedJumpTime -= Time.deltaTime;

		_moveInput.x = Input.GetAxisRaw("Horizontal");
		_moveInput.y = Input.GetAxisRaw("Vertical");

		if (_moveInput.x != 0)
			CheckDirectionToFace(_moveInput.x > 0);

		if(Input.GetKeyDown(KeyCode.Space))
        {
			OnJumpInput();
        }

		if (Input.GetKeyUp(KeyCode.Space))
		{
			OnJumpUpInput();
		}

		if(GetComponent<Rigidbody2D>().velocity.magnitude < 0.5)
		{
			IsRunning = false;
			IsIdle = true;
		} 
		else if(GetComponent<Rigidbody2D>().velocity.magnitude > 0.5){
			IsRunning = true;
			IsIdle = false;
		}

		if (!IsJumping)
		{
			if (Physics2D.OverlapBox(_groundCheckPoint.position, _groundCheckSize, 0, _groundLayer) && !IsJumping) //checks if set box overlaps with ground
			{
				LastOnGroundTime = Data.coyoteTime; 
            }		
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)
					|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)) && !IsWallJumping)
				LastOnWallRightTime = Data.coyoteTime;
			if (((Physics2D.OverlapBox(_frontWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && !IsFacingRight)
				|| (Physics2D.OverlapBox(_backWallCheckPoint.position, _wallCheckSize, 0, _groundLayer) && IsFacingRight)) && !IsWallJumping)
				LastOnWallLeftTime = Data.coyoteTime;
			LastOnWallTime = Mathf.Max(LastOnWallLeftTime, LastOnWallRightTime);
		}
		if (IsJumping && RB.velocity.y < 0)
		{
			IsJumping = false;

			if(!IsWallJumping)
				_isJumpFalling = true;
		}
		if (IsWallJumping && Time.time - _wallJumpStartTime > Data.wallJumpTime)
		{
			IsWallJumping = false;
		}
		if (LastOnGroundTime > 0 && !IsJumping && !IsWallJumping)
        {
			_isJumpCut = false;

			if(!IsJumping)
				_isJumpFalling = false;
		}
		if (CanJump() && LastPressedJumpTime > 0)
		{
			IsJumping = true;
			IsWallJumping = false;
			_isJumpCut = false;
			_isJumpFalling = false;
			Jump();
		}
		else if (CanWallJump() && LastPressedJumpTime > 0)
		{
			IsWallJumping = true;
			IsJumping = false;
			_isJumpCut = false;
			_isJumpFalling = false;
			_wallJumpStartTime = Time.time;
			_lastWallJumpDir = (LastOnWallRightTime > 0) ? -1 : 1;
			
			WallJump(_lastWallJumpDir);
		}

		if (CanSlide() && ((LastOnWallLeftTime > 0 && _moveInput.x < 0) || (LastOnWallRightTime > 0 && _moveInput.x > 0)))
		{
			IsSliding = true;
		}
		else
		{
			IsSliding = false;
		}
		if (IsSliding)
		{
			SetGravityScale(0);
		}
		else if (RB.velocity.y < 0 && _moveInput.y < 0)
		{
			SetGravityScale(Data.gravityScale * Data.fastFallGravityMult);
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFastFallSpeed));
		}
		else if (_isJumpCut)
		{
			SetGravityScale(Data.gravityScale * Data.jumpCutGravityMult);
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
		}
		else if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			SetGravityScale(Data.gravityScale * Data.jumpHangGravityMult);
		}
		else if (RB.velocity.y < 0)
		{
			SetGravityScale(Data.gravityScale * Data.fallGravityMult);
			RB.velocity = new Vector2(RB.velocity.x, Mathf.Max(RB.velocity.y, -Data.maxFallSpeed));
		}
		else
		{
			SetGravityScale(Data.gravityScale);
		}
    }

	//Function thar runs once every frame
	//Controller the animation for the player
	//Controls methods for running and wall sliding with refrence to player data
    public void FixedUpdate()
	{
		AnimatorController();
		if (IsWallJumping)
			Run(Data.wallJumpRunLerp);
		else
			Run(1);
		if (IsSliding)
			Slide();
    }

	//Function that sets the timer when jump input as been pressed
    public void OnJumpInput()
	{
		LastPressedJumpTime = Data.jumpInputBufferTime;
	}

	//Function that determines long the player has jumped
	//And when no longer pressing cuts the jump
	public void OnJumpUpInput()
	{
		if (CanJumpCut() || CanWallJumpCut())
			_isJumpCut = true;
	}
	
	//Function that sets the rigidbodys gravity
    public void SetGravityScale(float scale)
	{
		RB.gravityScale = scale;
	}

	//Function that handles the movement
	//Calculate the directionto move in and  desired velocity
	//Reduce control using Lerp() this smooths changes to direction and speed
	//Gets an acceleration value based on if accelerating as well as applying a multiplier if air borne
	//Calculate difference between current velocity and desired velocity
	//Calculate force along x-axis to apply to thr player
	//Convert this to a vector and apply to rigidbody
    private void Run(float lerpAmount)
	{
		float targetSpeed = _moveInput.x * Data.runMaxSpeed;
		targetSpeed = Mathf.Lerp(RB.velocity.x, targetSpeed, lerpAmount);
		float accelRate;

		if (LastOnGroundTime > 0)
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount : Data.runDeccelAmount;
		else
			accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? Data.runAccelAmount * Data.accelInAir : Data.runDeccelAmount * Data.deccelInAir;

		if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		if(Data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
		{
			accelRate = 0; 
		}
		
		float speedDif = targetSpeed - RB.velocity.x;
		float movement = speedDif * accelRate;
		RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
	}

	//Function that controls the direction of the player
	//stores scale and flips the player along the x axis, 
	private void Turn()
	{
		Vector3 scale = transform.localScale; 
		scale.x *= -1;
		transform.localScale = scale;
		IsFacingRight = !IsFacingRight;
	}

	//Function that controls the jump
	//Ensures that the player can't call Jump multiple times from one press
	//Increases the force applied if falling
    private void Jump()
	{
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		float force = Data.jumpForce;

		if (RB.velocity.y < 0)
			force -= RB.velocity.y;

		RB.AddForce(Vector2.up * force, ForceMode2D.Impulse);
	}

	//Function that controls walljump
	//Ensures we can't call Wall Jump multiple times from one press
	//Applys force in opposite direction of the wall
	//Checks whether player is falling, if so subtracts the velocity.y 	
	private void WallJump(int dir)
	{
		LastPressedJumpTime = 0;
		LastOnGroundTime = 0;
		LastOnWallRightTime = 0;
		LastOnWallLeftTime = 0;

		Vector2 force = new Vector2(Data.wallJumpForce.x, Data.wallJumpForce.y);
		force.x *= dir; 

		if (Mathf.Sign(RB.velocity.x) != Mathf.Sign(force.x))
			force.x -= RB.velocity.x;

		if (RB.velocity.y < 0) 
			force.y -= RB.velocity.y;

		RB.AddForce(force, ForceMode2D.Impulse);
	}

	//Function that controls wall slide
	//Works the same as the Run but only in the y-axis
	//Applys y-axis velocity to the RigidBody of the player
	private void Slide()
	{
		float speedDif = Data.slideSpeed - RB.velocity.y;	
		float movement = speedDif * Data.slideAccel;
		movement = Mathf.Clamp(movement, -Mathf.Abs(speedDif)  * (1 / Time.fixedDeltaTime), Mathf.Abs(speedDif) * (1 / Time.fixedDeltaTime));

		RB.AddForce(movement * Vector2.up);
	}
    
	//Function that checks waht direction to face
	//If facing wrong way the runs tunr function
    public void CheckDirectionToFace(bool isMovingRight)
	{
		if (isMovingRight != IsFacingRight)
			Turn();
	}

	//Function that controls the bool for whether or not the player can jump 
    private bool CanJump()
    {
		return LastOnGroundTime > 0 && !IsJumping;
    }

	//Function that controls  thr bool for whether or not the player can wall jump
	private bool CanWallJump()
    {
		return LastPressedJumpTime > 0 && LastOnWallTime > 0 && LastOnGroundTime <= 0 && (!IsWallJumping ||
			 (LastOnWallRightTime > 0 && _lastWallJumpDir == 1) || (LastOnWallLeftTime > 0 && _lastWallJumpDir == -1));
	}

	//Function that controls the bool can jump cut
	private bool CanJumpCut()
    {
		return IsJumping && RB.velocity.y > 0;
    }

	//Function that controls the bool can wall jump cut
	private bool CanWallJumpCut()
	{
		return IsWallJumping && RB.velocity.y > 0;
	}

	//Function that controls the bool can slide
	public bool CanSlide()
    {
		if (LastOnWallTime > 0 && !IsJumping && !IsWallJumping && LastOnGroundTime <= 0)
			return true;
		else
			return false;
	}

	//Function that controls the animation controller
	public void AnimatorController()
	{
		PlayerANIM.SetBool("IsJumping", IsJumping);
		PlayerANIM.SetBool("IsRunning", IsRunning);
		PlayerANIM.SetBool("IsIdle", IsIdle);
	}
	
	//Draw Gizmos for visualizing objects in the Scene view
    private void OnDrawGizmosSelected()
    {
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(_groundCheckPoint.position, _groundCheckSize);
		Gizmos.color = Color.blue;
		Gizmos.DrawWireCube(_frontWallCheckPoint.position, _wallCheckSize);
		Gizmos.DrawWireCube(_backWallCheckPoint.position, _wallCheckSize);
	}
}

