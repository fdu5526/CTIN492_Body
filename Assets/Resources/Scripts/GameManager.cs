using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static List<GameObject> playerParts = new List<GameObject>();

	public static void AddGameObject (GameObject g) {
		playerParts.Add(g);
	}

	public static void RemoveGameObject (GameObject g) {
		playerParts.Remove(g);
	}
}
