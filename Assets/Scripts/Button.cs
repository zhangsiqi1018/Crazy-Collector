using UnityEngine;
using UnityEngine.EventSystems;

public class Button : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

    //[HideInInspector]
    public bool pressed;

    // Use this for initialization
    private void Awake(){
        pressed = false;
    }

    void Start(){
    }

    // Update is called once per frame
    void Update(){
    }

    public void OnPointerDown(PointerEventData eventData){
        pressed = true;
    }

    public void OnPointerUp(PointerEventData eventData){
        pressed = false;
    }
}