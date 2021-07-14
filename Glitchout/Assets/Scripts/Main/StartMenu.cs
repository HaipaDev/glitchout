using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class StartMenu : MonoBehaviourPunCallbacks{
    public static StartMenu instance;
    public GameObject mainPanel;
    public GameObject perksPanel;
    [SerializeField] GameObject[] skinObj;
    [SerializeField] TMPro.TextMeshProUGUI[] playersReadyTxt;
    public GameObject startButton;
    public TMPro.TextMeshProUGUI roomNameTxt;
    public int editPerksID;
    [HideInInspector]public float timerMin=2;
    [HideInInspector]public float timerSec=30;
    void Start(){
        instance=this;
        if(mainPanel==null){mainPanel=transform.GetChild(0).gameObject;}if(perksPanel==null){perksPanel=transform.GetChild(1).gameObject;}
        mainPanel.SetActive(false);perksPanel.SetActive(false);
        Open();
        if(!PhotonNetwork.OfflineMode){roomNameTxt.text=PhotonNetwork.CurrentRoom.Name;startButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="Ready";}
    }
    void Update(){
        if(!GameManager.instance.GameIsStarted){
            //Sum up timer
            timerMin=Mathf.Clamp(timerMin,0,404);
            timerSec=Mathf.Clamp(timerSec,0,59);
            GameConditions.instance.startCond.timerSet=(timerMin*60)+timerSec;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!GameManager.instance.GameIsStarted){Leave();}
        }

        //Set skins
        if(GameManager.instance.players.Length>1)if(GameManager.instance.players[0].skinID==0&&GameManager.instance.players[1].skinID==0){GameManager.instance.players[1].skinID=1;}
        for(var s=0;s<skinObj.Length;s++){
        //if(GameSession.instance.players.Length>=skinObj.Length){
        if(GameManager.instance.players[s]!=null){
            var skinID=GameManager.instance.players[s].skinID;
            //Check others for same skin
            for(var s2=0;s2<skinObj.Length;s2++){if(s2!=s){
                var skinID2=GameManager.instance.players[s2].skinID;
                if(skinID2==skinID){skinID++;}
            }}
            if(skinID>=0&&skinID<GameAssets.instance.skins.Length){
                skinObj[s].GetComponent<Image>().sprite=GameAssets.instance.GetSkin(skinID);
            }
        }}//}

        if(!PhotonNetwork.OfflineMode){
            for(var i=0;i<playersReadyTxt.Length;i++){
                if(GameManager.instance.players[i].ready){playersReadyTxt[i].text="Ready";playersReadyTxt[i].color=Color.green;}
                else{playersReadyTxt[i].text="Not Ready";playersReadyTxt[i].color=Color.grey;}
            }
            if(GameManager.instance.players[GameManager.instance.GetLocalPlayerID()].ready){
                startButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="Unready";
            }else{
                startButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="Ready";
            }
        }else{
            roomNameTxt.text="";
            startButton.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="Start";
        }
    }
    
    public void StartGame(){PhotonView.Get(GameManager.instance).RPC("StartGameRPC",RpcTarget.All,GameManager.instance.GetLocalPlayerID());}
    public void Open(){
        mainPanel.SetActive(true);
        perksPanel.SetActive(false);
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
    }
    public void Leave(){
        if(PhotonNetwork.IsConnectedAndReady){PhotonNetwork.LeaveRoom();PhotonNetwork.LeaveLobby();Level.instance.LoadOnlineScene();}
        if(PhotonNetwork.OfflineMode){PhotonNetwork.LeaveRoom();Level.instance.LoadStartMenu();}
    }

    public void PerksMenu(int number){
        editPerksID=number;
        mainPanel.SetActive(false);
        perksPanel.SetActive(true);
    }
    public void BackStartMenu(){
        perksPanel.SetActive(false);
        mainPanel.SetActive(true);
    }

    
    public void GiveMaster(int ID){if(PhotonNetwork.IsMasterClient)PhotonNetwork.SetMasterClient(PhotonNetwork.PlayerList[ID]);}
    public void SkinPrev(int ID){PhotonView.Get(GameManager.instance).RPC("SkinPrevRPC",RpcTarget.All,ID);}
    public void SkinNext(int ID){PhotonView.Get(GameManager.instance).RPC("SkinNextRPC",RpcTarget.All,ID);}
    public void SetPerk(perks enumPerk){PhotonView.Get(GameManager.instance).RPC("SetPerkRPC",RpcTarget.All,enumPerk);}

    public void SetTimeMinutes(TMPro.TMP_InputField txt){if(Application.isPlaying)timerMin=int.Parse(txt.text);if(int.Parse(txt.text)>404){txt.text="404";}}
    public void SetTimeSeconds(TMPro.TMP_InputField txt){if(Application.isPlaying)timerSec=int.Parse(txt.text);if(int.Parse(txt.text)>59){txt.text="59";}}
    public void SetScoreLimit(TMPro.TMP_InputField txt){
        if(Application.isPlaying)GameManager.instance.startCond.scoreLimit=int.Parse(txt.text);
    }public void SetKillsLimit(TMPro.TMP_InputField txt){
        if(Application.isPlaying)GameManager.instance.startCond.killsLimit=int.Parse(txt.text);
    }


    public void SetTimeLimitEnabled(bool isTimeLimit){
        if(Application.isPlaying)GameManager.instance.startCond.timerEnabled=isTimeLimit;
    }public void SetTimeLimitKills(bool isTimeLimitKills){
    if(Application.isPlaying){
        if(GameManager.instance.startCond.timerEnabled){
            GameManager.instance.startCond.timeKillsEnabled=isTimeLimitKills;
            var ck=GameObject.Find("CheckmarkKS").GetComponent<TMPro.TextMeshProUGUI>();
            if(isTimeLimitKills)ck.text="K";else ck.text="S";
        }
    }}
    public void SetScoreLimitEnabled(bool isScoreLimit){
        if(Application.isPlaying)GameManager.instance.startCond.scoreEnabled=isScoreLimit;
    }
    public void SetKillsLimitEnabled(bool isKillsLimit){
        if(Application.isPlaying)GameManager.instance.startCond.killsEnabled=isKillsLimit;
    }
}