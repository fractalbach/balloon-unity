using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// loonString 
// handles collisions for the balloon string.

public class LoonString : MonoBehaviour
{	
	const string PLAYER_NAME = "StickPlayer";
	const string PLAYER_TAG  = "Player";

	bool neverBeenGrabbedBefore = true;
	PlayerControls player;
	BalloonController balloonController;

	
	void Awake()
	{
		GameObject p = GameObject.Find(PLAYER_NAME);
		player = p.GetComponent<PlayerControls>();
	}

	void Start()
	{
		balloonController = transform.parent.parent.GetComponent<BalloonController>();
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == PLAYER_TAG) {
			player.grabBalloon();
			player.transform.parent = transform.parent;
			if (neverBeenGrabbedBefore) {
				balloonController.BeginRising();
				neverBeenGrabbedBefore = false;
			}
		}
	}
	
	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == PLAYER_TAG) {
			player.releaseBalloon();
			player.transform.parent = null;
		}
	}
}


