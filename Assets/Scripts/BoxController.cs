using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {

	enum BoxStates {
		NONE,
		PICKED_UP,
		PUT_DOWN
	};

	private BoxStates boxState;
	private GameObject pointer;

	// Use this for initialization
	void Start () {
		boxState = BoxStates.NONE;
		pointer = GameObject.Find("Laser");
	}
	
	// Update is called once per frame
	void Update () {
		switch (boxState){
			case BoxStates.NONE:
				break;
			case BoxStates.PICKED_UP:
				FollowPointer();
				break;
			case BoxStates.PUT_DOWN:
				boxState = BoxStates.NONE;
				break;
			default:
				break;
		}
	}

	public void PickUp() {
		boxState = BoxStates.PICKED_UP;
	}

	public void PutDown() {
		boxState = BoxStates.PUT_DOWN;
	}

	public void FollowPointer() {
		Ray ray = new Ray (pointer.transform.position, pointer.transform.forward);
		transform.position = ray.GetPoint (5.0f);
	}
}
