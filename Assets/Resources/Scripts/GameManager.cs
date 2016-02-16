using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static GameObject player;

	public static void SetPlayer (GameObject g) {
		player = g;
	}


	public static void RecomputePlayerParts () {
		Object[] allC = Object.FindObjectsOfType(typeof(CreaturePart));
		for (int i = 0; i < allC.Length; i++) {
			CreaturePart c = (CreaturePart)allC[i];
			c.gameObject.layer = 0;
		}

		List<GameObject> playerParts = player.GetComponent<CreaturePart>().AllParts;
		for (int i = 0; i < playerParts.Count; i++) {
			playerParts[i].gameObject.layer = Global.layerPlayer;
		}

	}

	public static void FindNewPlayer () {
		List<GameObject> playerParts = player.GetComponent<CreaturePart>().AllParts;
		if (playerParts.Count > 0) {
				// find the best player
				GameObject g = playerParts[0];
				float maxLife = g.GetComponent<CreaturePart>().Lifespan;
				for (int i = 1; i < playerParts.Count; i++) {
					playerParts[i].layer = 0;
					if (playerParts[i].GetComponent<CreaturePart>().Lifespan > maxLife) {
						g = playerParts[i];
						maxLife = playerParts[i].GetComponent<CreaturePart>().Lifespan;
					}
				}
				// store important info
				player = g;
				player.layer = Global.layerPlayer;
				float l = player.GetComponent<ConsumablePart>().Lifespan;
				List<CreaturePart> npp = player.GetComponent<CreaturePart>().attachedparts;

				// destroy the old
				Destroy(player.GetComponent<ConsumablePart>());
				Destroy(player.GetComponents<Collider2D>()[1]);

				// transfer data
				player.AddComponent<Player>();
				player.GetComponent<Player>().Lifespan = l;
				player.GetComponent<Player>().attachedparts = npp;
				
				// set everything in player blob to be player layer
				playerParts = player.GetComponent<CreaturePart>().AllParts;
				for (int i = 0; i < playerParts.Count; i++) {
					playerParts[i].layer = Global.layerPlayer;
				}

				// set the camera
				GameObject.Find("Main Camera").GetComponent<MainCamera>().SetNewPlayer(player);


			} else {
				//TODO game over
				print("game over");
			}
	}
}