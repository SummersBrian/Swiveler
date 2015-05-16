/*
 * Author: Brian Summers, summers.brian.cs@gmail.com
 * Date: 5/19/15
 * 
 * This script controls the winning behavior. The player wins when they
 * reach the exit platform. Upon winning, the character is disabled and
 * a panel appears.
 */

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
