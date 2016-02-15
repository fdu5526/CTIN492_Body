using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public class ConsumableCircle : Circle {

	// Use this for initialization
	protected override void Awake () {
		lifespan = RandomLifeSpan;
		transform.localScale = new Vector2(lifespan/100f, lifespan/100f);

		float a = GetComponent<SpriteRenderer>().color.a;
		GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, a);
		base.Awake();
	}

	protected override void Die () {
		GetComponent<HingeJoint2D>().enabled = false;
	}

	float RandomLifeSpan { get { return UnityEngine.Random.Range(5, 18f); } }
	
	bool Attached { get { return GetComponent<HingeJoint2D>().enabled; } }


	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == Global.layerPlayer && !Attached) {
			GetComponent<HingeJoint2D>().enabled = true;
			GetComponent<HingeJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
			this.gameObject.layer = Global.layerPlayer;
			GameManager.AddGameObject(this.gameObject);
			audios[0].Play();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Attached) {
			ComputeLifeSpan();
		}
		
	}
}
