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
		if (canSlide && Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
			if (hit.collider != null) {
				toDrag = hit.collider.transform;
				screenPoint = Camera.main.WorldToScreenPoint (toDrag.position);
				v3 = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
				offset = toDrag.position - v3;
				dragging = true;

			}
		}
		if (Input.GetMouseButton(0)) {

			if (canSlide && dragging) {
			v3 = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
			v3 = Camera.main.ScreenToWorldPoint(v3);
			setOrientation ();
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

		if (Input.GetMouseButtonUp (0)) {
			if (dragging) {
				setOrientation();
				dragging = false;
				float delta1, delta2;
				if (myOrientation == Orientation.Horizontal) {
					delta1 = toDrag.position.x - slideMin;
					delta2 = toDrag.position.x - slideMax + (toDrag.lossyScale.x / 2.0f);
					if (delta1 * delta1 <= delta2 * delta2) {
						toDrag.position = new Vector3 (slideMin - toDrag.lossyScale.x / 2.0f, toDrag.position.y, toDrag.position.z);
					} else {
						toDrag.position = new Vector3 (slideMax - toDrag.lossyScale.x / 2.0f, toDrag.position.y, toDrag.position.z);
					}
					SlidebarConnector sb = toDrag.GetComponentInParent<SlidebarConnector>();
					if (sb.swapOwnerOfPlatform(toDrag.position.x, toDrag.position.y)) {
						toDrag.SetParent(sb.connectedSlideBar.transform, true);
						if (rotatingSquare != null)
							this.setRotatingSquare(sb.rotatingSquare);
					}

				} else {
					delta1 = toDrag.position.y - slideMin;
					delta2 = toDrag.position.y - slideMax + (toDrag.lossyScale.y / 2.0f);
					if (delta1 * delta1 <= delta2 * delta2) {
						toDrag.position = new Vector3 (toDrag.position.x, slideMin + toDrag.lossyScale.y / 2.0f, toDrag.position.z);
					} else {
						toDrag.position = new Vector3 (toDrag.position.x, slideMax + toDrag.lossyScale.y / 2.0f, toDrag.position.z);
					}
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
	
	void setOrientation() {
		if (rotatingSquare != null) {
			if (rotatingSquare.transform.localEulerAngles.z == 180.0f || rotatingSquare.transform.localEulerAngles.z == 0.0f) {
				myOrientation = Orientation.Horizontal;
			} else {
				myOrientation = Orientation.Vertical;
			}
		}
	}

	public void setSlideMax(float val) {
		slideMax = val;
	}

	public void setSlideMin(float val) {
		slideMin = val;
	}

	public void CanSlide(bool b) {
		canSlide = b;
	}

	public void DetermineCanSlide() {
		SlidebarConnector sb = GetComponentInParent<SlidebarConnector> ();
		if (sb != null) {
			canSlide = sb.isConnected();
		}
	}

	public void setRotatingSquare(GameObject rs) {
		rotatingSquare = rs;
	}

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Player")
			canSlide = false;
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.transform.tag == "Player")
			canSlide = true;
	} 
	
}
