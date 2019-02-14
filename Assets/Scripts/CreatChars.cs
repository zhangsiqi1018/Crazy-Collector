using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatChars : MonoBehaviour {

    string[] nameSet;
    string[] vowelSet;
    float vowelProb;
    string tagChar;

    float coolDownBase;
    float coolDownLength;
    float coolDownTime;

    int leastChars;
    int mostChars;

    Monster monster;
    GameObject monsterObject;

    Vector3 youPosition;
    float leasteDistance;

    Vector3 scale;

    Rect imageSize;
    Vector2 imagePivot;
    string imagePath;

    private void Awake(){
        nameSet = new string[] {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Trash"};
        vowelSet = new string[] { "A", "E", "I", "O", "U" };
        vowelProb = 0.3f;
        tagChar = "Char";

        coolDownBase = 1f;
        coolDownLength = 3f;

        leastChars = 2;
        mostChars = 5;

        monsterObject = GameObject.Find("Monster");
        monster = (Monster)monsterObject.GetComponent(typeof(Monster));

        youPosition = monster.GetYouPosition();
        leasteDistance = 1.6f;

        scale = new Vector3(0.08f, 0.08f, 0);

        imageSize = new Rect(0, 0, 800, 1000);
        imagePivot = new Vector2(0f, 0f);
        imagePath = "Characters/";
    }

    // Use this for initialization
    void Start () {

        coolDownTime = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (coolDownTime <= 0){
            CreatMultipleChars();
            coolDownTime = Random.value * coolDownLength + coolDownBase;
        }
        else{
            coolDownTime -= Time.deltaTime;
        }		
	}

    void CreatChar() {
        GameObject character = new GameObject{
            name =  Random.value > vowelProb? nameSet[Random.Range(0, nameSet.Length)] : vowelSet[Random.Range(0, vowelSet.Length)],
            tag = tagChar
        };

        character.transform.SetParent(transform);

        character.transform.position = monster.RandomAwayPoint(youPosition, transform.position, leasteDistance);
        character.transform.localScale = scale;

        character.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRendere = character.GetComponent<SpriteRenderer>();
        Texture2D texture2d = (Texture2D)Resources.Load(imagePath + character.name);
        spriteRendere.sprite = Sprite.Create(texture2d, imageSize, imagePivot);

        character.AddComponent<BoxCollider>();
        character.AddComponent<Char>();
   
    }

    void CreatMultipleChars(){
        int num = Random.Range(leastChars, mostChars);
        while (num > 0){
            CreatChar();
            num--;
        }
    }
}
