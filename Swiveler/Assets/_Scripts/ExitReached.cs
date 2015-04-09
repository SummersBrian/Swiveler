using UnityEngine;
using System.Collections;

public class ExitReached : MonoBehaviour {

	public GameObject winPanel;

	public void OnTriggerEnter(Collider col) {
		winPanel.SetActive (true);
	}
}
