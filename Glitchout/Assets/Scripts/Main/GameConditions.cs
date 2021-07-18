using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameConditions : MonoBehaviour, IPunObservable{
    public static GameConditions instance;
    public GameStartConditions startCond;
    public float timer;
    public bool MatchFinished;
    public int winningPlayer=-1;
    public bool wonBySKLimit;
    public bool doubleScoreDisplay;
    void Start(){
        instance=this;
        //if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
        StartSettings();
    }
    public void StartSettings(){if(startCond.timerEnabled){timer=startCond.timerSet;}}

    void Update(){
        if(!GameManager.instance.GameIsStarted){StartSettings();}
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
                if(startCond.killsEnabled||startCond.timeKillsEnabled){GameObject.Find("DefaultScore1").transform.GetChild(0).GetChild(1).GetComponent<ValueDisplay>().value="kills_0";GameObject.Find("DefaultScore2").transform.GetChild(0).GetChild(1).GetComponent<ValueDisplay>().value="kills_1";}
                else{GameObject.Find("DefaultScore1").transform.GetChild(0).GetChild(1).GetComponent<ValueDisplay>().value="score_0";GameObject.Find("DefaultScore2").transform.GetChild(0).GetChild(1).GetComponent<ValueDisplay>().value="score_1";}
                GameObject.Find("DoubleScore1").transform.GetChild(0).gameObject.SetActive(false);GameObject.Find("DoubleScore2").transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        if(SceneManager.GetActiveScene().name=="Game"&&GameManager.instance.GameIsStarted==true){
            if(startCond.timerEnabled==true){
                if(timer>0&&!GameManager.instance.TimeIs0){timer-=Time.deltaTime;}
                if(timer<=0){timer=-4;MatchFinished=true;}
                if(timer<=10&&!AudioManager.instance.GetSource("ClockTick").isPlaying){AudioManager.instance.Play("ClockTick");}
            }
            if(startCond.scoreEnabled==true&&MatchFinished!=true){
                for(var i=0;i<GameManager.instance.players.Length;i++){
                    if(GameManager.instance.players[i].score>=startCond.scoreLimit){MatchFinished=true;wonBySKLimit=true;winningPlayer=i;}
                }
            }if(startCond.killsEnabled==true&&MatchFinished!=true){
                for(var i=0;i<GameManager.instance.players.Length;i++){
                    if(GameManager.instance.players[i].kills>=startCond.killsLimit){MatchFinished=true;wonBySKLimit=true;winningPlayer=i;}
                }
            }

            if(MatchFinished){
                AudioManager.instance.Stop("ClockTick");
                SetWinningPlayer();
                EndMenu.instance.Open();
                //GameSession.instance.speedChanged=true;
                //GameSession.instance.gameSpeed=0;
            }
        }
        if(!GameManager.instance.GameIsStarted){MatchFinished=false;}
    }
    void SetWinningPlayer(){
        if(startCond.timerEnabled==true&&!wonBySKLimit&&GameManager.instance.players.Length>1){
            if(!startCond.timeKillsEnabled){
                if(GameManager.instance.players[0].score>GameManager.instance.players[1].score){winningPlayer=0;}
                else if(GameManager.instance.players[1].score>GameManager.instance.players[0].score){winningPlayer=1;}
                else{winningPlayer=-1;}
            }else{
                if(GameManager.instance.players[0].kills>GameManager.instance.players[1].kills){winningPlayer=0;}
                else if(GameManager.instance.players[1].kills>GameManager.instance.players[0].kills){winningPlayer=1;}
                else{winningPlayer=-1;}
            }
        }else if(GameManager.instance.players.Length==1){winningPlayer=0;}
        
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){// We own this player: send the others our data
            stream.SendNext(timer);
            stream.SendNext(winningPlayer);
            stream.SendNext(MatchFinished);
        }
        else{// Network player, receive data
            this.timer=(float)stream.ReceiveNext();
            this.winningPlayer=(int)stream.ReceiveNext();
            this.MatchFinished=(bool)stream.ReceiveNext();
        }
    }
}
[System.Serializable]
public class GameStartConditions{
    public float timerSet=150;
    public bool timerEnabled=true;
    public bool timeKillsEnabled;
    public int scoreLimit;
    public bool scoreEnabled;
    public int killsLimit;
    public bool killsEnabled;
}