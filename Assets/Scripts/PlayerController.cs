using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	public float speed = 15.0f;
	public float padding = 0.5f;
	public float projectileSpeed = 0.5f;
	public float fireRate = 0.2f;
	public float health = 250f;
	public GameObject projectile;
	
	public AudioClip fireSound;
	
	float xmin;
	float xmax;

	// Use this for initialization
	void Start () {
		float distanceZ = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftmost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceZ));
		Vector3 rightmost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceZ));
		xmin = leftmost.x + padding;
		xmax = rightmost.x - padding;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			InvokeRepeating("Fire", 0.00001f, fireRate);  
		}
		
		if (Input.GetKeyUp(KeyCode.Space)) {
			CancelInvoke("Fire");
		}
		
		if (Input.GetKey(KeyCode.LeftArrow)) {
			//transform.position += new Vector3(-speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.left * speed * Time.deltaTime;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			//transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
			transform.position += Vector3.right * speed * Time.deltaTime;
		}
		
		//restrict the player to the gamespace
		float newX = Mathf.Clamp(transform.position.x, xmin, xmax);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
	
	void Fire() {
		
		Vector3 offset = new Vector3 (0, 1, 0);
		GameObject beam = Instantiate(projectile, transform.position + offset, Quaternion.identity) as GameObject;
		beam.rigidbody2D.velocity = new Vector3(0, projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position, 0.5f);
	}
	
	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		
		if (missile) {
			health -= missile.getDamage();
			missile.Hit();
			
			if (health <= 0) {
				LevelManager manager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
				manager.LoadLevel("Win Screen");
				Destroy(gameObject);
			}
		}
	}
}
