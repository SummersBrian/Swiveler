using UnityEngine;
using System.Collections;

public class ButtonNextLevel : MonoBehaviour {

	public void NextLevelButton(string levelName) {
		Application.LoadLevel (levelName);
	}	
}
