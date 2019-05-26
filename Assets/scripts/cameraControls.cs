using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControls : MonoBehaviour
{
	public GameObject player;

	private Vector3 offset;
	private float   minSize = 11f;
	
	void Start()
	{
		offset = transform.position - player.transform.position;
		float w = Camera.main.orthographicSize * Screen.width / Screen.height;
		if (w < minSize) {
			Camera.main.orthographicSize = minSize;
		}
		Debug.Log(w);
	}	

	void LateUpdate()
	{
		directFollow();
	}

	void directFollow()
	{
		Vector3 newpos = player.transform.position + offset;
		newpos[0] = 0;
		transform.position = newpos;
	}

}


