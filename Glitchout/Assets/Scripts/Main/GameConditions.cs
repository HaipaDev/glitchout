using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameConditions : MonoBehaviour{
    public static GameConditions instance;
    public GameStartConditions startCond;
    public float timer;
    public bool matchFinished;
    public int winningPlayer=-1;
    public bool wonBySKLimit;
    public bool doubleScoreDisplay;
    void Start(){
        if(SceneManager.GetActiveScene().name!="OnlineMatchmaking")instance=this;
        //if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
        ChangeSettings();
    }
    public void ChangeSettings(){
        if(startCond.timerEnabled){timer=startCond.timerSet;}
    }

    void Update(){
        if(((startCond.timerEnabled&&startCond.timeKillsEnabled)&&startCond.scoreEnabled==true)
        ||((startCond.timerEnabled&&!startCond.timeKillsEnabled)&&startCond.killsEnabled==true)
        ||(startCond.scoreEnabled==true&&startCond.killsEnabled==true)){doubleScoreDisplay=true;}
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
        if(SceneManager.GetActiveScene().name=="Game"&&GameManager.GameIsStarted==true){
            if(startCond.timerEnabled==true){
                if(timer>0&&Time.timeScale>0.0001f){timer-=Time.deltaTime;}
                if(timer<=0){timer=-4;matchFinished=true;}
            }
            if(startCond.scoreEnabled==true&&matchFinished!=true){
                for(var i=0;i<GameManager.instance.players.Length;i++){
                    if(GameManager.instance.players[i].score>=startCond.scoreLimit){matchFinished=true;wonBySKLimit=true;winningPlayer=i+1;}
                }
            }if(startCond.killsEnabled==true&&matchFinished!=true){
                for(var i=0;i<GameManager.instance.players.Length;i++){
                    if(GameManager.instance.players[i].kills>=startCond.killsLimit){matchFinished=true;wonBySKLimit=true;winningPlayer=i+1;}
                }
            }

            if(matchFinished){
                SetWinningPlayer();
                EndMenu.instance.Open();
                //GameSession.instance.speedChanged=true;
                //GameSession.instance.gameSpeed=0;
            }
        }
        if(!GameManager.GameIsStarted){matchFinished=false;}
    }
    void SetWinningPlayer(){
        if(startCond.timerEnabled==true&&!wonBySKLimit){
            if(!startCond.timeKillsEnabled){
                if(GameManager.instance.players[0].score>GameManager.instance.players[1].score){winningPlayer=0;}
                else if(GameManager.instance.players[1].score>GameManager.instance.players[0].score){winningPlayer=1;}
                else{winningPlayer=-1;}
            }else{
                if(GameManager.instance.players[0].kills>GameManager.instance.players[1].kills){winningPlayer=0;}
                else if(GameManager.instance.players[1].kills>GameManager.instance.players[0].kills){winningPlayer=1;}
                else{winningPlayer=-1;}
            }
        }
        AudioManager.instance.Play("Victory");
    }
}
[System.Serializable]
public class GameStartConditions{
    public byte Id{get;set;}
    public static byte[] Serialize(object customType){
        var c=(GameStartConditions)customType;
        return new byte[]{c.Id};
    }
    public static object Deserialize(byte[] data){
        var result=new GameStartConditions();
        result.Id=data[0];
        return result;
    }
    public float timerSet=150;
    public bool timerEnabled=true;
    public bool timeKillsEnabled;
    public int scoreLimit;
    public bool scoreEnabled;
    public int killsLimit;
    public bool killsEnabled;
}