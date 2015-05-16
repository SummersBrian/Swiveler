/*
 * Author: Brian Summers, summers.brian.cs@gmail.com
 * Date: 5/19/15
 * 
 * This script controls death behavior. Upon death, the character is
 * disabled and a panel becomes visible.
 */

using System;
using UnityEngine;


public class Restarter : MonoBehaviour
{

	//Modification
	public GameObject deathPanel;
	public GameObject player;

   	void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
			deathPanel.SetActive(true);
			player.SetActive(false);
        }
    }
}
