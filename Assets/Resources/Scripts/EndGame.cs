using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EndGame : MonoBehaviour {

	bool ended;

	GameObject mainCamera;
	GameObject endCanvas;

	// Use this for initialization
	void Start () {
		ended = false;
		mainCamera = GameObject.Find("Main Camera");
		endCanvas = GameObject.Find("EndCanvas");
	}

	void OnTriggerEnter2D (Collider2D other) {
		Player p = other.GetComponent<Player>();
		if (p != null && !p.Disabled) {
			// TODO win
			p.Disabled = true;
			Destroy(mainCamera.GetComponent<MainCamera>());
			ended = true;

			GameObject.Find("EndCanvas/Parts").GetComponent<Text>().text = "Number of parts collected: " + GameManager.numConsumedCircles;
			GameObject.Find("EndCanvas/Died").GetComponent<Text>().text = "Number of times the main \"you\" died: " + GameManager.numPlayerScriptTransfer;
		}
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (ended) {
			Vector2 p = Vector2.Lerp(mainCamera.transform.position, endCanvas.transform.position, 0.05f);
			mainCamera.transform.position = new Vector3(p.x, p.y, -10f);
		}
	}
}
