﻿using UnityEngine;
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

	public int PartsSize { get { return attachedparts.Count; } }


	public void AttachPart (CreaturePart c) {
		attachedparts.Add(c);
	}

	// get all the parts latched onto this one
	public List<GameObject> AllParts (int layer) {
		int maxLayers = 10;
		List<GameObject> a = new List<GameObject>();
		if (layer < maxLayers) {
			for (int i = 0; i < attachedparts.Count; i++) {
				if (attachedparts[i] != null) {
					a.AddRange(attachedparts[i].AllParts(layer+1));
					a.Add(attachedparts[i].gameObject);
				}
			}
		}
		return a;
	}

	public void ActivateAttach (bool active) {
		if (trigger2d != null) {
			trigger2d.enabled = active;
		}
	}

	public void Detach () {
		if (gameObject.layer != Global.layerDead) {
			ActivateAttach(true);
		}
		GetComponent<HingeJoint2D>().connectedBody = null;
		GetComponent<HingeJoint2D>().enabled = false;
	}

	public void DetachParts (bool shouldClear) {
		for (int i = 0; i < attachedparts.Count; i++) {
			if (attachedparts[i] != null) {
				attachedparts[i].Detach();
			}
		}
		if (shouldClear) {
			attachedparts.Clear();
		}
		
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
				rigidbody2d.drag = 0.5f;
				this.gameObject.layer = Global.layerDead;
				Die();
			} else if (IsAttachedToRealCreature) {
				lifespan -= Time.deltaTime;
				ComputeColor();
			}
		}
	}
}
