using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Char : MonoBehaviour {

    float lifeTime;
    float lifeLength;
    float lifeBase;
    const float INF = 999f;

    Color changeColor;
    float transparentTime;
    float transparentDegree;
    float transparentDegreeNo;

    private void Awake(){
        lifeLength = 6f;
        lifeBase = 6f;

        changeColor = GetComponent<SpriteRenderer>().color;
        transparentTime = 3f;
        transparentDegree = 0.5f;
        transparentDegreeNo = 1;
    }
    // Use this for initialization

    void Start () {
        lifeTime = lifeLength * Random.value + lifeBase;
	}
	
	// Update is called once per frame
	void Update () {

        if (lifeTime > 0){
            CheckTransparent();
            lifeTime -= Time.deltaTime;
        }else{
            Destroy(gameObject);
        }
	}

    void CheckTransparent(){
        if (lifeTime < transparentTime){
            changeColor.a = transparentDegree;;
        }
        else{
            changeColor.a = transparentDegreeNo;
        }
        GetComponent<SpriteRenderer>().color = changeColor;
    }

    public void NeverDisappear(){
        lifeTime = INF;
    }
}
