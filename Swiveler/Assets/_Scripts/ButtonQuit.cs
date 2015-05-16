/*
 * Author: Brian Summers, summers.brian.cs@gmail.com
 * Date: 5/19/15
 * 
 * This script defines the quit button behavior. The script simply
 * quits the application.
 */

using UnityEngine;
using System.Collections;

public class ButtonQuit : MonoBehaviour {

	public void QuitButton() {
		Application.Quit ();
	}
}
