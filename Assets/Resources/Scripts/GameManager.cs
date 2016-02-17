using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	static GameObject player;

	public static int numConsumedCircles = 0;
	public static int numPlayerScriptTransfer = 0;

	public static void SetPlayer (GameObject g) {
		player = g;
	}

	public static void IncreaseConsumedCircleCount () {
		numConsumedCircles++;
	}

	public static void Reset () {
		numConsumedCircles = 0;
		numPlayerScriptTransfer = 0;
	}

	public static void RecomputePlayerParts () {
		Object[] allC = Object.FindObjectsOfType(typeof(CreaturePart));
		for (int i = 0; i < allC.Length; i++) {
			CreaturePart c = (CreaturePart)allC[i];
			c.ActivateAttach(false);
			c.gameObject.layer = 0;
		}

		List<GameObject> playerParts = player.GetComponent<CreaturePart>().AllParts(0);
		for (int i = 0; i < playerParts.Count; i++) {
			playerParts[i].gameObject.layer = Global.layerPlayer;
		}
		player.gameObject.layer = Global.layerPlayer;

		for (int i = 0; i < allC.Length; i++) {
			CreaturePart c = (CreaturePart)allC[i];
			c.ActivateAttach(true);
		}

	}

	public static void FindNewPlayer () {
		numPlayerScriptTransfer++;
		List<GameObject> playerParts = player.GetComponent<CreaturePart>().AllParts(0);
		if (playerParts.Count > 0) {
				// find the best player
				GameObject g = playerParts[0];
				float maxLife = g.GetComponent<CreaturePart>().Lifespan;
				for (int i = 1; i < playerParts.Count; i++) {
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
				playerParts = player.GetComponent<CreaturePart>().AllParts(0);
				for (int i = 0; i < playerParts.Count; i++) {
					playerParts[i].gameObject.layer = Global.layerPlayer;
				}
				player.layer = Global.layerPlayer;

				// set the camera
				GameObject.Find("Main Camera").GetComponent<MainCamera>().SetNewPlayer(player);


			} else {
				GameObject.Find("Canvas").GetComponent<Canvas>().enabled = true;
			}
	}
}