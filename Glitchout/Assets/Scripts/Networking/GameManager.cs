using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable{
    public static GameManager instance;
    [SerializeField] GameObject playerPrefab;
    public PlayerSession[] players;
    int defPlayerCount=2;
    [SerializeReference] public GameStartConditions startCond=new GameStartConditions();
    public int timerMin=2;
    public int timerSec=30;

    public float startTimer=-4;
    public bool GameIsStarted=false;
    public bool GameIsPaused=false;
    public bool TimeIs0=false;
    [Range(0f,10f)]public float gameSpeed=1;
    void Start(){instance=this;Resize();
        PhotonNetwork.SerializationRate=6000;//60/s
        if(GameSession.instance.offlineMode){PhotonNetwork.LeaveLobby();PhotonNetwork.OfflineMode=true;PhotonNetwork.CreateRoom("");}
        if(PhotonNetwork.IsConnectedAndReady){var go=PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}//go.GetPhotonView().ViewID=1000+players.Length;}
        if(PhotonNetwork.OfflineMode){var go=PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}//Second player on OfflineMode
        //else{if(FindObjectOfType<NetworkController>()==null)Instantiate(new GameObject("NetworkController",typeof(NetworkController)),new Vector2(0,0),Quaternion.identity);}
    }
    void Update(){
        if(Time.timeScale<0.0001f||!GameIsStarted||GameIsPaused||GameConditions.instance.MatchFinished){TimeIs0=true;}else{TimeIs0=false;}
        Resize();
        StartGame();
        if(!PhotonNetwork.OfflineMode){
            var pausedCount=Array.FindAll(players,x=>x.paused).Length;
            if(pausedCount!=players.Length){GameIsPaused=false;}else{GameIsPaused=true;}
        }else{
            if(PauseMenu.GameIsPaused)GameIsPaused=true;else GameIsPaused=false;
        }
        
        if(!GameIsStarted){
            //Sum up timer
            timerMin=Mathf.Clamp(timerMin,0,404);
            timerSec=Mathf.Clamp(timerSec,0,59);
            startCond.timerSet=(timerMin*60)+timerSec;
            GameConditions.instance.startCond=startCond;
        }
    }
    void OnValidate() {
        var allPerks=Enum.GetValues(typeof(perks));
        foreach(PlayerSession p in players){if(p.playPerks.Count==0)foreach(var pk in allPerks)if((perks)pk!=perks.empty)p.playPerks.Add(perks.empty);}//Resize playPerks
    }
    void Resize(){
        if(players==null){players=new PlayerSession[defPlayerCount];}
        if(players.Length==0){Array.Resize(ref players,defPlayerCount);for(var pi=0;pi<defPlayerCount;pi++){players[pi]=new PlayerSession();}}
        PlayerScript[] allPlayers=FindObjectsOfType<PlayerScript>();
        if(allPlayers.Length>0)if(players.Length!=allPlayers.Length)Array.Resize(ref players,allPlayers.Length);
        for(var ap=0;ap<allPlayers.Length;ap++){if(allPlayers[ap]!=null){
            if(players[ap]!=null){players[ap].playerScript=Array.Find(allPlayers,x=>x.playerNum==ap);}else{players[ap]=new PlayerSession();}}
        else{Debug.Log("No PlayerScript found for ID "+ap);}}
        var allPerks=Enum.GetValues(typeof(perks));
        foreach(PlayerSession p in players){if(p!=null){
            if(p.playPerks==null){p.playPerks=new List<perks>();}//Resize playPerks
            if(p.playPerks.Count==0){foreach(var pk in allPerks){p.playPerks.Add(perks.empty);}p.playPerks.Remove(p.playPerks[0]);}
            
            if(p.respawnTimer==0)p.respawnTimer=-4;
            if(p.playerScript!=null){
                p.playerScript.skinID=p.skinID;
                if(p.playerScript.GetComponent<PlayerPerks>()!=null)p.playerScript.GetComponent<PlayerPerks>().playPerks=p.playPerks;
            }
        }}
        for(var i=0;i<players.Length;i++){if(players[i]!=null){
            if(!PhotonNetwork.OfflineMode&&PhotonNetwork.PlayerList.Length>i)players[i].nick=PhotonNetwork.PlayerList[i].NickName;
            players[i].score=Mathf.Clamp(players[i].score,0,44444);
            if(players[i].respawnTimer>0)players[i].respawnTimer-=Time.deltaTime;
            if(players[i].respawnTimer<=0&&players[i].respawnTimer!=-4){players[i].playerScript.Respawn();players[i].respawnTimer=-4;}
        }}
        //if(SceneManager.GetActiveScene().name=="Game")
    }
    void StartGame(){
        if(startTimer<=0&&startTimer!=-4&&!GameIsStarted&&players.Length>1){
            PhotonNetwork.CurrentRoom.IsOpen=false;
            StartMenu.instance.Close();
            //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
            foreach(PlayerSession player in players){
                if(player.playerScript!=null){
                    player.playerScript.GetComponent<PlayerPerks>().SetStartParams();
                    player.playerScript.GetComponent<PlayerPerks>().RespawnPerks();
                }else{Debug.LogWarning("No PlayerScript attached to "+System.Array.FindIndex(players,0,players.Length,x=>x==player));}
            }
            GameConditions.instance.startCond=startCond;
            GameIsStarted=true;
        }
        if(startTimer>0)startTimer-=Time.unscaledDeltaTime;
        if(!PhotonNetwork.OfflineMode){
            var readyCount=Array.FindAll(players,x=>x.ready).Length;
            if(readyCount!=players.Length){return;}else{if(startTimer==-4){startTimer=3;}}
        }
    }

    #region RPCs
    [PunRPC]
    void SetPaused(int ID, bool set){if(players.Length>ID)if(players[ID]!=null)players[ID].paused=set;}
    [PunRPC]
    void StartGameRPC(int ID){
        if(!PhotonNetwork.OfflineMode){
            if(players.Length>1){
                if(!players[ID].ready){players[ID].ready=true;}
                else{players[ID].ready=false;startTimer=-4;}
            }
        }else{
            startTimer=0;
        }
    }

    [PunRPC]
    void SkinPrevRPC(int ID){
        //for(var s=0;s<skinObj.Length;s++){
        var skinID=players[ID].skinID;
        //if(players.Length>1){
            for(var s2=0;s2<players.Length;s2++){if(players.Length==1||s2!=ID){
                var skinID2=players[s2].skinID;
                if(skinID>0){
                    if(skinID2!=skinID-1){skinID--;}else if(skinID2==skinID-1&&skinID>1){skinID-=2;}else{skinID=GameAssets.instance.skins.Length-1;}
                }else if(skinID==0){//Wrap skins outside and dont allow the same one
                    skinID=GameAssets.instance.skins.Length-1;for(;skinID2==skinID&&skinID>0;skinID--);
                }
            }}
        //}else{skinID--;}
        players[ID].skinID=skinID;
    }//}
    [PunRPC]
    public void SkinNextRPC(int ID){//for(var s=0;s<skinObj.Length;s++){
        var skinID=players[ID].skinID;
        //if(players.Length>1){
            for(var s2=0;s2<players.Length;s2++){if(players.Length==1||s2!=ID){
                var skinID2=players[s2].skinID;
                if(skinID<GameAssets.instance.skins.Length-1){
                    if(skinID2!=skinID+1){skinID++;}else if(skinID2==skinID+1&&skinID<GameAssets.instance.skins.Length-2){skinID+=2;}else{skinID=0;}
                }else if(skinID==GameAssets.instance.skins.Length-1){//Wrap skins outside and dont allow the same one
                    skinID=0;for(;skinID2==skinID;skinID++);
                }
            }}
        //}else{skinID++;}
        players[ID].skinID=skinID;
    }//}
    [PunRPC]
    void SetPerkRPC(perks enumPerk,int editPerksID){
        //var player=GameSession.instance.players[editPerksID].playerScript;var playerPerks=player.GetComponent<PlayerPerks>().playPerks;
        var playerPerks=players[editPerksID].playPerks;
        if(playerPerks.Contains(enumPerk)){var usedprkID=playerPerks.FindIndex(0,playerPerks.Count,x=>x==enumPerk);playerPerks[usedprkID]=perks.empty;return;}
        else{
            for(var i=0;i<playerPerks.Count;i++){
                if(playerPerks[i]==perks.empty){playerPerks[i]=enumPerk;return;}
            }
        }
    }
    [PunRPC]void SetTimeMinutesRPC(int value){timerMin=value;}
    [PunRPC]void SetTimeSecondsRPC(int value){timerSec=value;}
    [PunRPC]void SetScoreLimitRPC(int value){startCond.scoreLimit=value;}
    [PunRPC]void SetKillLimitRPC(int value){startCond.killsLimit=value;}
    [PunRPC]void SetTimeLimitEnabledRPC(bool value){startCond.timerEnabled=value;}
    [PunRPC]void SetTimeLimitKillsRPC(bool value){startCond.timeKillsEnabled=value;}
    [PunRPC]void SetScoreLimitEnabledRPC(bool value){startCond.scoreEnabled=value;}
    [PunRPC]void SetKillsLimitEnabledRPC(bool value){startCond.killsEnabled=value;}

    [PunRPC]
    public void RestartGame(){
        GameConditions.instance.MatchFinished=false;
        GameConditions.instance.wonBySKLimit=false;
        startTimer=-4;
        GameIsStarted=false;
        PauseMenu.instance.Resume();
        EndMenu.instance.Close();
        StartMenu.instance.Open();
        foreach(PlayerSession p in players){p.ready=false;
            p.score=0;p.kills=0;p.respawnTimer=-4;
            p.playerScript.xpos=0;p.playerScript.ypos=0;p.playerScript.health=p.playerScript.maxHealth;
        }
        PhotonNetwork.CurrentRoom.IsOpen=true;
    }
    #endregion
    
    public void Die(int playerNum, float hitTimer){
        PhotonView.Get(this).RPC("DieRPC",RpcTarget.All,playerNum,hitTimer);
    }
    [PunRPC]
    void DieRPC(int playerNum, float hitTimer){
        if(playerNum==0){
            AddSubScore(playerNum,GameSession.instance.score_death,false);
            players[playerNum].respawnTimer=GameSession.instance.respawnTime;
            if(hitTimer>0){AddSubScore(playerNum+1,GameSession.instance.score_kill);players[playerNum+1].kills++;}
        }else if(playerNum==1){
            AddSubScore(playerNum,GameSession.instance.score_death,false);
            players[playerNum].respawnTimer=GameSession.instance.respawnTime;
            if(hitTimer>0){AddSubScore(playerNum-1,GameSession.instance.score_kill);players[playerNum-1].kills++;}
        }
    }

    public void AddSubScore(int i,int scoreValue,bool add=true){
        if(add)players[i].score+=scoreValue;//Mathf.RoundToInt(scoreValue*scoreMulti);
        else players[i].score-=scoreValue;//Mathf.RoundToInt(scoreValue*scoreMulti);
    }

    public void MultiplyScore(int i,float multipl){
        int result=Mathf.RoundToInt(players[i].score*multipl);
        players[i].score=result;
    }

    public void ResetPlayers(){Array.Clear(players,0,players.Length);}


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){// We own this player: send the others our data
            for(var i=0;i<players.Length;i++){
                if(!GameIsStarted){
                stream.SendNext(players[i].nick);
                stream.SendNext(players[i].skinID);
                stream.SendNext(players[i].ready);
                for(var p=0;p<this.players[i].playPerks.Count;p++){
                    stream.SendNext((int)players[i].playPerks[p]);
                }
                }
                stream.SendNext(players[i].score);
                stream.SendNext(players[i].kills);
                stream.SendNext(players[i].paused);
            }
            if(!GameIsStarted){
                stream.SendNext(timerMin);
                stream.SendNext(timerSec);
                stream.SendNext(startCond.timerEnabled);
                stream.SendNext(startCond.scoreEnabled);
                stream.SendNext(startCond.scoreLimit);
                stream.SendNext(startCond.killsEnabled);
                stream.SendNext(startCond.killsLimit);
            }
            stream.SendNext(GameIsPaused);
            stream.SendNext(gameSpeed);
            //for(var i=0;i<players.Length;i++)stream.SendNext(players[i].nick);
            //stream.SendNext(players);
            //stream.SendNext(startCond);
        }
        else{// Network player, receive data
            for(var i=0;i<this.players.Length;i++){
                if(!GameIsStarted){
                this.players[i].nick=(string)stream.ReceiveNext();
                this.players[i].skinID=(int)stream.ReceiveNext();
                this.players[i].ready=(bool)stream.ReceiveNext();
                for(var p=0;p<this.players[i].playPerks.Count;p++){
                    this.players[i].playPerks[p]=(perks)((int)stream.ReceiveNext());
                }
                }
                this.players[i].score=(int)stream.ReceiveNext();
                this.players[i].kills=(int)stream.ReceiveNext();
                this.players[i].paused=(bool)stream.ReceiveNext();
            }
            if(!GameIsStarted){
                this.timerMin=(int)stream.ReceiveNext();
                this.timerSec=(int)stream.ReceiveNext();
                this.startCond.timerEnabled=(bool)stream.ReceiveNext();
                this.startCond.scoreEnabled=(bool)stream.ReceiveNext();
                this.startCond.scoreLimit=(int)stream.ReceiveNext();
                this.startCond.killsEnabled=(bool)stream.ReceiveNext();
                this.startCond.killsLimit=(int)stream.ReceiveNext();
            }
            this.GameIsPaused=(bool)stream.ReceiveNext();
            gameSpeed=(float)stream.ReceiveNext();
            //this.players=(PlayerSession[])stream.ReceiveNext();
            //this.startCond=(GameStartConditions)stream.ReceiveNext();
        }
    }
    public override void OnJoinedRoom(){

    }
    public override void OnPlayerEnteredRoom(Player newPlayer){Debug.Log(newPlayer.NickName+" just joined "+PhotonNetwork.CurrentRoom.Name);}
    public override void OnPlayerLeftRoom(Player newPlayer){Debug.Log(newPlayer.NickName+" just left "+PhotonNetwork.CurrentRoom.Name);if(PhotonNetwork.PlayerList.Length==1){GameConditions.instance.MatchFinished=true;}}//PhotonView.Get(this).RPC("RestartGame",RpcTarget.All);}}
    public int GetLocalPlayerID(){
        return Array.FindIndex(players,0,players.Length,x=>x.nick==PhotonNetwork.LocalPlayer.NickName);
        //Array.FindIndex(players,0,players.Length,x=>Array.IndexOf(players,x)==Array.FindIndex(PhotonNetwork.PlayerList,x=>x==PhotonNetwork.LocalPlayer));
    }
}


[System.Serializable]
public class PlayerSession{
    public PlayerScript playerScript;
    public string nick;
    public int skinID=0;
    public List<perks> playPerks=new List<perks>();
    public int score=0;
    public int kills=0;
    public float respawnTimer=-4;
    public bool ready;
    public bool paused;
}