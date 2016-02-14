using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public class ConsumableCircle : Circle {

	// Use this for initialization
	protected override void Awake () {
		base.Awake();
		lifespan = RandomLifeSpan;
		GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value, 1.0f);
	}


	float RandomLifeSpan { get { return UnityEngine.Random.Range(5, 18f); } }
	
	bool Attached { get { return GetComponent<HingeJoint2D>().enabled; } }


	void ComputeScale () {
		float s = lifespan / 100f;
		Vector2 v = transform.localScale;
		v.Set(s,s);
		transform.localScale = v;
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
			ComputeScale();
		}
		
	}
}
