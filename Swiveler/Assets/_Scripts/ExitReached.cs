using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitReached : MonoBehaviour {

	public GameObject player;
	public GameObject winPanel;

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Player") {
			winPanel.SetActive(true);
			player.SetActive(false);
		}
	}
}
