using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {

    int life;
    const int INITIAL_LIFE = 3;
    int point;

    float moveSpeed;
    float escapeSpeed;
    float speed;
    float attackProb;
    float safeDistance;
    float xLength;
    float yLength;
    float xOffset;
    float yOffset;

    Vector3 youPosition;
    Vector3 target;

    Vector3 escapePoint;
    bool isScared;

    string tagHook;
    //string tagChar;
    string tagYou;
    string tagEnergy;
    HashSet<string> partsSet;

    RectTransform bloodBar;

    private void Awake(){

        life = INITIAL_LIFE;

        point = 0;

        moveSpeed = 1.2f;
        escapeSpeed = 2 * moveSpeed;
        speed = moveSpeed;
        attackProb = 0.1f;
        safeDistance = 2f;

        xLength = 13;
        yLength = 7;
        xOffset = -6.5f;
        yOffset = -4f;

        youPosition = GameObject.Find("You").transform.position;
        target = transform.position;

        isScared = false;

        tagHook = "Hook";
        //tagChar = "Char";
        tagYou = "You";
        tagEnergy = "Energy";

        bloodBar = GameObject.Find("Blood").GetComponent<RectTransform>();

    }

    // Use this for initialization
    void Start () {
        partsSet = GameObject.Find("CreatWords").GetComponent<CreatWords>().GetPartsSet();
        GetComponent<SpriteRenderer>().color = Color.white;
        NextMove();
	}
	
	// Update is called once per frame
	void Update () {
        CheckEscapePoint();
        if (transform.position.Equals(target)){
            NextMove();
        }
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
		
	}

    private void OnTriggerEnter(Collider other){

        if (other.gameObject.CompareTag(tagHook)){
            SetEscape(escapeSpeed, Color.green);
        } else if(other.gameObject.CompareTag(tagEnergy)){
            life -= 1;
            SetEscape(escapeSpeed, Color.red);
        }
        else if (partsSet.Contains(other.gameObject.tag)){
            NextMove();
            Destroy(other.gameObject);
        }else if (other.gameObject.CompareTag(tagYou)){
            NextMove();
        }
    }

    void SetEscape(float setSpeed, Color setColor){
        isScared = true;
        speed = setSpeed;
        GetComponent<SpriteRenderer>().color = setColor;
        escapePoint = new Vector3(transform.position.x < 0 ? xOffset : xOffset + xLength, Random.value * yLength + yOffset, 0);
        target = escapePoint;
    }

    void NextMove (){
        if(isScared){
            return;
        }
        target = Random.value < attackProb ? youPosition : RandomAwayPoint(youPosition, transform.position, safeDistance);
    }

    void CheckEscapePoint(){
        if (isScared && transform.position.Equals(escapePoint)){
            isScared = false;
            speed = moveSpeed;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    Vector3 RandomPoint(){
        return new Vector3(Random.value * xLength + xOffset, Random.value * yLength + yOffset, 0);
    }

    public int LifeValue(){
        return life;
    }

    public void SetLife(int value){
        life -= value;
        bloodBar.localScale = new Vector3((float)life / INITIAL_LIFE, 1, 0);
        if(life == 0){
            Destroy(gameObject);
        }
    }

    public Vector3 RandomAwayPoint(Vector3 fixedPoint, Vector3 origniPoint, float distance){
        Vector3 nextPoint = RandomPoint();
        Vector3 middlePoint = (nextPoint + origniPoint) / 2;
        while (Vector3.Distance(fixedPoint, middlePoint) < distance){
            nextPoint = RandomPoint();
            middlePoint = (nextPoint + origniPoint) / 2;
        }
        return nextPoint;
    }

    public Vector3 GetYouPosition(){
        return youPosition;
    }

}
