using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPack : MonoBehaviour{
    public float timerMax=5f;
    public float timer;
    void Start(){
        timer=timerMax;
    }

    void Update(){
        if(timer>0){
            timer-=Time.deltaTime;
            GetComponentInChildren<Image>().fillAmount=1/timer;
        }else GetComponentInChildren<Image>().fillAmount=1;
    }
}
