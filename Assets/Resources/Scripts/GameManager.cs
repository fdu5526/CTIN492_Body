using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static GameObject player;
	static List<GameObject> globalGlayerParts;

	public static void SetPlayer (GameObject g) {
		player = g;
	}


	public static void RecomputePlayerParts () {
		Object[] allC = Object.FindObjectsOfType(typeof(CreaturePart));
		for (int i = 0; i < allC.Length; i++) {
			CreaturePart c = (CreaturePart)allC[i];
			c.ActivateAttach(false);
			c.gameObject.layer = 0;
		}

		List<GameObject> playerParts = player.GetComponent<CreaturePart>().AllParts;
		for (int i = 0; i < playerParts.Count; i++) {
			playerParts[i].gameObject.layer = Global.layerPlayer;
		}

		for (int i = 0; i < allC.Length; i++) {
			CreaturePart c = (CreaturePart)allC[i];
			c.ActivateAttach(true);
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
				float l = player.GetComponent<ConsumablePart>().Lifespan;
				List<CreaturePart> npp = player.GetComponent<CreaturePart>().attachedparts;

				// destroy the old
				player.GetComponent<CreaturePart>().ActivateAttach(false);
				Destroy(player.GetComponent<ConsumablePart>());

				// transfer data
				player.AddComponent<Player>();
				player.GetComponent<Player>().Lifespan = l;
				player.GetComponent<Player>().attachedparts = npp;

				// reset layer informations
				RecomputePlayerParts();
				player.layer = Global.layerPlayer;

				// set the camera
				GameObject.Find("Main Camera").GetComponent<MainCamera>().SetNewPlayer(player);


			} else {
				//TODO game over
				print("game over");
			}
	}
}