using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class You : MonoBehaviour {

    int heart;
    int energy;

    const int INITIAL_HEART = 5;
    const int INITIAL_ENERGY = 90;

	// Use this for initialization
	void Start () {
        heart = INITIAL_HEART;
        energy = INITIAL_ENERGY;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public int HeartValue(){
        return heart;
    }

    public int EnergyValue(){
        return energy;
    }

    public void ChangeHeart(int count){
        heart = heart + count > 0 ? heart + count : 0;
    }

    public void ChangeEnergy(int count){
        energy = energy + count > 0 ? energy + count : 0;
    }
}
