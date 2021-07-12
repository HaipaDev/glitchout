using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class StartMenu : MonoBehaviourPunCallbacks{
    public static StartMenu instance;
    public static bool GameIsStarted=false;
    public GameObject mainPanel;
    public GameObject perksPanel;
    [SerializeField] GameObject[] skinObj;
    public int editPerksID;
    [HideInInspector]public float timerMin=2;
    [HideInInspector]public float timerSec=30;
    void Start(){
        //if((!GameSession.instance.offlineMode&&SceneManager.GetActiveScene().name=="Game")){StartGame();Destroy(transform.root.gameObject);}
        instance=this;
        //if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
        if(mainPanel==null){mainPanel=transform.GetChild(0).gameObject;}if(perksPanel==null){perksPanel=transform.GetChild(1).gameObject;}
        mainPanel.SetActive(false);perksPanel.SetActive(false);
        Open();
    }
    void Update(){
        if(!GameIsStarted){
            //Sum up timer
            timerMin=Mathf.Clamp(timerMin,0,404);
            timerSec=Mathf.Clamp(timerSec,0,59);
            GameConditions.instance.startCond.timerSet=(timerMin*60)+timerSec;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!GameIsStarted){Leave();}
        }

        //Set skins
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
    }
    
    [PunRPC]
    public void StartGame(){
        if(!PhotonNetwork.OfflineMode){if(FindObjectOfType<NetworkController>()!=null)FindObjectOfType<NetworkController>().StartGame();}
        if(PhotonNetwork.IsMasterClient){
            mainPanel.SetActive(false);
            perksPanel.SetActive(false);
            //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
            GameManager.instance.gameSpeed=1;
            GameIsStarted=true;
            //Setting start params in NetworkController
        }
    }
    public void Open(){
        mainPanel.SetActive(true);
        perksPanel.SetActive(false);
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameManager.instance.gameSpeed=0f;
    }
    public void Leave(){
        if(PhotonNetwork.IsConnectedAndReady){PhotonNetwork.LeaveRoom();PhotonNetwork.LeaveLobby();Level.instance.LoadOnlineScene();}
        if(PhotonNetwork.OfflineMode){GameManager.instance.gameSpeed=1;Level.instance.LoadStartMenu();}
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