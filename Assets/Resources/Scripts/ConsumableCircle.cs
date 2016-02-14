using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public class ConsumableCircle : Circle {

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		lifespan = RandomLifeSpan;
		transform.localScale = new Vector2(lifespan/100f, lifespan/100f);
		GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 1.0f);
	}


	float RandomLifeSpan { get { return UnityEngine.Random.Range(5, 18f); } }
	
	bool Attached { get { return GetComponent<HingeJoint2D>().enabled; } }


	void ComputeColor () {
		if (lifespan < 5f) {
			float rate = 0.03f;
			Color c = GetComponent<SpriteRenderer>().color;
			c.r = c.r - rate;
			c.g = c.g - rate;
			c.b = c.b - rate;
			GetComponent<SpriteRenderer>().color = c;

		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == Global.layerPlayer && !Attached) {
			GetComponent<HingeJoint2D>().enabled = true;
			GetComponent<HingeJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
			this.gameObject.layer = Global.layerPlayer;
			GameManager.AddGameObject(this.gameObject);
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Attached) {
			ComputeColor();
		}
		
	}
}
