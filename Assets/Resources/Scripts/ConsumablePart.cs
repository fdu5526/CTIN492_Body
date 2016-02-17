using UnityEngine;
using System.Collections;

public class ConsumablePart : CreaturePart {

	// Use this for initialization
	protected override void Awake () {
		lifespan = RandomLifeSpan;
		transform.localScale = new Vector2(lifespan/100f, lifespan/100f);

		float a = GetComponent<SpriteRenderer>().color.a;
		GetComponent<SpriteRenderer>().color = new Color(0.31f, 0.84f, 0.15f, a);
		//GetComponent<SpriteRenderer>().color = new Color(RandomColorValue, RandomColorValue, RandomColorValue, a);
		base.Awake();

		rigidbody2d.velocity = new Vector2(RandomSpeed, RandomSpeed);

	}

	protected override void Die () {
		DetachParts(true);
		//GameManager.RecomputePlayerParts();
	}

	float RandomColorValue { get { return UnityEngine.Random.Range(0.3f, 1f); } }

	float RandomSpeed { get { return UnityEngine.Random.Range(-1f, 1f); } }

	float RandomLifeSpan { get { return UnityEngine.Random.Range(12f, 24f); } }
	
	bool Attached { get { return GetComponent<HingeJoint2D>().enabled; } }


	void OnTriggerEnter2D (Collider2D other) {
		// this attaches to other
		if (other.gameObject.layer == Global.layerPlayer && !Attached) {
			rigidbody2d.velocity = Vector2.zero;
			rigidbody2d.angularVelocity = 0f;
			GetComponent<HingeJoint2D>().enabled = true;
			GetComponent<HingeJoint2D>().connectedBody = other.GetComponent<Rigidbody2D>();

			Vector2 d = (other.transform.position - transform.position);
			GetComponent<HingeJoint2D>().connectedAnchor = 4f * d;
			ActivateAttach(false);
			CreaturePart c = other.gameObject.GetComponent<CreaturePart>();
			if (c != null) {
				c.AttachPart(this);
			}
			this.gameObject.layer = Global.layerPlayer;

			audios[0].Play();

			GameManager.IncreaseConsumedCircleCount();
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (Attached) {
			ComputeLifeSpan();
		}
		
	}
}
