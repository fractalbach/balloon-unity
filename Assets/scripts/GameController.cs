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

	private Transform playerTransform;
	private Transform backgroundTransform;
	private Renderer  backgroundRenderer;

	private GameObject lowerBackground;
	private GameObject upperBackground;

	private const float FIRST_BACKGROUND_Y = 12f;
	private float backgroundHeight;
	private float nextBackgroundY;

	private float lowerCutoff = 0f;

	private bool  youHaveLost = false;
	private float youLoseCutoff = -10f;
	

	
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
		playerTransform = player.GetComponent<Transform>();
		backgroundTransform = background.GetComponent<Transform>();
		backgroundRenderer = background.GetComponent<Renderer>();
		backgroundHeight = backgroundRenderer.bounds.size[1];
		createSomeBalloonsAtStart();
		initBackground();
	}

	void Update()
	{
		if (readyToMoveUpScreen()) {
			moveUpScreen();
		}
		autoDeleteBalloonsBelow();
	}

	void LateUpdate()
	{
		if (playerHasFallenTooFar()) {
			resetGame();
		}
	}


	// =========================================================
	// Moving the Screen up as the player goes higher
	// ---------------------------------------------------------

	void initBackground()
	{
		Vector3 pos = new Vector3(0f, FIRST_BACKGROUND_Y, 0f);
		lowerBackground = make(background, pos);
		pos[1] += backgroundHeight;
		upperBackground = make(background, pos);
		debugBackground();
	}

	bool readyToMoveUpScreen()
	{
		float playerY = getPos(player).y;
		float backgroundY = getPos(upperBackground).y;
		if (playerY > backgroundY) {
			return true;
		}
		return false;
	}

	void moveUpScreen()
	{
		Vector3 pos = getPos(upperBackground);
		pos[1] += backgroundHeight;
		setPos(lowerBackground, pos);
		GameObject temp = lowerBackground;
		lowerBackground = upperBackground;
		upperBackground = temp;
		debugBackground();
	}

	void debugBackground()
	{
		Vector3 upper = getPos(upperBackground);
		Vector3 lower = getPos(lowerBackground);
		Debug.Log("upperY = " + upper.y + ", lowerY = " + lower.y);
	}


	// =========================================================
	// Create Some Balloons When the Game Starts
	// ---------------------------------------------------------

	void createSomeBalloonsAtStart()
	{
		float x, y, z;
		Vector3 pos;
		for (int i = 0; i < 100; i++) {
			x = Random.Range(-5f, 5f);
			y = Random.Range(-1f, 1f) + 2 * i;
			z = 0f;
			pos = new Vector3(x, y, z);
			make(balloonPrefab, pos);
		}
	}

	
	// =========================================================
	// Automatic Balloon Creation and Destruction
	// ---------------------------------------------------------

	void autoDeleteBalloonsBelow()
	{
		lowerCutoff = getPos(lowerBackground)[1] - backgroundHeight / 2;
		GameObject[] gos = GameObject.FindGameObjectsWithTag("balloon");
		foreach (GameObject go in gos) {
			if (getPos(go)[1] < lowerCutoff) {
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
		float low = lowerBackground.GetComponent<Renderer>().bounds.min.y;
		float playerY = getPos(player).y;
		if (playerY < low) {
			return true;
		}
		return false;
	}

	void resetGame() 
	{
		Debug.Log("You have LOST! Resetting Game.");
		resetPlayer();
		resetBackground();
		resetBalloons();
	}

	void resetPlayer()
	{
		Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
		rb.velocity = Vector3.zero;
		setPos(player, Vector3.zero);
	}

	void resetBackground()
	{
		Vector3 pos = new Vector3(0f, FIRST_BACKGROUND_Y, 0f);
		setPos(lowerBackground, pos);
		pos[1] += backgroundHeight;
		setPos(upperBackground, pos);
	}

	void resetBalloons()
	{
		GameObject[] gos = GameObject.FindGameObjectsWithTag("balloon");
		foreach (GameObject go in gos) {
			Destroy(go);
		}
	}

	// =========================================================
	// Functions to Make Clean Code
	// ---------------------------------------------------------

	Vector3 getPos(GameObject go)
	{
		return go.GetComponent<Transform>().position;
	}

	void setPos(GameObject go, Vector3 position)
	{
		go.GetComponent<Transform>().position = position;
	}

	GameObject make(GameObject original, Vector3 position)
	{
		return Instantiate(original, position, Quaternion.identity);
	}
}
