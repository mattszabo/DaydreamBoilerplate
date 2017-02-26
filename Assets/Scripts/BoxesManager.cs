using UnityEngine;
using System.Collections.Generic;

public class BoxesManager : MonoBehaviour {

	private GameObject resetButton;
	public GameObject[] Boxes;
	private List<Vector3> BoxPositions = new List<Vector3>();

	// Use this for initialization
	void Start () {
		resetButton = transform.FindChild("ResetButton").gameObject;
		resetButton.SetActive (false);
		for (int i = 0; i < Boxes.Length; i++) {
			BoxPositions.Add (Boxes [i].transform.position);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (GvrController.AppButtonUp) {
			resetButton.SetActive (!resetButton.activeSelf);
		}
	}

	public List<Vector3> GetBoxPositions () {
		return BoxPositions;
	}

	public GameObject[] GetBoxes() {
		return Boxes;
	}
}
