using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ExitReached : MonoBehaviour {

	public GameObject WinPanel;

	void Start() {
		WinPanel.SetActive (false);
	}

	void OnTriggerEnter(Collider col) {
		Application.LoadLevel (1);
	}
}
