using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour {

    Vector3 target;
    float speed;

    string tagBoundary;
    string tagHook;
    string tagChar;
    string tagYou;

	// Use this for initialization
	void Start () {
        tagBoundary = "Boundary";
        tagHook = "Hook";
        tagChar = "Char";
        tagYou = "You";
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag(tagHook) || other.gameObject.CompareTag(tagYou)){
            return;
        }

        if(other.gameObject.CompareTag(tagChar)){
            Destroy(other.gameObject);
        }
        Destroy(gameObject);

    }

    public void SetValues(Vector3 setTarge, float setSpeed){
        target = setTarge;
        speed = setSpeed;
    }
}
