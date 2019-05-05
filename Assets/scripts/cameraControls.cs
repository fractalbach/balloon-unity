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

	void FixedUpdate()
	{
		// slowFollow();
	}

	void LateUpdate()
	{
		directFollow();
	}

	void slowFollow()
	{
		point = player.transform.position;
		point[1] = player.transform.position.y + offset.y;
		dest = player.transform.position + offset;
		transform.position = Vector3.SmoothDamp(transform.position, dest, ref vel, dampTime);
	}

	void directFollow()
	{
		transform.position = player.transform.position + offset;
	}
}


