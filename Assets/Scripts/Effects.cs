using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effects: MonoBehaviour{

    public Hook hook;
    public Monster monster;

    public void IncreaseSpeed(){
    }

    /*clean all the letters on the screen */
    public void CleanScreen(){
        GameObject[] chars = GameObject.FindGameObjectsWithTag("Char");
        foreach(GameObject c in chars){
            Destroy(c);
        }
    }
    /*slow down the monster's moving speed */
    public void SlowMonster(){


    }
    /*icrease letter's existence time */
    public void IncreaseExistenceTime(){

    }




}