using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControls : MonoBehaviour
{
	public float speed = 5f;
	public float maxSpeed = 5f;
	private Rigidbody2D rb;
	private Animator anim;

	void Start()
	{
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}
	
	void Update()
	{
		HandleSword();
	}
	
	void FixedUpdate()
	{	
		HandleMovement();
	}
	
	void HandleMovement()
	{
		if (rb.velocity.magnitude > maxSpeed) {
			rb.velocity = rb.velocity.normalized * maxSpeed;
			return;
		}
		float moveX = Input.GetAxis ("Horizontal");
		float moveY = Input.GetAxis ("Vertical");
		Vector2 movement = new Vector2 (moveX, moveY);
		rb.AddForce (movement * speed);
	}


	// Another potential input button/thing to use is:
	// 	Input.GetButton("Fire1")
	// but currently it has some unexpected results.	
	void HandleSword()
	{
		if (Input.GetKeyDown(KeyCode.Space)) {
			anim.SetTrigger("Slash");
		}
	}

	
	
}
