using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zeroGravity : MonoBehaviour
{
	private Collider2D coll;

	void Start()
	{
		coll = GetComponent<Collider2D>();
		coll.isTrigger = true;
	}

	// Disables gravity on all rigidbodies entering this collider.
	void OnTriggerEnter(Collider other)
	{
		if (other.attachedRigidbody) {
		    other.attachedRigidbody.useGravity = false;
		}
	}
}



