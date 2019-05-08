using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonController : MonoBehaviour
{
	/// the number of seconds the balloon shows "POP!" before vanishing
	const float secondsForPop = 1;

	/// Pops the balloon! It will vanish after a short delay.
	void pop()
	{
		changeBalloonToPopImage();
		StartCoroutine(delayedDelete());
	}

	IEnumerator delayedDelete()
	{
		yield return new WaitForSeconds(secondsForPop);
		Destroy(gameObject);
	}

	/// displays the image that says "POP!" where the ballon used to be
	void changeBalloonToPopImage()
	{
		transform.Find("pop").gameObject.SetActive(true);
		transform.Find("red_balloon").gameObject.SetActive(false);
	}

}
