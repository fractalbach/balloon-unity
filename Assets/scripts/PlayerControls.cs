using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// =================================================================
// Player Controls
// -----------------------------------------------------------------

public class PlayerControls : MonoBehaviour
{	
	private const float speed         = 5f;
	private const float maxSpeed      = 10f;
	private const float speedOnString = 2f;

	private Rigidbody2D rb;
	private Animator    anim;
	
	private const float DEFAULT_GRAVITY   = 2f;
	private const int   MAX_GRAB_COOLDOWN = 20;
	
	private bool  canJump      = false;
	private float moveX        = 0f;
	private float moveY        = 0f;
	private bool  requestJump  = false;
	private bool  isGrabbing   = false;
	private int   grabCooldown = 0;


	// =========================================================
	// Unity Event Handlers
	// ---------------------------------------------------------

	void Start()
	{
		rb   = GetComponent<Rigidbody2D>();
		anim = GetComponent<Animator>();
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

	// handleInput looks at the player's input and updates
	// variables. It does nothing directly to the game, so it
	// can be called freely without worry.
	void handleInput()
	{
		moveX = Input.GetAxis("Horizontal");
		moveY = Input.GetAxis("Vertical");
		requestJump = Input.GetButton("Jump");
		requestJump |= (moveY > 0.1);
	}

	// handleMovement affects the movement of in-game character.
	// Makes use of the stored player input and the state 
	// of the game. Decides how the player should move.
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
	
	// handleDefaultMovement moves the player when they are in
	// normal environmental conditions. They aren't grabbing
	// anything etc.
	void handleDefaultMovement()
	{
		Vector2 v = new Vector2(moveX, 0);
		v *= speed;
		rb.AddForce(v, ForceMode2D.Impulse);
	}
	
	// handleMovementOnString moves the player when they are
	// holding onto a balloon string.
	void handleMovementOnString()
	{
		Vector2 v = new Vector2(moveX, moveY);
		v *= speedOnString;
		// rb.AddForce(v, ForceMode2D.Impulse);
		rb.velocity = v;
	}
	
	// Puts a cap on the maximum speed a player can go, and
	// forces that velocity to within the limit.
	void ensureMaximumVelocity()
	{
		float vx = rb.velocity.x;
		float vy = rb.velocity.y;
		if (Mathf.Abs(vx) > maxSpeed) {
			vx = Mathf.Sign(vx) * maxSpeed;
		}
		if (Mathf.Abs(vy) > maxSpeed) {
			vy = Mathf.Sign(vy) * maxSpeed;
		}
		rb.velocity = new Vector2(vx, vy);
	}
	
	
	// =========================================================
	// Jumping
	// ---------------------------------------------------------

	void jump()
	{
		if (isGrabbing) {
			releaseBalloon();
		}
		rb.AddForce(new Vector2(0, 10), ForceMode2D.Impulse);
		canJump = false;
	}
	
	void resetJump()
	{
		canJump = true;
	}
	
	
	// =========================================================
	// Grabbin' Stuff
	// ---------------------------------------------------------
	
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
	}
	
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
	}
}





