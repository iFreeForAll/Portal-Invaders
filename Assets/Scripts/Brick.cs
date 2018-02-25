using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour {

	private int timesHit;
	private LevelManager levelManager;
	
	public Sprite[] hitSprites;
	public AudioClip crack;
	public GameObject smoke;
	
	private bool isBreakable;

	public static int breakableCount = 0;

	// Use this for initialization
	void Start () {
		isBreakable = (this.tag == "Breakable");
		// Keep track of breakable bricks
		if (isBreakable) {
			breakableCount++;
		}
		
		levelManager = GameObject.FindObjectOfType<LevelManager>();
		timesHit = 0;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
		
		AudioSource.PlayClipAtPoint (crack, transform.position, 0.5f);
		
		if (isBreakable) {
			HandleHits();
		}
		//SimulateWin();
	}
	
	void HandleHits() {
		timesHit++;
		int maxHits = hitSprites.Length + 1;
		if (timesHit >= maxHits) {
			breakableCount--;
			levelManager.BrickDestroyed();
			SmokePuff();
			Destroy(gameObject);
		} else {
			LoadSprites();
		}
	}
	
	void SmokePuff() {
		GameObject smokePuff = Instantiate (smoke, gameObject.transform.position, Quaternion.identity) as GameObject;
		smokePuff.particleSystem.startColor = gameObject.GetComponent<SpriteRenderer>().color;
	}
	
	//TODO delete this method once we can actually win
	void SimulateWin() {
		levelManager.LoadNextLevel();
	}
	
	void LoadSprites() {
		//array starts from 0, so if we hit a block one time, we need to load 1st (0) element
		int spriteIndex = timesHit - 1;
		if (hitSprites[spriteIndex]) {
			this.GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
		}
	}
}
