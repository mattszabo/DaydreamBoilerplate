using UnityEngine;
using System.Collections;

public class ButtonController : MonoBehaviour {

	private GameObject box1;
	private GameObject box2;
	private GameObject box3;

	private Vector3 box1Pos;
	private Vector3 box2Pos;
	private Vector3 box3Pos;

	// Use this for initialization
	void Start () {
		box1 = GameObject.Find("Box1");
		box2 = GameObject.Find("Box2");
		box3 = GameObject.Find("Box3");

		box1Pos = box1.transform.position;
		box2Pos = box2.transform.position;
		box3Pos = box3.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() {
		Debug.Log("Button click");
		
		box1.transform.position = box1Pos;
		box2.transform.position = box2Pos;
		box3.transform.position = box3Pos;
	}
}
