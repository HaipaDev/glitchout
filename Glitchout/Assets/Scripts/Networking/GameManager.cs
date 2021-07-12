using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks, IPunObservable{
    public static GameManager instance;
    [SerializeField] GameObject playerPrefab;
    public PlayerSession[] players;
    int defPlayerCount=2;
    [SerializeReference] public GameStartConditions startCond=new GameStartConditions();
    [Range(0f,10f)]public float gameSpeed=1;
    void Start(){instance=this;Resize();
        PhotonNetwork.SerializationRate=6000;//60/s
        if(GameSession.instance.offlineMode){PhotonNetwork.OfflineMode=true;PhotonNetwork.CreateRoom("");}
        if(PhotonNetwork.IsConnectedAndReady){var go=PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}//go.name="Player"+go.GetComponent<PlayerScript>().playerNum;}
        if(PhotonNetwork.OfflineMode){var go=PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}//Instantiate second player on OfflineMode}
    }
    void Update(){Resize();}
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
            if(p.playPerks.Count==0)foreach(var pk in allPerks)p.playPerks.Add(perks.empty);
            
            if(p.respawnTimer==0)p.respawnTimer=-4;
            if(p.playerScript!=null){
                p.playerScript.skinID=p.skinID;
                if(p.playerScript.GetComponent<PlayerPerks>()!=null)p.playerScript.GetComponent<PlayerPerks>().playPerks=p.playPerks;
            }
        }}
        for(var i=0;i<players.Length;i++){if(players[i]!=null){
            players[i].nick=PhotonNetwork.PlayerList[i].NickName;
            players[i].score=Mathf.Clamp(players[i].score,0,44444);
            if(players[i].respawnTimer>0)players[i].respawnTimer-=Time.deltaTime;
            if(players[i].respawnTimer<=0&&players[i].respawnTimer!=-4){players[i].playerScript.Respawn();players[i].respawnTimer=-4;}
        }}
        //if(SceneManager.GetActiveScene().name=="Game")
    }

    [PunRPC]
    void SkinPrevRPC(int ID){
        //for(var s=0;s<skinObj.Length;s++){
        var skinID=players[ID].skinID;
        for(var s2=0;s2<players.Length;s2++){if(s2!=ID){
            var skinID2=players[s2].skinID;
            if(skinID>0){
                if(skinID2!=skinID-1){skinID--;}else if(skinID2==skinID-1&&skinID>1){skinID-=2;}else{skinID=GameAssets.instance.skins.Length-1;}
            }else if(skinID==0){//Wrap skins outside and dont allow the same one
                skinID=GameAssets.instance.skins.Length-1;for(;skinID2==skinID&&skinID>0;skinID--);
            }
        }}
        players[ID].skinID=skinID;
    }//}
    [PunRPC]
    public void SkinNextRPC(int ID){//for(var s=0;s<skinObj.Length;s++){
        var skinID=players[ID].skinID;
        for(var s2=0;s2<players.Length;s2++){if(s2!=ID){
            var skinID2=players[s2].skinID;
            if(skinID<GameAssets.instance.skins.Length-1){
                if(skinID2!=skinID+1){skinID++;}else if(skinID2==skinID+1&&skinID<GameAssets.instance.skins.Length-2){skinID+=2;}else{skinID=0;}
            }else if(skinID==GameAssets.instance.skins.Length-1){//Wrap skins outside and dont allow the same one
                skinID=0;for(;skinID2==skinID;skinID++);
            }
        }}
        players[ID].skinID=skinID;
    }//}
    [PunRPC]
    void SetPerkRPC(perks enumPerk){
        //var player=GameSession.instance.players[editPerksID].playerScript;var playerPerks=player.GetComponent<PlayerPerks>().playPerks;
        var playerPerks=players[StartMenu.instance.editPerksID].playPerks;
        if(playerPerks.Contains(enumPerk)){var usedprkID=playerPerks.FindIndex(0,playerPerks.Count,(x)=>x==enumPerk);playerPerks[usedprkID]=perks.empty;return;}
        for(var i=0; i<playerPerks.Count;i++){
            if(playerPerks[i]==perks.empty){if(!playerPerks.Contains(enumPerk)){playerPerks[i]=enumPerk;}}
        }
    }

    
    public void Die(int playerNum, float hitTimer){
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

    public void ResetPlayers(){
        Array.Clear(players,0,players.Length);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){// We own this player: send the others our data
            stream.SendNext(gameSpeed);
            for(var i=0;i<players.Length;i++)stream.SendNext(players[i].nick);
            //for(var i=0;i<players.Length;i++)stream.SendNext(players[i].nick);
            //stream.SendNext(players);
            //stream.SendNext(startCond);
        }
        else{// Network player, receive data
            gameSpeed=(float)stream.ReceiveNext();
            for(var i=0;i<this.players.Length;i++)this.players[i].nick=(string)stream.ReceiveNext();
            //this.players=(PlayerSession[])stream.ReceiveNext();
            //this.startCond=(GameStartConditions)stream.ReceiveNext();
        }
    }
    public override void OnConnectedToMaster(){
        Debug.Log("OfflineMode: "+PhotonNetwork.OfflineMode);
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
}