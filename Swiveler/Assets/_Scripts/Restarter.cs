using System;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class Restarter : MonoBehaviour
    {

		public GameObject deathPanel;
		public GameObject player;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Player")
            {
				deathPanel.SetActive(true);
				player.SetActive(false);
            }
        }
    }
}
