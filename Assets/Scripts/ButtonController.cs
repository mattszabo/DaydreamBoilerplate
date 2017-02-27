using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerClickHandler {

    // this is the way to use the Unity event system via code
    // the same IPointerEvents can be scripted and then attached to any kind of UI component
    BoxesManager boxesManager;

    void Start() {
        boxesManager = transform.parent.GetComponent<BoxesManager>();
    }

    public void OnPointerClick(PointerEventData data) {
        boxesManager.ResetBoxes();
    }
}
