using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// loonString 
// handles collisions for the balloon string.

public class LoonString : MonoBehaviour
{	
	const string PLAYER_NAME = "StickPlayer";
	const string PLAYER_TAG  = "Player";

	PlayerControls player;
	
	void Awake()
	{
		GameObject p = GameObject.Find(PLAYER_NAME);
		player = p.GetComponent<PlayerControls>();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
	}
	
	void OnTriggerStay2D(Collider2D other)
	{
		if (other.gameObject.tag == PLAYER_TAG) {
			player.grabBalloon();
			player.transform.parent = transform.parent;
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


