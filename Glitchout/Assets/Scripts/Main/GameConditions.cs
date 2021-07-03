using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameConditions : MonoBehaviour{
    public static GameConditions instance;
    public float timer;
    public float timerSet;
    public bool timerEnabled=true;
    public bool timeKillsEnabled;
    public int scoreLimit;
    public bool scoreEnabled;
    public int killsLimit;
    public bool killsEnabled;
    public bool matchFinished;
    public int winningPlayer=-1;
    public bool wonBySKLimit;
    public bool doubleScoreDisplay;
    void Start(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
        ChangeSettings();
    }
    public void ChangeSettings(){
        if(timerEnabled){timer=timerSet;}
    }

    void Update(){
        if(((timerEnabled&&timeKillsEnabled)&&scoreEnabled==true)||((timerEnabled&&!timeKillsEnabled)&&killsEnabled==true)||(scoreEnabled==true&&killsEnabled==true)){doubleScoreDisplay=true;}
        else{doubleScoreDisplay=false;}
        if((GameObject.Find("DefaultScore1")!=null&&GameObject.Find("DefaultScore2")!=null)&&(GameObject.Find("DoubleScore1")!=null&&GameObject.Find("DoubleScore2")!=null)){
            if(doubleScoreDisplay==true){
                GameObject.Find("DefaultScore1").transform.GetChild(0).gameObject.SetActive(false);GameObject.Find("DefaultScore2").transform.GetChild(0).gameObject.SetActive(false);
                GameObject.Find("DoubleScore1").transform.GetChild(0).gameObject.SetActive(true);GameObject.Find("DoubleScore2").transform.GetChild(0).gameObject.SetActive(true);
            }else{
                GameObject.Find("DefaultScore1").transform.GetChild(0).gameObject.SetActive(true);GameObject.Find("DefaultScore2").transform.GetChild(0).gameObject.SetActive(true);
                GameObject.Find("DoubleScore1").transform.GetChild(0).gameObject.SetActive(false);GameObject.Find("DoubleScore2").transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        if(SceneManager.GetActiveScene().name=="Game"&&StartMenu.GameIsStarted==true){
            if(timerEnabled==true){
                if(timer>0&&Time.timeScale>0.0001f){timer-=Time.deltaTime;}
                if(timer<=0){timer=-4;matchFinished=true;}
            }
            if(scoreEnabled==true&&matchFinished!=true){
                for(var i=0;i<GameSession.instance.players.Length;i++){
                    if(GameSession.instance.players[i].score>=scoreLimit){matchFinished=true;wonBySKLimit=true;winningPlayer=i+1;}
                }
            }if(killsEnabled==true&&matchFinished!=true){
                for(var i=0;i<GameSession.instance.players.Length;i++){
                    if(GameSession.instance.players[i].kills>=killsLimit){matchFinished=true;wonBySKLimit=true;winningPlayer=i+1;}
                }
            }

            if(matchFinished){
                SetWinningPlayer();
                EndMenu.instance.Open();
                //GameSession.instance.speedChanged=true;
                //GameSession.instance.gameSpeed=0;
            }
        }
        if(!StartMenu.GameIsStarted){matchFinished=false;}
    }
    void SetWinningPlayer(){
        if(timerEnabled==true&&!wonBySKLimit){
            if(!timeKillsEnabled){
                if(GameSession.instance.players[0].score>GameSession.instance.players[1].score){winningPlayer=0;}
                else if(GameSession.instance.players[1].score>GameSession.instance.players[0].score){winningPlayer=1;}
                else{winningPlayer=-1;}
            }else{
                if(GameSession.instance.players[0].kills>GameSession.instance.players[1].kills){winningPlayer=0;}
                else if(GameSession.instance.players[1].kills>GameSession.instance.players[0].kills){winningPlayer=1;}
                else{winningPlayer=-1;}
            }
        }
        AudioManager.instance.Play("Victory");
    }
}
