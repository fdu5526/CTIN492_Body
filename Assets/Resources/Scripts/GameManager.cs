using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static List<GameObject> playerParts = new List<GameObject>();
	static GameObject player;

	public static void SetPlayer (GameObject g) {
		player = g;
	}

	public static void FindNewPlayer () {
		playerParts = player.GetComponent<CreaturePart>().AllParts;
		if (playerParts.Count > 0) {
				
				GameObject g = playerParts[0];
				float maxLife = g.GetComponent<CreaturePart>().Lifespan;
				for (int i = 1; i < playerParts.Count; i++) {
					if (playerParts[i].GetComponent<CreaturePart>().Lifespan > maxLife) {
						g = playerParts[i];
						maxLife = playerParts[i].GetComponent<CreaturePart>().Lifespan;
					}
				}
				player = g;

				float l = player.GetComponent<ConsumablePart>().Lifespan;
				Destroy(player.GetComponent<ConsumablePart>());
				Destroy(player.GetComponents<Collider2D>()[1]);
				player.AddComponent<Player>();
				player.GetComponent<Player>().Lifespan = l;
				GameObject.Find("Main Camera").GetComponent<MainCamera>().SetNewPlayer(player);
			} else {
				//TODO game over
				print("game over");
			}
	}
}