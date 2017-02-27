using UnityEngine;
using System.Collections;

public class BoxController : MonoBehaviour {

    public bool debuggingMode = false;

	enum BoxStates {
		NONE,
		PICKED_UP,
		PUT_DOWN
	};

	private BoxStates boxState;
	private GameObject pointer;

	// Use this for initialization
	void Awake () {
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
        Vector3 newPos = ray.GetPoint (5.0f);
        newPos.z = 0; 
        transform.position = newPos;

        if (debuggingMode)
            Debug.DrawRay(newPos, pointer.transform.forward, Color.red, 10.0f);
    }
}
