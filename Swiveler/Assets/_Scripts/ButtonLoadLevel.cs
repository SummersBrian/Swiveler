/*
 * Author: Brian Summers, summers.brian.cs@gmail.com
 * Date: 5/19/15
 * 
 * Loads a specific scene by name.
 */
using UnityEngine;
using System.Collections;

public class ButtonLoadLevel : MonoBehaviour {

	public void LoadLevelButton(string levelName) {
		Application.LoadLevel (levelName);
	}	
}
