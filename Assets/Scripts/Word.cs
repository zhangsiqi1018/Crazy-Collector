using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Word : MonoBehaviour{

    const string IMAGE_PATH = "Words/Image/";
    Vector2 imagePivot;

    string[] partsList;
    HashSet<string> partsSet;
    Dictionary<string, int> partsIndex;
    string[][] wordsLists;
    Dictionary<string,int> pointsLists;
    Color[] partsColor;
    int partsNum;

    float lifeTime;
    float initialLifeTime;
    float lifeLength;
    float lifeBase;
    int points;
    const float INF = 999f;

    bool beCaught;

    Color changeColor;
    float transparentTime;
    float transparentDegree;
    float transparentDegreeNo;

    Vector3 target;
    Vector3 center;
    float speed;
    float leastDistance;

    float dummyRate;
    bool isDummy;

    const float CHANGE_TIME = 3f;
    float changTime;

    Monster monster;

    string tagHook;

    GameObject timeChild;
    Vector3 timeChangeScale;

    GameObject lifeChild;
    Vector3 lifeBarPosition;
    float lifeBarLen;

    private void Awake(){
        imagePivot = new Vector2(0f, 0f);

        lifeLength = 8f;
        lifeBase = 6f;

        transparentTime = 3f;
        transparentDegree = 0.5f;
        transparentDegreeNo = 1;

        target = transform.position;
        speed = 0.55f;
        leastDistance = 2.5f;

        dummyRate = 0.2f;
        isDummy = Random.value < dummyRate ? true : false;

        monster = GameObject.Find("Monster").GetComponent<Monster>();
        center = monster.GetYouPosition();

        tagHook = "Hook";

        timeChangeScale = new Vector3(0.4f, 0.4f, 0);

        lifeBarPosition = new Vector3(0, 2.3f, 0);
        lifeBarLen = 1.5f;

    }
    // Use this for initialization

    void Start(){
        changTime = CHANGE_TIME;
        lifeTime = lifeLength * Random.value + lifeBase;
        initialLifeTime = lifeTime;
        beCaught = false;
        LoadData();
    }

    // Update is called once per frame
    void Update(){

        CheckLife();
        if (gameObject.transform.parent.gameObject.name == tagHook){
            return;
        }
        CheckChange();
        CheckTarget();
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);
    }

    void ChangeObject(){
        int partInex = Random.Range(0, partsNum);
        gameObject.name = wordsLists[partInex][Random.Range(0, wordsLists[partInex].Length)];
        gameObject.tag = partsList[partInex];
        SpriteRenderer spriteRendere = GetComponent<SpriteRenderer>();

        Texture2D texture2d;
        if (!isDummy){
            texture2d = (Texture2D)Resources.Load(IMAGE_PATH + gameObject.tag + "/" + gameObject.name);
            //spriteRendere.color = partsColor[partsIndex[gameObject.tag]];
        }
        else{
            texture2d = (Texture2D)Resources.Load(IMAGE_PATH + "dummy");
        }
        spriteRendere.sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), imagePivot);

        changeColor = GetComponent<SpriteRenderer>().color;

        //Destroy(GetComponent<BoxCollider>());
    }

    void CheckTransparent(){
        if (lifeTime < transparentTime){
            changeColor.a = transparentDegree; ;
        }
        else{
            changeColor.a = transparentDegreeNo;
        }
        GetComponent<SpriteRenderer>().color = changeColor;
    }

    void SetTarget(){
        target = monster.RandomAwayPoint(center, transform.position, leastDistance);
    }

    void CheckTarget(){
        if(target.Equals(transform.position)){
            SetTarget();
        }
    }

    void CheckLife(){
        if(beCaught){
            return;
        }
        if (lifeTime > 0){
            SetTimeBarLen();
            CheckTransparent();
            lifeTime -= Time.deltaTime;
        }
        else{
            Destroy(gameObject);
        }
    }

    public void NeverDisappear(){
        beCaught = true;
    }

    public bool IsDummy(){
        return isDummy;
    }


    private void OnTriggerEnter(Collider other){
        SetTarget();
    }

    void LoadData(){
        CreatWords partsInfo = GameObject.Find("CreatWords").GetComponent<CreatWords>();
        partsList = partsInfo.GetPartsList();
        partsSet = partsInfo.GetPartsSet();
        partsIndex = partsInfo.GetPartsIndex();
        partsColor = partsInfo.GetPartsColor();
        partsNum = partsList.Length;

        wordsLists = partsInfo.GetWordsLists();
        pointsLists = partsInfo.GetPointsLists();

        ChangeObject();
     
        AddChangeTime();
        AddTimeBar();

        gameObject.AddComponent<BoxCollider>();
        gameObject.GetComponent<BoxCollider>().isTrigger = true;
    }

    void AddChangeTime(){
        timeChild = new GameObject { name = "ChangeTime" };
        timeChild.transform.SetParent(transform);
        timeChild.transform.localScale = timeChangeScale;
        timeChild.transform.localPosition = new Vector3(0, 0, 0);
        timeChild.AddComponent<MeshRenderer>();
        var timeText = timeChild.AddComponent<TextMesh>();
        timeText.text = ((int)changTime).ToString();
        timeText.color = Color.black;
    }

    void AddTimeBar(){
        lifeChild = new GameObject { name = "LifeTime" };
        lifeChild.transform.SetParent(transform);
        SetTimeBarLen();
        lifeChild.transform.localPosition = lifeBarPosition;
        lifeChild.AddComponent<SpriteRenderer>();
        Texture2D texture2d;
        texture2d = (Texture2D)Resources.Load("Images/timebar");
        lifeChild.GetComponent<SpriteRenderer>().sprite = Sprite.Create(texture2d, new Rect(0, 0, texture2d.width, texture2d.height), imagePivot);
        lifeChild.GetComponent<SpriteRenderer>().color = Color.green;
    }

    void SetTimeBarLen(){
        lifeChild.transform.localScale = new Vector3(lifeBarLen * lifeTime / initialLifeTime, 1f, 0);
        if (lifeTime < transparentTime){
            lifeChild.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    void CheckChange(){
        var timeText = timeChild.GetComponent<TextMesh>();
        timeText.text = ((int)changTime).ToString();
        if (changTime > 0){
            changTime -= Time.deltaTime;
        }else{
            ChangeObject();
            changTime = CHANGE_TIME;
        }
    }
}
