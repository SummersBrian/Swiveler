using UnityEngine;
using System.Collections;

public class SlidebarConnector : MonoBehaviour {

	public GameObject platform;
	public GameObject platformSlot;
	public GameObject rotatingSquare;
	
	public enum Orientation{Vertical,Horizontal};
	private Orientation myOrientation;
	private BoxCollider2D myCollider;
	private float max;
	private float min;
	public GameObject connectedPlatformSlot;
	public SlidebarConnector connectedSlideBar;

	void Start() {
		//setOrientation ();
		myCollider = GetComponent<BoxCollider2D> ();
	}

	void OnTriggerEnter2D(Collider2D collider) {
		if (collider.transform.tag != "RotatingPiecesContainer") {
			findOrientation ();
			if (platform != null) {
				if (collider.transform.tag == "SlideBar") {
				PlatformDrag platformScript = platform.GetComponent<PlatformDrag> ();
				connectedSlideBar = collider.GetComponent<SlidebarConnector> ();
					if (connectedSlideBar.platform != null) {
						return;
					}
				connectedPlatformSlot = connectedSlideBar.platformSlot;
				connectedSlideBar.connectedSlideBar = GetComponent<SlidebarConnector> ();
				connectedSlideBar.connectedPlatformSlot = platformSlot;
					platformScript.CanSlide (true);
					//setOrientation ();
					if (myOrientation == Orientation.Horizontal) {
						if (myCollider.bounds.min.x < collider.bounds.min.x) {
							//min = myCollider.bounds.min.x;
							//max = collider.bounds.max.x;
							min = platformSlot.transform.position.x;
							max = connectedPlatformSlot.transform.position.x;
						} else {
							//min = collider.bounds.min.x;
							//max = myCollider.bounds.max.x;
							min = connectedPlatformSlot.transform.position.x;
							max = platformSlot.transform.position.x;
						}
					} else {
						if (myCollider.bounds.min.y < collider.bounds.min.y) {
							min = platformSlot.transform.position.y;
							max = connectedPlatformSlot.transform.position.y;
						} else {
							min = connectedPlatformSlot.transform.position.y;
							max = platformSlot.transform.position.y;
						}
					}

					platformScript.setSlideMax (max);
					platformScript.setSlideMin (min);
				} 
			}
		}
	}

	void OnTriggerExit2D(Collider2D collider) {
		if (collider.transform.tag != "RotatingPiecesContainer") {
			findOrientation ();
			if (collider.gameObject.CompareTag ("SlideBar")) {
				connectedSlideBar = null;
				if (myOrientation == Orientation.Horizontal) {
					min = myCollider.bounds.min.x;
					max = myCollider.bounds.max.x;
				} else {
					min = myCollider.bounds.min.y;
					max = myCollider.bounds.max.y;
				}
		
				if (platform != null) {
					PlatformDrag platformScript = platform.GetComponent<PlatformDrag> ();
					platformScript.CanSlide (false);
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
	}

	void findOrientation() {
		if (rotatingSquare.transform.localEulerAngles.z == 180.0f || rotatingSquare.transform.localEulerAngles.z == 0.0f) {
			myOrientation = Orientation.Horizontal;
		} else {
			myOrientation = Orientation.Vertical;
		}
	}

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
			}
			else return false;
		} else {
			if (y > myCollider.bounds.max.y || y < myCollider.bounds.min.y) {
				connectedSlideBar.platform = this.platform;
				platform = null;
				return true;
			} else return false;
		}
	}

	public bool isConnected() {
		return connectedSlideBar != null;
	}
}
