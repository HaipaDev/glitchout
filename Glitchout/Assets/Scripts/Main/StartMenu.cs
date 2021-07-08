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
    [HideInInspector]public float prevGameSpeed=1f;

    void Start(){
        if((!GameSession.instance.offlineMode&&SceneManager.GetActiveScene().name=="Game")){StartGame();Destroy(transform.root.gameObject);}
        GameSession.instance.resize=true;
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
            GameConditions.instance.timer=(timerMin*60)+timerSec;
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(!GameIsStarted){Leave();}
        }

        //Set skins
        for(var s=0;s<skinObj.Length;s++){
        if(GameSession.instance.players.Length>skinObj.Length){
        if(GameSession.instance.players[s]!=null){
            var skinID=GameSession.instance.players[s].skinID;
            //Check others for same skin
            for(var s2=0;s2<skinObj.Length;s2++){if(s2!=s){
                var skinID2=GameSession.instance.players[s2].skinID;
                if(skinID2==skinID){
                    skinID++;
                    //Wrap skins outside and dont allow the same one
                    //if(skinID==GameAssets.instance.skins.Length-1){skinID=0;/*for(;skinID2==skinID;skinID++);*/}
                }
            }}
            if(skinID>=0&&skinID<GameAssets.instance.skins.Length){
                skinObj[s].GetComponent<Image>().sprite=GameAssets.instance.GetSkin(skinID);
            }
        }}}
    }
    
    [PunRPC]
    public void StartGame(){
        if(!PhotonNetwork.OfflineMode){if(FindObjectOfType<NetworkController>()!=null)FindObjectOfType<NetworkController>().StartGame();}
        if(PhotonNetwork.IsMasterClient){
            mainPanel.SetActive(false);
            perksPanel.SetActive(false);
            //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
            GameSession.instance.speedChanged=false;
            GameSession.instance.gameSpeed=prevGameSpeed;
            GameIsStarted=true;
            //Setting start params in NetworkController
        }
    }
    public void Open(){
        prevGameSpeed=GameSession.instance.gameSpeed;
        mainPanel.SetActive(true);
        perksPanel.SetActive(false);
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameSession.instance.speedChanged=true;
        GameSession.instance.gameSpeed=0f;
    }
    public void Leave(){
        if(PhotonNetwork.IsConnectedAndReady){PhotonNetwork.LeaveRoom();PhotonNetwork.LeaveLobby();Level.instance.LoadOnlineScene();}
        if(PhotonNetwork.OfflineMode){PreviousGameSpeed();Level.instance.LoadStartMenu();}
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

    [PunRPC]
    public void SetPerk(perks enumPerk){
        //var player=GameSession.instance.players[editPerksID].playerScript;var playerPerks=player.GetComponent<PlayerPerks>().playPerks;
        var playerPerks=GameSession.instance.players[editPerksID].playPerks;
        if(playerPerks.Contains(enumPerk)){var usedprkID=playerPerks.FindIndex(0,playerPerks.Count,(x) => x==enumPerk);playerPerks[usedprkID]=perks.empty;return;}
        for(var i=0; i<playerPerks.Count;i++){
            if(playerPerks[i]==perks.empty){if(!playerPerks.Contains(enumPerk)){playerPerks[i]=enumPerk;}}
        }
    }
    [PunRPC]
    public void SkinPrev(int ID){//for(var s=0;s<skinObj.Length;s++){
        var p=GameSession.instance.players;
        var skinID=p[ID].skinID;
        for(var s2=0;s2<GameSession.instance.players.Length;s2++){if(s2!=ID){
            var skinID2=p[s2].skinID;
            if(skinID>0){
                if(skinID2!=skinID-1){skinID--;}else if(skinID2==skinID-1&&skinID>1){skinID-=2;}else{skinID=GameAssets.instance.skins.Length-1;}
            }else if(skinID==0){//Wrap skins outside and dont allow the same one
                skinID=GameAssets.instance.skins.Length-1;for(;skinID2==skinID&&skinID>0;skinID--);
            }
        }}
        GameSession.instance.players[ID].skinID=skinID;
    }//}
    [PunRPC]
    public void SkinNext(int ID){//for(var s=0;s<skinObj.Length;s++){
        var p=GameSession.instance.players;
        var skinID=p[ID].skinID;
        for(var s2=0;s2<GameSession.instance.players.Length;s2++){if(s2!=ID){
            var skinID2=p[s2].skinID;
            if(skinID<GameAssets.instance.skins.Length-1){
                if(skinID2!=skinID+1){skinID++;}else if(skinID2==skinID+1&&skinID<GameAssets.instance.skins.Length-2){skinID+=2;}else{skinID=0;}
            }else if(skinID==GameAssets.instance.skins.Length-1){//Wrap skins outside and dont allow the same one
                skinID=0;for(;skinID2==skinID;skinID++);
            }
        }}
        GameSession.instance.players[ID].skinID=skinID;
    }//}

    public void SetTimeMinutes(TMPro.TMP_InputField txt){if(Application.isPlaying)timerMin=int.Parse(txt.text);if(int.Parse(txt.text)>404){txt.text="404";}}
    public void SetTimeSeconds(TMPro.TMP_InputField txt){if(Application.isPlaying)timerSec=int.Parse(txt.text);if(int.Parse(txt.text)>59){txt.text="59";}}
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