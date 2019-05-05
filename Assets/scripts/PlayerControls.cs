using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================================================
// Player Controls
// -----------------------------------------------------------------

public class PlayerControls : MonoBehaviour
{	
	private float speed         = 1f;
	private float speedOnString = 0.5f;
	
	private float MAX_SPEED_X          = 15f;
	private float MAX_SPEED_Y          = 40f;
	private float DEFAULT_GRAVITY      = 2f;
	private float DEFAULT_JUMP         = 15f;
	private float SLOWDOWN_X           = 0.1f;
	private int   MAX_GRAB_COOLDOWN    = 20;
	private int   MAX_RELEASE_COOLDOWN = 10;

	private const float xLowerLimit = -8f;
	private const float xUpperLimit = 8f;
	private const float yLowerLimit = 0f;

	private Rigidbody2D rb;
	private Animator    anim;
	private Transform   tr;

	private bool  canJump         = false;
	private float moveX           = 0f;
	private float moveY           = 0f;
	private bool  requestJump     = false;
	private bool  isGrabbing      = false;
	private int   grabCooldown    = 0;
	private int   releaseCooldown = 0;

	private GameObject leftWall; 
	private GameObject rightWall;

	// =========================================================
	// Unity Event Handlers
	// ---------------------------------------------------------

	void Start()
	{
		rb   = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
		tr   = GetComponent<Transform>();
		leftWall = GameObject.Find("LeftWall");
		rightWall = GameObject.Find("RightWall");
	}
	
	void Update()
	{
		handleInput();
	}
	
	void FixedUpdate()
	{	
		handleInput();
		handleMovement();
		ensureMaximumVelocity();
		cooldown();
		// ensurePositionWithinLimits();
		// slowdown();
		moveWallWithPlayer();
	}
	
	void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "platform") {
			resetJump();
		}
	}
	
	
	// =========================================================
	// Input and Movement
	// ---------------------------------------------------------

	/// looks at player input and saves it
	void handleInput()
	{
		moveX = Input.GetAxis("Horizontal");
		moveY = Input.GetAxis("Vertical");
		requestJump = Input.GetButton("Jump");
		requestJump |= (moveY > 0.1);
	}

	/// moves the player, based on gamestate and input
	void handleMovement()
	{
		if (requestJump && canJump) {
			jump();
		}
		if (isGrabbing) {
			handleMovementOnString();
			return;
		}
		handleDefaultMovement();
	}
	
	/// moves the player when nothing extraordinary is happening
	void handleDefaultMovement()
	{
		if (moveX > 0.1 || moveX < -0.1) {
			Vector2 v = new Vector2(moveX  * speed, 0);
			rb.AddForce(v, ForceMode2D.Impulse);
			rb.velocity = new Vector2(moveX * MAX_SPEED_X, rb.velocity.y);
		}
	}
	
	/// moves the player when they are holding onto a balloon string
	void handleMovementOnString()
	{
		Vector2 v = new Vector2(moveX, moveY);
		v *= speedOnString;
		// rb.AddForce(v, ForceMode2D.Impulse);
		rb.velocity = v;
	}

	/// sets player's velocity to max if it goes too high
	void ensureMaximumVelocity()
	{
		float vx = rb.velocity.x;
		float vy = rb.velocity.y;
		if (Mathf.Abs(vx) > MAX_SPEED_X) {
			vx = Mathf.Sign(vx) * MAX_SPEED_X;
		}
		if (Mathf.Abs(vy) > MAX_SPEED_Y) {
			vy = Mathf.Sign(vy) * MAX_SPEED_Y;
		}
		rb.velocity = new Vector2(vx, vy);
	}

	/// keeps the player inside the boundaries of the gameplay area.
	void ensurePositionWithinLimits()
	{
		float x = transform.position.x;
		float y = transform.position.y;
		Vector2 newPos = transform.position;
		Vector2 newVel = rb.velocity;
		if (x > xUpperLimit) {
			newPos[0] = xUpperLimit;
			newVel[0] = 0;
		}
		if (x < xLowerLimit) {
			newPos[0] = xLowerLimit;
			newVel[0] = 0;
		}
		if (y < yLowerLimit) {
			newPos[1] = yLowerLimit;
			newVel[1] = Mathf.Max(0, newVel[1]);
			resetJump();
		}
		transform.position = newPos;
		rb.velocity = newVel;
	}

	/// decreases the player's horizontal velocity, similar to friction.
	void slowdown()
	{
		float vx = rb.velocity.x;
		if (Mathf.Abs(vx) < SLOWDOWN_X) {
			rb.velocity = new Vector2(0, rb.velocity.y);
			return;
		}
		vx = Mathf.Sign(vx) * (Mathf.Abs(vx) - SLOWDOWN_X);
		rb.velocity = new Vector2(vx, rb.velocity.y);
	}
	
	
	// =========================================================
	// Jumping
	// ---------------------------------------------------------

	/// makes the player jump upwards, releasing anything being held.
	void jump()
	{
		if (isGrabbing) {
			releaseBalloon();
		}
		rb.AddForce(new Vector2(0, DEFAULT_JUMP), ForceMode2D.Impulse);
		// rb.velocity = new Vector2(0, DEFAULT_JUMP);
		canJump = false;
	}
	
	/// enables the player's ability to jump.
	void resetJump()
	{
		canJump = true;
	}
	
	
	// =========================================================
	// Grabbin' Stuff
	// ---------------------------------------------------------
	
	/// puts player into a state where it acts like it's grabbing
	public void grabBalloon()
	{
		if (grabCooldown > 0) {
			return;
		}
		rb.gravityScale = 0f;
		rb.velocity = new Vector2(0, 0);
		isGrabbing = true;
		canJump = true;
		grabCooldown = MAX_GRAB_COOLDOWN;
		releaseCooldown = MAX_RELEASE_COOLDOWN;
	}
	
	/// puts player into the default state: not grabbing anything.
	public void releaseBalloon()
	{
		rb.gravityScale = DEFAULT_GRAVITY;
		isGrabbing = false;
	}
	
	void cooldown()
	{
		if (grabCooldown > 0) {
			grabCooldown--;
		}
		if (releaseCooldown > 0) {
			releaseCooldown--;
		}
	}

	// =========================================================
	// !!!   HACKS  !!!    Please  do it better :)
	// =========================================================

	void moveWallWithPlayer() 
	{
		if (leftWall) {
			Vector2 pos = leftWall.transform.position;
			pos[1] = transform.position.y;
			leftWall.transform.position = pos;
		}
		if (rightWall) {
			Vector2 pos = rightWall.transform.position;
			pos[1] = transform.position.y;
			rightWall.transform.position = pos;
		}
	}
}





