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
    private Vector3 originalPosition;

	private Vector3 objectVelocity;
	private Vector3 objectLastPosition;
	private bool isGravityEnabled;
	private Rigidbody rb;

	// Use this for initialization
	void Awake () {
		boxState = BoxStates.NONE;
		pointer = GameObject.Find("Laser");
        originalPosition = transform.position;
		rb = GetComponent<Rigidbody> ();
		isGravityEnabled = (rb) ? rb.useGravity : false;
	}
	
	// Update is called once per frame
	void Update () {
		switch (boxState){
			case BoxStates.NONE:
				break;
			case BoxStates.PICKED_UP:
				if (isGravityEnabled) {
					CalculateObjectVelocity ();
				}
				FollowPointer();
				break;
			case BoxStates.PUT_DOWN:
				if (isGravityEnabled) {
					ApplyMomentumToObject ();
				}
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

    public void ResetPosition()
    {
        transform.position = originalPosition;
    }

	private void CalculateObjectVelocity() {
		objectVelocity = (transform.position - objectLastPosition) / Time.fixedDeltaTime;
		objectLastPosition = transform.position;
	}

	private void ApplyMomentumToObject() {
		float velocityMultiplier = 10.0f;
		rb.AddForce (
			objectVelocity.x * velocityMultiplier,
			objectVelocity.y * velocityMultiplier,
			objectVelocity.z * velocityMultiplier,
			ForceMode.Force
		);
	}
}
