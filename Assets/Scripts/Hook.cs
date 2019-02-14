using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour {

    // Use this for initialization

    You youInfo;
    Vector3 center;

    float w;
    float r;
    float launchR;
    int direction;

    float baseSpeed;

    float rRate;
    float rotateSpeed;

    float lRate;
    float launchSpeed;
    float outSpeed;
    float backSpeed;

    bool isShooting;
    bool isRetracting;
    Vector3 origin;
    Vector3 target;

    Button LButton;
    Button RButton;

    Button HButton;
    Button EButton;
    bool HReleased;
    bool EReleased;

    Vector3 energyScale;
    float energySpeed;

    Rect imageSize;
    Vector2 imagePivot;
    string imagePath;

    string tagEnergy;
    //string tagChar;

    Status status;
    HashSet<string> partsSet;

    private void Awake(){
        GameObject you = GameObject.Find("You");
        center = you.transform.position;
        youInfo = you.GetComponent<You>();

        w = 0;
        r = 0.6f;
        launchR = 10f;
        direction = +1;

        baseSpeed = 1.5f;

        rRate = 1.3f;
        rotateSpeed = rRate * baseSpeed;

        lRate = 4.2f;
        outSpeed = lRate * baseSpeed;
        backSpeed = 1.4f * outSpeed;


        isShooting = false;
        isRetracting = false;

        LButton = (Button)GameObject.Find("LeftButton").GetComponent(typeof(Button));
        RButton = (Button)GameObject.Find("RightButton").GetComponent(typeof(Button));

        HButton = (Button)GameObject.Find("HookButton").GetComponent(typeof(Button));
        EButton = (Button)GameObject.Find("EnergyButton").GetComponent(typeof(Button));
        HReleased = true;
        EReleased = true;

        energyScale = new Vector3(0.2f, 0.2f, 0);
        energySpeed = 4f;
        imageSize = new Rect(0, 0, 225, 224);
        imagePivot = new Vector2(0f, 0f);
        imagePath = "";

        tagEnergy = "Energy";
        //tagChar = "Char";

        status = GameObject.Find("Status").GetComponent<Status>();

    }

    void Start(){
        partsSet = GameObject.Find("CreatWords").GetComponent<CreatWords>().GetPartsSet();
    }

    // Update is called once per frame
    void Update(){
        CheckShooting();
        CheckDirection();
        CheckLaunch();
        CheckFire();
        MoveFunction();
    }

    private void OnTriggerEnter(Collider other){

        if (!isShooting){
            return;
        }

        isRetracting = true;
        launchSpeed = backSpeed;
        target = origin;

        if (partsSet.Contains(other.gameObject.tag)){
            if(transform.childCount == 0){
                other.gameObject.transform.SetParent(transform);
                other.gameObject.GetComponent<Word>().NeverDisappear();
            }
        }
    }

    void CheckLaunch(){
        if (Input.GetKeyDown(KeyCode.H) || (HButton.pressed && HReleased)){
            HReleased = false;
            if (!isShooting){
                isShooting = true;
                launchSpeed = outSpeed;
                origin = transform.position;
                target = new Vector3(Mathf.Cos(w) * launchR + center.x, Mathf.Sin(w) * launchR + center.y, 0);
            }
            else{
                isRetracting = true;
                target = origin;
            }
        }
        HReleased |= !HButton.pressed;
    }

    void CheckFire(){
        if ((Input.GetKeyDown(KeyCode.J) || (EButton.pressed && EReleased)) && youInfo.EnergyValue() > 0){
            EReleased = false;
            youInfo.ChangeEnergy(-1);

            Vector3 energyOrigin = isShooting ? origin : transform.position;
            CreatEnergy(energyOrigin);
        }
        EReleased |= !EButton.pressed;
    }

    void CreatEnergy(Vector3 setOrigin){
        GameObject energy = new GameObject{
            name = "Energy",
            tag = tagEnergy
        };

        energy.transform.position = setOrigin;
        energy.transform.localScale = energyScale;

        energy.AddComponent<SpriteRenderer>();
        SpriteRenderer spriteRendere = energy.GetComponent<SpriteRenderer>();
        Texture2D texture2d = (Texture2D)Resources.Load(imagePath + energy.name);
        spriteRendere.sprite = Sprite.Create(texture2d, imageSize, imagePivot);

        energy.AddComponent<SphereCollider>();
        energy.GetComponent<SphereCollider>().isTrigger = true;

        energy.AddComponent<Rigidbody>();
        energy.GetComponent<Rigidbody>().useGravity = false;
        energy.GetComponent<Rigidbody>().isKinematic = true;

        energy.AddComponent<Energy>();
        energy.GetComponent<Energy>().SetValues(new Vector3(Mathf.Cos(w) * launchR + center.x, Mathf.Sin(w) * launchR + center.y, 0), energySpeed);
    }

    void MoveFunction(){
        if (!isShooting){
            float detalW = direction * rotateSpeed * Time.deltaTime;
            w += detalW;
            float x = Mathf.Cos(w) * r + center.x;
            float y = Mathf.Sin(w) * r + center.y;
            transform.position = new Vector3(x, y, transform.position.z);
            transform.Rotate(0, 0, detalW / Mathf.PI * 180);
        }
        else{
            transform.position = Vector3.MoveTowards(transform.position, target, launchSpeed * Time.deltaTime);
        }
    }

    void CheckDirection(){

        int reverse = 1;
        if (transform.position.y <= center.y){
            reverse = -1;
        }   
        if (LButton.pressed || Input.GetKeyDown(KeyCode.A)){
            direction = reverse;
        }
        if (RButton.pressed || Input.GetKeyDown(KeyCode.D)){
            direction = -reverse;
        }
    }


    private void CheckShooting(){
        if (isRetracting && transform.position.Equals(origin)){
            isShooting = false;
            isRetracting = false;

            if (transform.childCount > 0){
                Transform child = transform.GetChild(0);
                status.AddWord(child.gameObject.tag, child.gameObject.name, child.gameObject.GetComponent<Word>().IsDummy());
                Destroy(child.gameObject);
            }
        }
    }
}
