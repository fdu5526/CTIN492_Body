using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
[RequireComponent (typeof (HingeJoint2D))]
public abstract class CreaturePart : MonoBehaviour {

	protected Rigidbody2D rigidbody2d;
	protected Collider2D collider2d;
	protected Collider2D trigger2d;
	protected AudioSource[] audios;
	protected float lifespan;

	bool isAttached;
	bool isDead;
	Color origColor;
	
	List<CreaturePart> attachedparts;

	// Use this for initialization
	protected virtual void Awake () {
		rigidbody2d = GetComponent<Rigidbody2D>();
		Collider2D[] c = GetComponents<Collider2D>();
		collider2d = c[0];
		if (c.Length > 1) {
			trigger2d = c[1];
		}
		origColor = GetComponent<SpriteRenderer>().color;
		audios = GetComponents<AudioSource>();
		isDead = false;
		isAttached = false;
		attachedparts = new List<CreaturePart>();
	}

	public void AttachPart (CreaturePart c) {
		isAttached = true;
		attachedparts.Add(c);
	}

	public void DetachPart () {
		if (isAttached) {
			for (int i = 0; i < attachedparts.Count; i++) {
				attachedparts[i].DetachPart();
				GetComponent<HingeJoint2D>().enabled = false;
			}
			isAttached = false;
		}
	}

	protected virtual void Die () { }

	void ComputeColor () {
		if (lifespan < 10f) {;
			GetComponent<SpriteRenderer>().color = Color.Lerp(origColor, Color.black, 1f - (lifespan / 10f));

		}
	}

	protected void ComputeLifeSpan () {
		if (!isDead) {
			if (lifespan <= 0f) {
				isDead = true;
				collider2d.enabled = false;
				if (trigger2d != null) {
					trigger2d.enabled = false;
				}
				GameManager.RemoveGameObject(this.gameObject);
				Die();
			} else if (isAttached) {
				lifespan -= Time.deltaTime;
				ComputeColor();
			}
		}
	}
}
