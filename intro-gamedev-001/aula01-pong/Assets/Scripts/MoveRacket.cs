using UnityEngine;
using System.Collections;

public class MoveRacket : MonoBehaviour {

	public float speed = 20;
	public string axis = "Vertical";

	Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}
	
	void FixedUpdate () {
		if (!Score.isGameOver())
		{
			float vertical = Input.GetAxisRaw(axis);
			rb2d.velocity = new Vector2(0, vertical) * speed;
		}
		else
		{
			rb2d.velocity = new Vector2(0, 0);
			transform.position = new Vector3(transform.position.x, 0, 0);
		}
	}
}
