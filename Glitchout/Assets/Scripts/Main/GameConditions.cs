using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConditions : MonoBehaviour{
    public static GameConditions instance;
    public float timer;
    public float timerSet;
    public bool timerEnabled=true;
    public bool matchFinished;
    void Start(){
        instance=this;
        ChangeSettings();
    }
    public void ChangeSettings(){
        if(timerEnabled){timer=timerSet;}
    }

    void Update(){
        if(SceneManager.GetActiveScene().name=="Game"){
            if(timerEnabled){
                if(timer>0&&Time.timeScale>0.0001f){timer-=Time.deltaTime;}
                if(timer<=0){timer=-4;matchFinished=true;}
            }

            if(matchFinished){
                GameSession.instance.speedChanged=true;
                GameSession.instance.gameSpeed=0;
            }
        }
    }
    public void SetTimeLimit (bool isTimeLimit){
        timerEnabled = isTimeLimit;
    }
}
