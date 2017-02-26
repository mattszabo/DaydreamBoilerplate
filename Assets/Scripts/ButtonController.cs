using UnityEngine;
using System.Collections.Generic;

public class ButtonController : MonoBehaviour {

	public void OnClick() {
		GameObject[] Boxes = transform.parent.transform.GetComponent<BoxesManager> ().GetBoxes ();
		List<Vector3> BoxPositions = transform.parent.transform.GetComponent<BoxesManager> ().GetBoxPositions ();
		for (int i = 0; i < Boxes.Length; i++) {
			Boxes[i].transform.position = BoxPositions[i];
		}
	}
}
