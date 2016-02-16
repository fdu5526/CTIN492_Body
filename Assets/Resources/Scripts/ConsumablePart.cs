using UnityEngine;
using System.Collections;

public class ConsumablePart : CreaturePart {

	// Use this for initialization
	protected override void Awake () {
		lifespan = RandomLifeSpan;
		transform.localScale = new Vector2(lifespan/100f, lifespan/100f);

		float a = GetComponent<SpriteRenderer>().color.a;
		GetComponent<SpriteRenderer>().color = new Color(RandomColorValue, RandomColorValue, RandomColorValue, a);
		base.Awake();
	}

	protected override void Die () {
		DetachParts();
		GameManager.RecomputePlayerParts();
	}

	float RandomColorValue { get { return UnityEngine.Random.Range(0.3f, 1f); } }

	float RandomLifeSpan { get { return UnityEngine.Random.Range(5, 18f); } }
	
	bool Attached { get { return GetComponent<HingeJoint2D>().enabled; } }


	void OnTriggerEnter2D (Collider2D other) {
		// this attaches to other
		if (other.gameObject.layer == Global.layerPlayer && !Attached) {
			GetComponent<HingeJoint2D>().enabled = true;
			GetComponent<HingeJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
			CreaturePart c = other.gameObject.GetComponent<CreaturePart>();
			if (c != null) {
				c.AttachPart(this);
			}
			this.gameObject.layer = Global.layerPlayer;

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
