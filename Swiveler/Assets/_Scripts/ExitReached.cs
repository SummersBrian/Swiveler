using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitReached : MonoBehaviour {

	public GameObject player;
	public GameObject winPanel;
	//public GameObject exitPlatform;
	private float xMin;
	private float xMax;
	private float yMin;
	private float yMax;


	void Start() {
		xMin = transform.position.x - transform.lossyScale.x;
		xMax = transform.position.x + transform.lossyScale.x;
		yMin = transform.position.y - transform.lossyScale.y;
		yMax = transform.position.y + transform.lossyScale.y;
	}

	/*
	void FixedUpdate() {
		if (player.transform.position.x > xMin && player.transform.position.x < xMax 
		    && player.transform.position.y > yMin && player.transform.position.y < yMax) {
			winPanel.SetActive(true);
			player.SetActive(false);
		}
	}
	*/

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Player") {
			winPanel.SetActive(true);
			player.SetActive(false);
		}
	}
}
