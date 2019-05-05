using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// =================================================================
// Game Controller.  One Script to Rule Them All.
// -----------------------------------------------------------------

public class GameController : MonoBehaviour
{
	public static GameController instance;

	public GameObject background;
	public GameObject player;
	public GameObject balloonPrefab;
	public GameObject cutoffMarker;

	// Manage the moving backgrounds
	private GameObject lowerBG;
	private GameObject upperBG;
	private const float FIRST_BACKGROUND_Y = -12f;
	private float backgroundHeight;
	private float nextBackgroundY;

	// Creation of new balloons.
	private const float balloonSpacingY = 4f;
	private const float newBalloonLowX = -7f;
	private const float newBalloonHighX = 7f;
	private float nextBallonY = 0f;

	// private bool  youHaveLost = false;
	// private float youLoseCutoff = -10f;

	private float highestPlayerY = 0f;
	private float cutoffSize     = 30f;
	private float cutoff         = -10f;

	private const float initialPlayerY = 2f;


        // =========================================================
        // Events
        // ---------------------------------------------------------

        void Awake()
	{	
		if (instance == null) {
			instance = this;
		} 
		else if (instance != this) {
			Destroy(gameObject);
		}
	}

	void Start()
	{
		Renderer r = background.GetComponent<Renderer>();
		backgroundHeight = r.bounds.size[1];
		initBalloons();
		initBackground();
		cutoffSize = backgroundHeight/2;
	}

	void LateUpdate()
	{
		if (playerHasFallenTooFar()) {
			resetGame();
			return;
		}
		if (readyToMoveUpScreen()) {
			moveUpScreen();
		}
		if (readyToMakeNewBalloon()) {
			makeNewBalloon();
		}
		updateHighestPlayerY();
		updateCutoff();
		autoDeleteBalloonsBelow();
	}


	// =========================================================
	// Moving the Screen up as the player goes higher
	// ---------------------------------------------------------

	void initBackground()
	{
		Vector3 pos = new Vector3(0f, FIRST_BACKGROUND_Y, 0f);
		lowerBG = make(background, pos);
		pos[1] += backgroundHeight;
		upperBG = make(background, pos);
		debugBackground();
	}

        bool readyToMoveUpScreen()
	{
		return getPos(player).y > getPos(upperBG).y;
		
	}

        void moveUpScreen()
	{
		Vector3 pos = getPos(upperBG);
		pos[1] += backgroundHeight;
		setPos(lowerBG, pos);
		GameObject temp = lowerBG;
		lowerBG = upperBG;
		upperBG = temp;
		debugBackground();
	}

	void debugBackground()
	{
		Vector3 upper = getPos(upperBG);
		Vector3 lower = getPos(lowerBG);
		Debug.Log("upperY = " + upper.y + ", lowerY = " + lower.y);
	}


	// =========================================================
	// Creating Balloons
	// ---------------------------------------------------------

	void initBalloons()
	{
		nextBallonY = 0f;
		for (int i = 0; i < 50; i++) {
			makeNewBalloon();
		}
	}

	bool readyToMakeNewBalloon()
	{
		return (getPos(player).y + 2 * backgroundHeight) > nextBallonY;
	}

	void makeNewBalloon()
	{
		float x = Random.Range(newBalloonLowX, newBalloonHighX);
		float y = Random.Range(-0.5f, 0.5f) + nextBallonY;
		make(balloonPrefab, new Vector3(x, y, 0f));
		nextBallonY += balloonSpacingY;
	}
	

	// =========================================================
	// Automatic Balloon Creation and Destruction
	// ---------------------------------------------------------

	void autoDeleteBalloonsBelow()
	{
		float lowY = cutoff;
		GameObject[] gos = GameObject.FindGameObjectsWithTag("balloon");
		foreach (GameObject go in gos) {
			if (getPos(go)[1] < lowY) {
				Destroy(go);
			}
		}
	}
	

	// =========================================================
	// Restarting Game From Falling
	// ---------------------------------------------------------

	// TODO:
	// FIND A BETTER WAY TO RESTART THE GAME INSTANCE.
	// Currently, it's hacky. Reseting the game by directly changing each
	// variable is not-scalable as the game increases its complexity. Every
	// new variable that is introduced will need to be added here, and it
	// will SUCK and will create bugs later on.

	bool playerHasFallenTooFar()
	{
		return getPos(player).y < cutoff;
	}

	void resetGame() 
	{
		Debug.Log("You have LOST! Resetting Game.");
		resetPlayer();
		resetBackground();
		resetBalloons();
		resetCutoff();
	}

	void resetPlayer()
	{
		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
		rb.velocity = Vector3.zero;
		rb.angularVelocity = 0f;
		player.transform.position = new Vector3(0, initialPlayerY, 0);
	}

	void resetBackground()
	{
		Vector3 pos = new Vector3(0f, FIRST_BACKGROUND_Y, 0f);
		setPos(lowerBG, pos);
		pos[1] += backgroundHeight;
		setPos(upperBG, pos);
	}

	void resetBalloons()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag("balloon");
		foreach (GameObject go in gos) {
			Destroy(go);
		}
		initBalloons();
	}

	void resetCutoff()
	{
		highestPlayerY = 0f;
		cutoff = -10f;
		updateCutoff();
	}

	// =========================================================
	// Cutoff Points
	// ---------------------------------------------------------

	void updateHighestPlayerY()
	{
		float playerY = getPos(player).y;
		if (highestPlayerY < playerY) {
			highestPlayerY = playerY;
		}
	}

	void updateCutoff()
	{
		cutoff = highestPlayerY - cutoffSize;
		setPosY(cutoffMarker, cutoff);
	}


	// =========================================================
	// Functions to Make Clean Code
	// ---------------------------------------------------------

	Vector3 getPos(GameObject go)
	{
		return go.transform.position;
	}

	void setPos(GameObject go, Vector3 position)
	{
		go.transform.position = position;
	}

	void setPosY(GameObject go, float y) {
		Vector3 pos = go.transform.position;
		pos[1] = y;
		go.transform.position = pos;
	}

	void setPosX(GameObject go, float x) {
		Vector3 pos = go.transform.position;
		pos[0] = x;
		go.transform.position = pos;
	}

	GameObject make(GameObject original, Vector3 position)
	{
		return Instantiate(original, position, Quaternion.identity);
	}
}