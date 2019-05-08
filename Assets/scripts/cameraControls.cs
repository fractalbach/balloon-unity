using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
	public GameObject player;

	private float   dampTime = 0.3f;
	private Vector3 vel = Vector3.zero;
	private Vector3 offset;
	private Vector3 point;
	private Vector3 dest;
	
	void Start()
	{
		offset = transform.position - player.transform.position;
	}	

	void LateUpdate()
	{
		directFollow();
	}

	void directFollow()
	{
		Vector3 newpos = player.transform.position + offset;
		// newpos[0] = 0;
		transform.position = newpos;
		
	}
}


