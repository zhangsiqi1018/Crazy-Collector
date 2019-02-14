using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManger: MonoBehaviour{
    int playerPoints;

    int monsterPoints;
    int[] operators;

    

    Monster monster;
    You you;

    void Awake(){
        monster = GameObject.Find("Monster").GetComponent<Monster>();
        you = GameObject.Find("You").GetComponent<You>();
        monsterPoints = 0;
        playerPoints = 0;
        operators = new int[3];
    }

    void Update(){

    }
    public void ComparePoints(int playerPoints, int monsterPoints){
        if(monsterPoints < playerPoints){
            monster.SetLife(1);
        }else{
            you.ChangeHeart(-1);
        }
    }

    public int CalculateMonsterPoints(){
        return Random.Range(-10 ,10);
    }

    public int CalculatePlayerPoints(int[] pickedPoints){
        playerPoints = pickedPoints[0];
        for(int i = 1; i < pickedPoints.Length; i++){
            playerPoints += pickedPoints[i] * operators[i - 1];
        }
        return playerPoints;
    }

    public void InitializeOperators(){
        for(int i = 0; i < operators.Length; i++){
            operators[i] = -1 * (Random.Range(0, 10) > 2 ? -1 : 1);
        }
    }

    public int[] GetOperators(){
        return operators;
    }



}