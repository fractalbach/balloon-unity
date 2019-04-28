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
	}


	// =========================================================
	// Moving the Screen up as the player goes higher
	// ---------------------------------------------------------

	void initBackground()
	{
		Vector3 pos = new Vector3(0f, FIRST_BACKGROUND_Y, 0f);
		lowerBackground = Instantiate(background, pos, Quaternion.identity);
		pos[1] += backgroundHeight;
		upperBackground = Instantiate(background, pos, Quaternion.identity);
	}

	bool readyToMoveUpScreen()
	{
		float playerY = getPos(player).y;
		float backgroundY = getPos(upperBackground).y;
		if (playerY > backgroundY) {
			Debug.Log("Background is Ready to move up.");
			return true;
		}
		return false;
	}

	void moveUpScreen()
	{
		Destroy(lowerBackground);
		lowerBackground = upperBackground;
		createUpperBackground();
		Vector3 upper = getPos(upperBackground);
		Vector3 lower = getPos(lowerBackground);
		Debug.Log("upperY = " + upper.y + ", lowerY = " + lower.y);
	}

	void createUpperBackground()
	{
		Vector3 pos = getPos(lowerBackground);
		pos[1] += backgroundHeight;
		upperBackground = Instantiate(background, pos, Quaternion.identity);
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
			Instantiate(balloonPrefab, pos, Quaternion.identity);
		}
	}

	
	// =========================================================
	// Functions to Make Clean Code
	// ---------------------------------------------------------

	Vector3 getPos(GameObject obj)
	{
		return obj.GetComponent<Transform>().position;
	}
}
