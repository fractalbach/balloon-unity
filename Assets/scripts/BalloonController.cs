using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
	// =========================================================
	// Constants and Variables
	// ---------------------------------------------------------

	/// the player's name
	const string PLAYERNAME = "StickPlayer";

	/// default rising speed of the balloon when it's activated.
	const float risingSpeedY = 0.1f;

	/// the number of seconds the balloon shows "POP!" before vanishing
	const float secondsForPop = 1;
   	
	/// Delta y: the distance that the balloon can last before it pops
	float dy = 10f;

	/// initial y position
	float y0;

	/// state: true if the balloon has begun to rise.
	bool isRising = false;


	// Storage Variables:

	Vector3 nextPos;
	Transform container;
	GameObject popObject;
	GameObject redBalloonObject;

	
	// =========================================================
	// Event Handlers
	// ---------------------------------------------------------

	void Start()
	{
		y0 = transform.position.y;
		container = transform.Find("BalloonDriftContainer").gameObject.transform;
		popObject = container.Find("pop").gameObject;
		redBalloonObject = container.Find("red_balloon").gameObject;
	}

	void FixedUpdate()
	{
		if (isRising) {
			rise();
		}
		if (transform.position.y > (y0 + dy)) {
			pop();
		}
	}


	// =========================================================
	// Public Functions
	// ---------------------------------------------------------

	/// Begin the balloon's ascension toward the heavens! Godspeed, Balloon!
	public void BeginRising()
	{
		isRising = true;
	}

	/// set delta y: how high the balloon can go before it pops
	public void SetBalloonLifeDistance(float deltaY)
	{
		dy = deltaY;
	}


	// =========================================================
	// Rise Mechanics
	// ---------------------------------------------------------

	void rise()
	{
		nextPos = transform.position;
		nextPos[1] += risingSpeedY;
		transform.position = nextPos;
	}


	// =========================================================
	//                 ! <<< ~  P O P  ~ >>>  !
	// ---------------------------------------------------------

	/// Pops the balloon! It will vanish after a short delay.
	void pop()
	{
		changeBalloonToPopImage();
		StartCoroutine(delayedDelete());
	}

	IEnumerator delayedDelete()
	{
		yield return new WaitForSeconds(secondsForPop);
		destroyBalloon();
	}

	void destroyBalloon()
	{
		Transform player = container.Find(PLAYERNAME);
		if (player != null) {
			player.parent = null;
			player.GetComponent<PlayerControls>().releaseBalloon();
		}
		Destroy(gameObject);
	}

	/// displays the image that says "POP!" where the ballon used to be
	void changeBalloonToPopImage()
	{
		popObject.SetActive(true);
		redBalloonObject.SetActive(false);
	}

}
