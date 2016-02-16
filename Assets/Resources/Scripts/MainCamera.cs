using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").gameObject;
	}

	public void SetNewPlayer (GameObject g) {
		player = g;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 pp = player.transform.position;
		Vector2 tp = transform.position;
		tp = Vector2.Lerp(tp, pp, 0.2f);
		GetComponent<Transform>().position = new Vector3(tp.x, tp.y, -10f);

	}
}
