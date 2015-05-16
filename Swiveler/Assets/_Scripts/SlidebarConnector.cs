	/*
	* Author: Brian Summers, summers.brian.cs@gmail.com
	* Date: 5/19/15
	* 
	* This script defines the behavior of slidebars. Slidebars allow for
	* slide platforms to move along them. In order for a platform to be
	* "slideable" there have to be two slidebars in contact with one
	* another. This script will define the minimum and maximum sliding distance
	* of the slide platform based on the slidebar's orientation.
	*/

	using UnityEngine;
	using System.Collections;

	public class SlidebarConnector : MonoBehaviour {

	public GameObject platform; //the platform attached to the slidebar connect (can be null)
	public GameObject platformSlot; //the position at the end of this slidebar connector
	public GameObject rotatingSquare; //the parent rotating piece of this slidebar (can be null)

	public enum Orientation{Vertical,Horizontal};
	private Orientation myOrientation;
	private BoxCollider2D myCollider;
	private float max;
	private float min;
	public GameObject connectedPlatformSlot; //the end position of a connected slidebar (can be null)
	public SlidebarConnector connectedSlideBar; //a slidebar in contact with this slide bar
	public bool horizontal;

	void Start() {
		myCollider = GetComponent<BoxCollider2D> ();
		if (horizontal)
			myOrientation = Orientation.Horizontal;
		else
			myOrientation = Orientation.Vertical;
	}

	/// <summary>
	/// Raises the trigger enter2 d event. When two slidebars start connection with one another.
	/// Sets this slidebar's fields to reflect its connected slidebar and if this slidebar contains
	/// a platform, sets the min and max movement of the platform.
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerEnter2D(Collider2D collider) {
			
		//if this slidebar has a platform attached
		if (platform != null) {

			//and this collision is with another slidebar
			if (collider.transform.tag == "SlideBar") {

				findOrientation ();

				//script of the attached platform
				PlatformDrag platformScript = platform.GetComponent<PlatformDrag> ();

				//set the connected slidebar
				connectedSlideBar = collider.GetComponent<SlidebarConnector> ();

				//if the connected slidebar has a platform already, we can't slide, so just return
				if (connectedSlideBar.platform != null) {
					return;
				}

				//the end of the connect slidebar
				connectedPlatformSlot = connectedSlideBar.platformSlot;
				//tell the other slidebar that it's connected to this one
				connectedSlideBar.connectedSlideBar = GetComponent<SlidebarConnector> ();
				connectedSlideBar.connectedPlatformSlot = platformSlot;
				platformScript.CanSlide (true);
				if (myOrientation == Orientation.Horizontal) {
					//if our min is smaller, then our min is lowest the platform can slide, and his max is the max it can slide
					if (platformSlot.transform.position.x < connectedPlatformSlot.transform.position.x) {
						min = platformSlot.transform.position.x;
						max = connectedPlatformSlot.transform.position.x;
					} else {
						min = connectedPlatformSlot.transform.position.x;
						max = platformSlot.transform.position.x;
					}
				} else {
					//same but for y direction if we're vertical
					if (platformSlot.transform.position.y < connectedPlatformSlot.transform.position.y) {
						min = platformSlot.transform.position.y;
						max = connectedPlatformSlot.transform.position.y;
					} else {
						min = connectedPlatformSlot.transform.position.y;
						max = platformSlot.transform.position.y;
					}
				}

				//set the platform's min and max slide distances
				platformScript.setSlideMax (max);
				platformScript.setSlideMin (min);
			} 
		}
	}


	/// <summary>
	/// Raises the trigger exit 2d event. When two slidebars end connection with one another.
	/// Sets this slidebar's min and max, and if this slidebar contains a platform, sets
	/// the platforms min and max movement (which is none)
	/// </summary>
	/// <param name="collider">Collider.</param>
	void OnTriggerExit2D(Collider2D collider) {
		findOrientation ();

		//if we are leaving contact with another slidebar
		if (collider.gameObject.CompareTag ("SlideBar")) {
			//set to null because we're no longer connected
			connectedSlideBar = null;

			//mins and maxes are our own
			if (myOrientation == Orientation.Horizontal) {
				min = myCollider.bounds.min.x;
				max = myCollider.bounds.max.x;
			} else {
				min = myCollider.bounds.min.y;
				max = myCollider.bounds.max.y;
			}
	
			//if we contain a platform
			if (platform != null) {
				//get the platform's script
				PlatformDrag platformScript = platform.GetComponent<PlatformDrag> ();
				//platform can no longer slide
				platformScript.CanSlide (false);

				//set the platform's min and max movement to our own
				if (myOrientation == Orientation.Horizontal) {
					platformScript.setSlideMin (myCollider.bounds.min.x - (collider.transform.localScale.x / 2.0f));
					platformScript.setSlideMax (myCollider.bounds.min.x - (collider.transform.localScale.x / 2.0f));
				} else {
					platformScript.setSlideMin (myCollider.bounds.min.y - (collider.transform.localScale.y / 2.0f));
					platformScript.setSlideMax (myCollider.bounds.min.y - (collider.transform.localScale.y / 2.0f));
				}
			} 
		}
	}

	/// <summary>
	/// Finds the orientation of this slidebar.
	/// </summary>
	void findOrientation() {
		if (rotatingSquare != null) {
			if (rotatingSquare.transform.localEulerAngles.z == 180.0f || rotatingSquare.transform.localEulerAngles.z == 0.0f) {
				myOrientation = Orientation.Horizontal;
			} else {
				myOrientation = Orientation.Vertical;
			}
		}
	}

	/// <summary>
	/// Swaps the owner of the platform between two slidebars.
	/// </summary>
	/// <returns><c>true</c>, if owner of the platform was swaped, <c>false</c> otherwise.</returns>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	public bool swapOwnerOfPlatform(float x, float y) {
		float delta1, delta2;
			if (myOrientation == Orientation.Horizontal) {
				delta1 = x - platformSlot.transform.position.x;
				delta2 = x - connectedPlatformSlot.transform.position.x;
				if (delta1 * delta1 > delta2 * delta2) {
					//if (x > myCollider.bounds.max.x || x < myCollider.bounds.min.x) {
					connectedSlideBar.platform = platform;
					platform = null;
					return true;
				} else
					return false;
			} else {
				if (y > myCollider.bounds.max.y || y < myCollider.bounds.min.y) {
					connectedSlideBar.platform = this.platform;
					platform = null;
					return true;
				} else
					return false;
			}
		
	}

	/// <summary>
	/// Returns whether or not this slidebar is connected to another slidebar.
	/// </summary>
	/// <returns><c>true</c>, if connected was ised, <c>false</c> otherwise.</returns>
	public bool isConnected() {
		return connectedSlideBar != null;
	}
}
