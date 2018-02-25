using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	private Paddle paddle;
	private Vector3 paddleToBallVector;
	private bool hasStarted = false;
	
	public AudioClip beep;
	
	// Use this for initialization
	void Start () {
		paddle = GameObject.FindObjectOfType<Paddle>();
		paddleToBallVector = this.transform.position - paddle.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
		//following the paddle (locked)
		if(!hasStarted) {
			this.transform.position = paddle.transform.position + paddleToBallVector;
			
			//launching ball by clicking Left Mouse 
			if(Input.GetMouseButtonDown(0)) {
				hasStarted = true;
				this.rigidbody2D.velocity = new Vector2 (2f, 10f); 
			}
		}
	}
	
	void OnCollisionEnter2D (Collision2D collision) {
	
		Vector2 tweak = new Vector2 (Random.Range(0f, 0.2f), Random.Range(0f, 0.2f));
		
		if (hasStarted) {
			AudioSource.PlayClipAtPoint (beep, transform.position, 0.3f);
			rigidbody2D.velocity += tweak;
		}
	}
}
