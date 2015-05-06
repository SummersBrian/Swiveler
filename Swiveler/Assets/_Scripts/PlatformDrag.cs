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

	void Start() {
		dragging = false;
	}
	
	void Update(){
		Vector3 v3;
		if (canSlide && Input.GetMouseButtonDown (0)) {
			RaycastHit2D hit = Physics2D.Raycast (Camera.main.ScreenToWorldPoint (Input.mousePosition), Vector2.zero);
			
			if (hit.collider != null) {
				toDrag = transform;
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
					v3.x = slideMax - transform.lossyScale.x / 2.0f;
				} else if (v3.x < slideMin){
					v3.x = slideMin - transform.lossyScale.x / 2.0f;
				} else {
					v3.x += offset.x;
				}
				toDrag.position = new Vector3(v3.x, transform.position.y, transform.position.z);
				//transform.position = new Vector3(v3.x, lockedPosition);
			} else {
				if (v3.y > slideMax) {
					v3.y = slideMax - transform.lossyScale.y / 2.0f;
				} else if (v3.y < slideMin){
					v3.y = slideMin - transform.lossyScale.y / 2.0f;
				} else {
					v3.y += offset.y;
				}
				toDrag.position = new Vector3(toDrag.position.x, v3.y + offset.y, transform.position.z);
				}
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			if (dragging) {
				dragging = false;
				float delta1, delta2;
				if (myOrientation == Orientation.Horizontal) {
					delta1 = transform.position.x - slideMin;
					delta2 = transform.position.x - slideMax + (transform.lossyScale.x / 2.0f);
					if (delta1 * delta1 <= delta2 * delta2) {
						transform.position = new Vector3 (slideMin - transform.lossyScale.x / 2.0f, transform.position.y, transform.position.z);
					} else {
						transform.position = new Vector3 (slideMax - transform.lossyScale.x / 2.0f, transform.position.y, transform.position.z);
					}
					SlidebarConnector sb = transform.GetComponentInParent<SlidebarConnector>();
					if (sb.swapOwnerOfPlatform(transform.position.x, transform.position.y)) {
						transform.SetParent(sb.connectedSlideBar.transform, true);
					}

				} else {
					delta1 = transform.position.y - slideMin;
					delta2 = transform.position.y - slideMax;
					if (delta1 * delta1 <= delta2 * delta2) {
						transform.position = new Vector3 (transform.position.x, slideMin - transform.lossyScale.y / 2.0f, transform.position.z);
					} else {
						transform.position = new Vector3 (transform.position.x, slideMax - transform.lossyScale.y / 2.0f, transform.position.z);
					}
				}
			}
		}

		}
	
	void setOrientation() {
		if (gameObject.transform.eulerAngles.z == 180 || gameObject.transform.eulerAngles.z == 0) {
			myOrientation = Orientation.Horizontal;
		} else {
			myOrientation = Orientation.Vertical;
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

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.transform.tag == "Player")
			canSlide = false;
	}

	void OnCollisionExit2D(Collision2D collision) {
		if (collision.transform.tag == "Player")
			canSlide = true;
	}
	
}
