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
	
	bool isDead;
	
	Color origColor;
	
	public List<CreaturePart> attachedparts;

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
		attachedparts = new List<CreaturePart>();
	}


	public float Lifespan {
		get { return lifespan; }
		set { lifespan = value; }
	}

	public bool IsAttachedToRealCreature {
		get { return gameObject.layer == Global.layerPlayer; }
	}

	public bool IsDead { get { return isDead; } }


	public void AttachPart (CreaturePart c) {
		attachedparts.Add(c);
	}

	// get all the parts latched onto this one
	public List<GameObject> AllParts {
		get {
			List<GameObject> a = new List<GameObject>();
			for (int i = 0; i < attachedparts.Count; i++) {
				a.AddRange(attachedparts[i].AllParts);
				a.Add(attachedparts[i].gameObject);
			}
			return a;
		}
	}

	public void ActivateAttach (bool active) {
		if (trigger2d != null) {
			trigger2d.enabled = active;
		}
	}

	public void Detach () {
		GetComponent<HingeJoint2D>().connectedBody = null;
		GetComponent<HingeJoint2D>().enabled = false;
	}

	public void DetachParts () {
		for (int i = 0; i < attachedparts.Count; i++) {
			attachedparts[i].Detach();
		}
		attachedparts.Clear();
		Detach();
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
				Die();
			} else if (IsAttachedToRealCreature) {
				lifespan -= Time.deltaTime;
				ComputeColor();
			}
		}
	}
}
