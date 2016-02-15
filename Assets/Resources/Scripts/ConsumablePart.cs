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
		GetComponent<HingeJoint2D>().enabled = false;
	}

	float RandomColorValue { get { return UnityEngine.Random.Range(0.3f, 1f); } }

	float RandomLifeSpan { get { return UnityEngine.Random.Range(5, 18f); } }
	
	bool Attached { get { return GetComponent<HingeJoint2D>().enabled; } }


	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.layer == Global.layerPlayer && !Attached) {
			GetComponent<HingeJoint2D>().enabled = true;
			GetComponent<HingeJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();
			this.gameObject.layer = Global.layerPlayer;
			GameManager.AddGameObject(this.gameObject);

			ConsumablePart c = other.gameObject.GetComponent<ConsumablePart>();
			if (c != null) {
				c.AttachPart(this);
			}

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
