/*
 * Author: Brian Summers, summers.brian.cs@gmail.com
 * Date: 5/19/15
 * 
 * This script defines the behavior of the user clicking and dragging platforms.
 * A platform can slide if it has a slidebar parent and if the parent is connected
 * to another slidebar. When the platform can slide, the user clicks on the platform
 * and with the mouse 0 button held, can move the mouse to drag the platform. When the
 * user releases the mouse 0 button, the platform will "snap" to the end of the slidebar.
 * If a platform is horizontal, it will only move in the x direction when the user drags
 * it, and vice versa if the platform is vertical.
 */

using UnityEngine;
using System.Collections;

public class PlatformDrag : MonoBehaviour {

	private Vector3 screenPoint;
	private Vector3 offset;
	private float slideMax, slideMin;
	private enum Orientation{Vertical, Horizontal};
	private Orientation myOrientation;
	private bool canSlide;
	private bool dragging;
	private Transform toDrag;
	public GameObject rotatingSquare;
	public bool horizontal;

	void Start() {
		dragging = false;
		if (horizontal)
			myOrientation = Orientation.Horizontal;
		else
			myOrientation = Orientation.Vertical;
	}
	
	void Update(){
		Vector3 v3;

		//get the mouse down offset to the transform of the platform
		if (canSlide && Input.GetMouseButtonDown (0)) {
			//cast a ray into the scene from the mouse position, the ray will only collide with a sliding platform
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
			if (hit.collider != null) {
				toDrag = hit.collider.transform;
				screenPoint = Camera.main.WorldToScreenPoint (toDrag.position);
				v3 = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				offset = toDrag.position - v3;
				dragging = true;

			}
		}

		//while the mouse 0 button is held, the user can drag the platform
		if (Input.GetMouseButton(0)) {

			if (canSlide && dragging) {
			v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			v3 = Camera.main.ScreenToWorldPoint(v3);
			setOrientation ();
			
			//move in the x direction
			if (myOrientation == Orientation.Horizontal) {
				if (v3.x > slideMax) {
						v3.x = slideMax - toDrag.lossyScale.x / 2.0f;
				} else if (v3.x < slideMin){
						v3.x = slideMin - toDrag.lossyScale.x / 2.0f;
				} else {
					v3.x += offset.x;
				}
					toDrag.position = new Vector3(v3.x, toDrag.position.y, toDrag.position.z);
			} else {
				//move in the y direction
				if (v3.y > slideMax) {
						v3.y = slideMax + toDrag.lossyScale.y / 2.0f;
				} else if (v3.y < slideMin){
						v3.y = slideMin + toDrag.lossyScale.y / 2.0f;
				} else {
					v3.y += offset.y;
				}
					toDrag.position = new Vector3(toDrag.position.x, v3.y, toDrag.position.z);
				}
			}
		}

		//when the user releases the mouse 0 button, the platform will snap to an end position
		if (Input.GetMouseButtonUp (0)) {
			if (dragging) {
				setOrientation();
				dragging = false;
				float delta1, delta2;
				if (myOrientation == Orientation.Horizontal) {
					//get the distances to the end points and snap to the closest position
					delta1 = toDrag.position.x - slideMin;
					delta2 = toDrag.position.x - slideMax + (toDrag.lossyScale.x / 2.0f);
					if (delta1 * delta1 <= delta2 * delta2) {
						toDrag.position = new Vector3 (slideMin - toDrag.lossyScale.x / 2.0f, toDrag.position.y, toDrag.position.z);
					} else {
						toDrag.position = new Vector3 (slideMax - toDrag.lossyScale.x / 2.0f, toDrag.position.y, toDrag.position.z);
					}

					//transfer ownership of the platform between slidebars
					SlidebarConnector sb = toDrag.GetComponentInParent<SlidebarConnector>();
					if (sb.swapOwnerOfPlatform(toDrag.position.x, toDrag.position.y)) {
						toDrag.SetParent(sb.connectedSlideBar.transform, true);
						if (rotatingSquare != null)
							this.setRotatingSquare(sb.rotatingSquare);
					}

				} else {
					//same process but for y, get distances to endpoints and snap to the closest position
					delta1 = toDrag.position.y - slideMin;
					delta2 = toDrag.position.y - slideMax + (toDrag.lossyScale.y / 2.0f);
					if (delta1 * delta1 <= delta2 * delta2) {
						toDrag.position = new Vector3 (toDrag.position.x, slideMin + toDrag.lossyScale.y / 2.0f, toDrag.position.z);
					} else {
						toDrag.position = new Vector3 (toDrag.position.x, slideMax + toDrag.lossyScale.y / 2.0f, toDrag.position.z);
					}
					//transfer ownership of the platform between slidebars
					SlidebarConnector sb = toDrag.GetComponentInParent<SlidebarConnector>();
					if (sb.swapOwnerOfPlatform(toDrag.position.x, toDrag.position.y)) {
						toDrag.SetParent(sb.connectedSlideBar.transform, true);
						if (rotatingSquare != null)
							this.setRotatingSquare(sb.rotatingSquare);
					}
				}
			}
		}
	}

	/// <summary>
	/// Sets the orientation of the platform.
	/// </summary>
	void setOrientation() {
		if (rotatingSquare != null) {
			if (rotatingSquare.transform.localEulerAngles.z == 180.0f || rotatingSquare.transform.localEulerAngles.z == 0.0f) {
				myOrientation = Orientation.Horizontal;
			} else {
				myOrientation = Orientation.Vertical;
			}
		}
	}

	/// <summary>
	/// Sets the slide max of the platform.
	/// </summary>
	/// <param name="val">Value.</param>
	public void setSlideMax(float val) {
		slideMax = val;
	}

	/// <summary>
	/// Sets the slide minimum of the platform.
	/// </summary>
	/// <param name="val">Value.</param>
	public void setSlideMin(float val) {
		slideMin = val;
	}

	/// <summary>
	/// Sets whether this platform can slide.
	/// </summary>
	/// <param name="b">If set to <c>true</c> this platform can slide.</param>
	public void CanSlide(bool b) {
		canSlide = b;
	}

	/// <summary>
	/// Determines if this platform can slide.
	/// </summary>
	public void DetermineCanSlide() {
		SlidebarConnector sb = GetComponentInParent<SlidebarConnector> ();
		if (sb != null) {
			canSlide = sb.isConnected();
		}
	}

	/// <summary>
	/// Sets the rotating square this platform is in.
	/// </summary>
	/// <param name="rs">Rs.</param>
	public void setRotatingSquare(GameObject rs) {
		rotatingSquare = rs;
	}

	/// <summary>
	/// Raises the collision enter2 d event. The user cannot slide the platform
	/// when they are standing on it
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Player")
			canSlide = false;
	}

	/// <summary>
	/// Raises the collision exit2 d event. The user can slide the platform
	/// when they are not standing on it.
	/// </summary>
	/// <param name="collision">Collision.</param>
	void OnCollisionExit2D(Collision2D collision) {
		if (collision.transform.tag == "Player")
			canSlide = true;
	} 
	
}
