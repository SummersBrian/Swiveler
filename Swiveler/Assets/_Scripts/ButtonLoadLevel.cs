using UnityEngine;
using System.Collections;

public class ButtonLoadLevel : MonoBehaviour {

	public void LoadLevelButton(string levelName) {
		Application.LoadLevel (levelName);
	}	
}
