using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class StartMenu : MonoBehaviourPunCallbacks{
    public static StartMenu instance;
    public static bool GameIsStarted=false;
    public GameObject startMenuUI;
    public GameObject perksMenuUI;
    [HideInInspector]public float prevGameSpeed=1f;
    public int editPerksID;
    [HideInInspector]public float timerMin=2;
    [HideInInspector]public float timerSec=30;

    //Shop shop;
    void Start(){
        instance=this;
        if(startMenuUI==null){startMenuUI=transform.GetChild(0).gameObject;}
        if(perksMenuUI==null){startMenuUI=transform.GetChild(1).gameObject;}
        if(GameSession.instance.offlineMode)Open();
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        if(!GameIsStarted){
            //Sum up timer
            timerMin=Mathf.Clamp(timerMin,0,404);
            timerSec=Mathf.Clamp(timerSec,0,59);
            GameConditions.instance.timer=(timerMin*60)+timerSec;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!GameIsStarted){Leave();}
        }
    }
    
    [PunRPC]
    public void StartGame(){
        if(PhotonNetwork.IsMasterClient){
            startMenuUI.SetActive(false);
            perksMenuUI.SetActive(false);
            //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
            GameSession.instance.speedChanged=false;
            GameSession.instance.gameSpeed=prevGameSpeed;
            GameIsStarted=true;
            foreach(PlayerScript player in GameSession.instance.players){
                player.GetComponent<PlayerPerks>().SetStartParams();
                player.GetComponent<PlayerPerks>().RespawnPerks();
            }
        }
    }
    public void Open(){
        prevGameSpeed=GameSession.instance.gameSpeed;
        startMenuUI.SetActive(true);
        perksMenuUI.SetActive(false);
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameIsStarted=false;
        GameSession.instance.speedChanged=true;
        GameSession.instance.gameSpeed=0f;
    }
    public void Leave(){
        if(PhotonNetwork.IsConnectedAndReady){PhotonNetwork.LeaveRoom();PhotonNetwork.LeaveLobby();Level.instance.LoadOnlineScene();}
        if(PhotonNetwork.OfflineMode){PreviousGameSpeed();Level.instance.LoadStartMenu();}
    }

    public void PerksMenu(int number){
        editPerksID=number;
        startMenuUI.SetActive(false);
        perksMenuUI.SetActive(true);
    }
    public void BackStartMenu(){
        perksMenuUI.SetActive(false);
        startMenuUI.SetActive(true);
    }


    public void SetPerk(perks enumPerk){
        var player=GameSession.instance.players[editPerksID];
        var playerPerks=player.GetComponent<PlayerPerks>();
        if(playerPerks.playPerks.Contains(enumPerk)){var usedprkID=playerPerks.playPerks.FindIndex(0,playerPerks.playPerks.Count,(x) => x == enumPerk);playerPerks.playPerks[usedprkID]=perks.empty;return;}
        for(var i=0; i<playerPerks.playPerks.Count;i++){
            if(playerPerks.playPerks[i]==perks.empty){if(!playerPerks.playPerks.Contains(enumPerk)){playerPerks.playPerks[i]=enumPerk;}}
        }
    }
    public void SetTimeMinutes(TMPro.TMP_InputField txt){if(Application.isPlaying)timerMin=int.Parse(txt.text);if(int.Parse(txt.text)>404){txt.text="404";}}
    public void SetTimeSeconds(TMPro.TMP_InputField txt){if(Application.isPlaying)timerSec=int.Parse(txt.text);if(int.Parse(txt.text)>59){txt.text="59";}}
    /*public void SetGameTimeLimit(TMPro.TMP_InputField txt){
    if(Application.isPlaying){
        GameConditions.instance.scoreLimit=int.Parse(txt.text);
        txt.text=System.Math.Round((float.Parse(txt.text)),2).ToString();
        float min=(float)System.Math.Truncate(float.Parse(txt.text));
        float sec=float.Parse(txt.text)-min;
        GameConditions.instance.timer=(min*60)+(sec*100); //float.Parse(txt.text);
    }}*/
    public void SetScoreLimit(TMPro.TMP_InputField txt){
        if(Application.isPlaying)GameConditions.instance.scoreLimit=int.Parse(txt.text);
    }public void SetKillsLimit(TMPro.TMP_InputField txt){
        if(Application.isPlaying)GameConditions.instance.killsLimit=int.Parse(txt.text);
    }


    public void SetTimeLimitEnabled(bool isTimeLimit){
        if(Application.isPlaying)GameConditions.instance.timerEnabled=isTimeLimit;
    }public void SetTimeLimitKills(bool isTimeLimitKills){
    if(Application.isPlaying){
        if(GameConditions.instance.timerEnabled){
            GameConditions.instance.timeKillsEnabled=isTimeLimitKills;
            var ck=GameObject.Find("CheckmarkKS").GetComponent<TMPro.TextMeshProUGUI>();
            if(isTimeLimitKills)ck.text="K";else ck.text="S";
        }
    }}
    public void SetScoreLimitEnabled(bool isScoreLimit){
        if(Application.isPlaying)GameConditions.instance.scoreEnabled=isScoreLimit;
    }
    public void SetKillsLimitEnabled(bool isKillsLimit){
        if(Application.isPlaying)GameConditions.instance.killsEnabled=isKillsLimit;
    }

    public void PreviousGameSpeed(){GameSession.instance.gameSpeed=prevGameSpeed;}
}