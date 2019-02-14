using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CreatWords : MonoBehaviour{

    const string PARTS_NAME_FILE = "PartsName.txt";
    const string TEXT_PATH = "Assets/Resources/Words/Text/";

    Vector3 scale;

    string[] partsList;
    HashSet<string> partsSet;
    Dictionary<string, int> partsIndex;
    Dictionary<string, int> pointsMap;
    Color[] partsColor;
    int partsNum;

    string[][] wordsLists;
    float coolDownBase;
    float coolDownLength;
    float coolDownTime;

    int leastWords;
    int mostWords;

    Monster monster;

    Vector3 youPosition;
    float leasteDistance;

    private void Awake(){
        LoadData();

        scale = new Vector3(0.5f, 0.5f, 0);

        coolDownBase = 2f;
        coolDownLength = 3f;

        leastWords = 1;
        mostWords = 3;

        monster = (Monster)GameObject.Find("Monster").GetComponent(typeof(Monster));

        youPosition = monster.GetYouPosition();
        leasteDistance = 1.6f;

    }

    // Use this for initialization
    void Start(){

        coolDownTime = 0;
    }

    // Update is called once per frame
    void Update(){

        if (coolDownTime <= 0){
            CreatMultipleWords();
            coolDownTime = Random.value * coolDownLength + coolDownBase;
        }
        else{
            coolDownTime -= Time.deltaTime;
        }
    }

    void CreatWord(){

        int partInex = Random.Range(0, partsNum);
        GameObject word = new GameObject{
            name = wordsLists[partInex][Random.Range(0, wordsLists[partInex].Length)],
            tag = partsList[partInex],
        };

        word.transform.SetParent(transform);

        word.transform.position = monster.RandomAwayPoint(youPosition, transform.position, leasteDistance);
        word.transform.localScale = scale;

        word.AddComponent<SpriteRenderer>();


        word.AddComponent<Rigidbody>();
        word.GetComponent<Rigidbody>().useGravity = false;
        word.GetComponent<Rigidbody>().isKinematic = true;

        word.AddComponent<Word>();
        

    }

    void CreatMultipleWords(){
        int num = Random.Range(leastWords, mostWords);
        while (num > 0){
            CreatWord();
            num--;
        }
    }

    void LoadData(){
        partsList = File.ReadAllLines(TEXT_PATH + PARTS_NAME_FILE);
        partsNum = partsList.Length;
        partsColor = new Color[]{Color.blue, Color.red, Color.green, Color.yellow};

        partsSet = new HashSet<string>();
        foreach(string partName in partsList){
            partsSet.Add(partName);
        }
        partsIndex = new Dictionary<string, int>();
        pointsMap = new Dictionary<string, int>();
        wordsLists = new string[partsNum][];
        for (int i = 0; i < partsNum; i++){
            partsIndex.Add(partsList[i], i);
            wordsLists[i] = File.ReadAllLines(TEXT_PATH + partsList[i] + ".txt");

            for (int j = 0; j < wordsLists[i].Length; j++){
                string[] word = wordsLists[i][j].Split(' ');
                wordsLists[i][j] = word[0];
                pointsMap.Add(word[0], int.Parse(word[1]));
            }
        }
    }

    public string[] GetPartsList(){
        return partsList;
    }

    public HashSet<string> GetPartsSet(){
        return partsSet;
    }

    public Dictionary<string, int> GetPartsIndex(){
        return partsIndex;
    }

    public Color[] GetPartsColor(){
        return partsColor;
    }

    public string[][] GetWordsLists(){
        return wordsLists;
    }
    public Dictionary<string, int> GetPointsLists(){
        return pointsMap;
    }
}
