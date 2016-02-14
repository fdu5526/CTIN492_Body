using UnityEngine;
using System.Collections;


[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Collider2D))]
public abstract class Circle : MonoBehaviour {

	protected Rigidbody2D rigidbody2d;
	protected Collider2D collider2d;
	protected float lifespan;

	// Use this for initialization
	protected virtual void Awake () {
		rigidbody2d = GetComponent<Rigidbody2D>();
		collider2d = GetComponent<Collider2D>();
	}


	protected void ComputeLifeSpan () {
		if (lifespan <= 0f) {
			GameManager.RemoveGameObject(this.gameObject);
		} else {
			lifespan -= Time.deltaTime;
		}
	}
}
