using UnityEngine;
using System.Collections;

public class MainCamera : MonoBehaviour {

	GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player").gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector3 pp = player.transform.position;
		GetComponent<Transform>().position = new Vector3(pp.x, pp.y, -10f);

	}
}
