using UnityEngine;
using System.Collections.Generic;

public class BoxesManager : MonoBehaviour {

	private GameObject resetButton;
	public GameObject[] Boxes;

	// Use this for initialization
	void Start () {
		resetButton = transform.FindChild("ResetButton").gameObject;
		resetButton.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		if (GvrController.AppButtonUp) {
			resetButton.SetActive (!resetButton.activeSelf);
		}
	}

	public GameObject[] GetBoxes() {
		return Boxes;
	}

    public void ResetBoxes()
    {
        for (int i = 0; i < Boxes.Length; i++) 
            Boxes[i].GetComponent<BoxController>().ResetPosition();
    }
}
