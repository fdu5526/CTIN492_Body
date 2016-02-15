using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static List<GameObject> playerParts = new List<GameObject>();

	public static void AddGameObject (GameObject g) {
		playerParts.Add(g);
	}

	public static void FindNewPlayer () {
		if (playerParts.Count > 0) {
				GameObject g = playerParts[0];
				playerParts.Remove(g);
				Destroy(g.GetComponent<ConsumablePart>());
				g.AddComponent<Player>();
			} else {
				//TODO game over
			}
	}

	public static void RemoveGameObject (GameObject g) {
		playerParts.Remove(g);
	}
}