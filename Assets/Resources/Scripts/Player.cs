using UnityEngine;
using System.Collections;

public class Player : CreaturePart {

	float speed = 3f;
	string[] inputStrings = {"w", "a", "s", "d"};
	bool[] inputs;

	// Use this for initialization
	protected override void Awake () {
		inputs = new bool[inputStrings.Length];
		lifespan = 5f;
		base.Awake();
		GameManager.SetPlayer(this.gameObject);
		this.gameObject.layer = Global.layerPlayer;
	}

	// given vector, change facing direction to that way
	void FaceDirection (Vector2 d) {
		float origZ = rigidbody2d.rotation;
		float targetZ = Global.Angle(Vector2.down, d);
  	rigidbody2d.rotation = Mathf.LerpAngle(origZ, targetZ, 0.3f);
	}

	protected override void Die () {
		GameManager.SetPlayerParts(AllParts);
		DetachParts();
		GameManager.FindNewPlayer();
		Destroy(this);
	}

	void FixedUpdate () {
		float dx = 0f;
		float dy = 0f;
		if (inputs[0]) {
			dy = speed;
		} else if (inputs[2]) {
			dy = -speed;
		}

		if (inputs[1]) {
			dx = -speed;
		} else if (inputs[3]) {
			dx = speed;
		}
		rigidbody2d.velocity = new Vector2(dx, dy);
		FaceDirection(rigidbody2d.velocity);
		ComputeLifeSpan();
	}
	
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < inputs.Length; i++) {
			inputs[i] = Input.GetKey(inputStrings[i]);
		}
	}
}
