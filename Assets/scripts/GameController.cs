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
		lowerBackground.GetComponent<Transform>().position = pos;
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
	// Functions to Make Clean Code
	// ---------------------------------------------------------

	Vector3 getPos(GameObject obj)
	{
		return obj.GetComponent<Transform>().position;
	}

	GameObject make(GameObject original, Vector3 position)
	{
		return Instantiate(original, position, Quaternion.identity);
	}
}
