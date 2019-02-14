using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Status : MonoBehaviour {

    const float TRANSPARENT_DEGREE = 0.3f;

    string[] partsList;
    HashSet<string> partsSet;
    Dictionary<string, int> partsIndex;
    Dictionary<string, int> pointsMap;
    int partsNum;

    //Text collectedChars;
    Text[] partsText;
    Text[] pointsText;
    Text[] operators;
    Text comparator;
    Color[] partsColor;
    Color[] partsTransparentColor;
    bool[] partsCollected;
    int collectedPartsNum;
    string[] collectedParts;


    int collectedNumber;

    Text effectNote;
    float noteTime;
    const float NOTE_TIME = 1.5f;

    Text sentenceCounter;

    Text timeCounter;
    int second;
    int minute;
    float realTime;

    Text heartNumber;

    Button enterButton;
    Button escButton;
    bool escReleased;

    bool enableEnter;
    bool enableEsc;

    const float RESET_TIME = 2f;
    float resetTime;

    You you;
    public ScoreManger scoreManger;
    private void Awake(){

        //collectedChars = GameObject.Find("CollectedChars").GetComponent<Text>();
        collectedNumber = 0;
        collectedPartsNum = 0;

        effectNote = GameObject.Find("EffectNote").GetComponent<Text>();
        noteTime = 0;

        sentenceCounter = GameObject.Find("SentenceCounter").GetComponent<Text>();

        timeCounter = GameObject.Find("TimeCounter").GetComponent<Text>();
        second = 0;
        minute = 0;
        realTime = 0;

        heartNumber = GameObject.Find("HeartNumber").GetComponent<Text>();
        enterButton = GameObject.Find("EnterButton").GetComponent<Button>();
        escButton = GameObject.Find("EscButton").GetComponent<Button>();
        escReleased = true;

        enableEnter = false;
        enableEsc = false;

        you = GameObject.Find("You").GetComponent<You>();
        scoreManger = GameObject.Find("ScoreManger").GetComponent<ScoreManger>();

        resetTime = 0;
        //Find operators
        operators = new Text[3];
        operators[0] = GameObject.Find("FirstOperator").GetComponent<Text>();
        operators[1] = GameObject.Find("SecondOperator").GetComponent<Text>();
        operators[2] = GameObject.Find("ThirdOperator").GetComponent<Text>();

        comparator = GameObject.Find("Comparator").GetComponent<Text>();
        
    }
    // Use this for initialization
    void Start () {
        LoadData();
        effectNote.text = "";
        sentenceCounter.text = collectedNumber.ToString();
        timeCounter.text = "00:00";
        scoreManger.InitializeOperators();
        for(int i = 0; i < operators.Length; i++){
            operators[i].text = scoreManger.GetOperators()[i] == 1 ? "+" : "-";
        }
        
        HeartEnergyUpdate();
    }
	
	// Update is called once per frame
	void Update () {
        TimeUpdate();
        HeartEnergyUpdate();
        CheckEnter();
        CheckEsc();
        //CheckEffectNote();
	}

    void TimeUpdate(){
        realTime += Time.deltaTime;
        second = (int)realTime % 60;
        minute = (int)realTime / 60;
        timeCounter.text = minute.ToString().PadLeft(2, '0') + ":" + second.ToString().PadLeft(2, '0');
    }

    void CheckEffectNote(){
        if (noteTime > 0){
            noteTime -= Time.deltaTime;
        }else{
            effectNote.text = "";
        }
    }

    void SetEffectNote(string note, Color color){
        effectNote.color = color;
        effectNote.text = note;
        noteTime = NOTE_TIME;
    }

    public void AddWord(string tag, string name, bool isdummy){
        if (!partsCollected[partsIndex[tag]]){
            collectedPartsNum += 1;
        }
        int index = partsIndex[tag];
        partsCollected[index] = true;

        collectedParts[index] = name;
        partsText[index].color = partsColor[index];
        partsText[index].text = isdummy ? "?" : name;
        pointsText[index].text = isdummy ? "?" : pointsMap[name] + ""; 
        CheckEnableEnter();
    }

    void CheckEnableEnter(){
        enableEnter = (collectedPartsNum == partsNum);

    }

    void CheckEnter(){
        if ((enterButton.pressed || Input.GetKeyDown(KeyCode.W)) && enableEnter){
            int[] points = new int[4];
            for (int i = 0; i < partsNum; i++){
                partsText[i].text = collectedParts[i];
                partsText[i].color = partsColor[i];
                pointsText[i].text = pointsMap[collectedParts[i]] + "";
                points[i] = pointsMap[collectedParts[i]];
            }
            int playerPoints = scoreManger.CalculatePlayerPoints(points);
            int monsterPoints = scoreManger.CalculateMonsterPoints();
            pointsText[4].text = playerPoints + "";
            pointsText[5].text = monsterPoints + "";
            comparator.text = playerPoints == monsterPoints ? "==" : playerPoints > monsterPoints ? ">" : "<";
            scoreManger.ComparePoints(playerPoints, monsterPoints);    
            collectedNumber += playerPoints;
            sentenceCounter.text = collectedNumber.ToString();
            collectedPartsNum = 0;
            enableEnter = false;
            enableEsc = true;
            resetTime = RESET_TIME;
        }
    }

    void CheckEsc(){
        if (enableEsc){
            if(resetTime <= 0){
                for (int i = 0; i < partsNum; i++){
                    partsText[i].text = "?";
                    partsText[i].color = partsTransparentColor[i];
                    partsCollected[i] = false;
                }
                for(int i = 0; i < pointsText.Length; i++){
                    pointsText[i].text = "";
                }
                comparator.text = "";
                scoreManger.InitializeOperators();
                for (int i = 0; i < operators.Length; i++){
                    operators[i].text = scoreManger.GetOperators()[i] == 1 ? "+" : "-";
                }
                enableEsc = false;
            }else{
                resetTime -= Time.deltaTime;
            }

        }
        escReleased = !escButton.pressed;
    }

    void LoadData(){
        CreatWords partsInfo = GameObject.Find("CreatWords").GetComponent<CreatWords>();
        partsList = partsInfo.GetPartsList();
        partsSet = partsInfo.GetPartsSet();
        partsIndex = partsInfo.GetPartsIndex();
        partsNum = partsList.Length;

        pointsMap = partsInfo.GetPointsLists();
        partsText = new Text[partsNum];
        pointsText = new Text[partsNum + 2];

        partsColor = partsInfo.GetPartsColor();
        partsTransparentColor = new Color[partsNum];
        for (int i = 0; i < partsNum; i++){
            partsText[i] = GameObject.Find(partsList[i] + "Part").GetComponent<Text>();
            pointsText[i] = GameObject.Find(partsList[i] + "Points").GetComponent<Text>();

            Color tempColor = partsColor[i];
            tempColor.a = TRANSPARENT_DEGREE;
            partsTransparentColor[i] = tempColor;
            partsText[i].color = partsTransparentColor[i];
        }
        pointsText[4] = GameObject.Find("YourPoints").GetComponent<Text>();
        pointsText[5] = GameObject.Find("MonsterPoints").GetComponent<Text>();

        partsCollected = new bool[partsNum];
        collectedPartsNum = 0;
        collectedParts = new string[partsNum];
    }

  

    public void HeartEnergyUpdate(){
        heartNumber.text = "× " + you.HeartValue().ToString().PadLeft(2, '0');
    }
}
