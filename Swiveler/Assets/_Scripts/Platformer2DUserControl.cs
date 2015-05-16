/*
 * Author: Brian Summers, summers.brian.cs@gmail.com
 * Date: 5/19/15
 * This file has been modified from its original version in the standard assets package.
 * 
 * This script controls character movement and user input. Modifications include: user's ability
 * to control pieces in the scene, and user can access an in game menu.
 */
using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets._2D
{
    [RequireComponent(typeof (PlatformerCharacter2D))]
    public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D m_Character;
        private bool m_Jump;
		public GameObject rotatingSquaresContainer;
		public GameObject menu;
		private bool menu_showing = false;

        private void Awake()
        {
            m_Character = GetComponent<PlatformerCharacter2D>();
        }


        private void Update()
        {
			/*
			 * Modification : added user control of menu and pieces in the scene
			 */
			if (rotatingSquaresContainer != null) {
				if (rotatingSquaresContainer.transform.childCount > 0) {
					if (Input.GetKeyDown (KeyCode.Alpha1)) {
						Transform t = rotatingSquaresContainer.transform.GetChild (0);
						for (int i = 0; i < t.childCount; i++) {
							Transform c = t.GetChild (i);
							float currRot = c.eulerAngles.z;
							currRot = Mathf.Floor (currRot + 90.0f);
							if (currRot == 360.0f)
								currRot = 0.0f;
							c.rotation = Quaternion.AngleAxis (currRot, c.forward);
						}
					}
				}

				if (rotatingSquaresContainer.transform.childCount > 1) {
					if (Input.GetKeyDown (KeyCode.Alpha2)) {
						Transform t = rotatingSquaresContainer.transform.GetChild (1);
						for (int i = 0; i < t.childCount; i++) {
							Transform c = t.GetChild (i);
							float currRot = c.eulerAngles.z;
							currRot = Mathf.Floor (currRot + 90.0f);
							if (currRot == 360.0f)
								currRot = 0.0f;
							c.rotation = Quaternion.AngleAxis (currRot, c.forward);
						}
					}
				}
			}

			if (Input.GetKeyDown (KeyCode.Escape)) {
				if (!menu_showing) {
					menu_showing = true;
					menu.SetActive (true);
				} else {
					menu_showing = false;
					menu.SetActive (false);
				}
			}
			/*
			 * End  of modification
			 */


			if (!m_Jump) {
				// Read the jump input in Update so button presses aren't missed.
				m_Jump = CrossPlatformInputManager.GetButtonDown ("Jump");
			}
        }


        private void FixedUpdate()
        {
			if (!menu_showing) {
				// Read the inputs.
				bool crouch = Input.GetKey (KeyCode.LeftControl);

				//Modification: changed the input axis from "horizontal" to "WASD"
				float dir = Input.GetAxis ("WASD");

				// Pass all parameters to the character control script.
				m_Character.Move (dir, crouch, m_Jump);

				m_Jump = false;
			}
		}
        
    }
}
