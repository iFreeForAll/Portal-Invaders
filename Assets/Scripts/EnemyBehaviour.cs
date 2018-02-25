using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public float health = 150;
	public GameObject projectile;
	public float projectileSpeed = 10f;
	public float shotsPerSecond = 0.5f;
	public int scoreValue = 150;
	
	public AudioClip dieSound;
	public AudioClip fireSound;
	
	private ScoreKeeper scoreKeeper;
	
	void Start() {
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper>();
	}
	
	void Update() {
	
		float probability = Time.deltaTime * shotsPerSecond;
		if(Random.value < probability) {
			Fire ();
		}
	}
	
	void Fire() {
		Vector3 startPosition = transform.position + new Vector3 (0, -1, 0);
		GameObject missile = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
		missile.rigidbody2D.velocity = new Vector3 (0, -projectileSpeed, 0);
		AudioSource.PlayClipAtPoint(fireSound, transform.position, 1f);
	}

	void OnTriggerEnter2D (Collider2D collider) {
		Projectile missile = collider.gameObject.GetComponent<Projectile>();
		
		if (missile) {
			health -= missile.getDamage();
			missile.Hit();
			
			if (health <= 0) {
				Die ();
			}
		}
	}
	
	void Die() {
		AudioSource.PlayClipAtPoint(dieSound, transform.position, 100f);
		Destroy(gameObject);
		scoreKeeper.Score(scoreValue);
	}
}
