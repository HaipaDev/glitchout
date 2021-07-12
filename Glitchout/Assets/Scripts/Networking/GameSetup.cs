using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GameSetup : MonoBehaviourPunCallbacks, IPunObservable{
    public static GameSetup instance;
    public PlayerSession[] players;
    int defPlayerCount=2;
    [SerializeReference] public GameStartConditions startCond=new GameStartConditions();
    void Start(){instance=this;Resize();}
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
        //if(SceneManager.GetActiveScene().name=="Game")
        GameSession.instance.players=players;
    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){// We own this player: send the others our data
            for(var i=0;i<players.Length;i++)stream.SendNext(players[i].nick);
            //for(var i=0;i<players.Length;i++)stream.SendNext(players[i].nick);
            //stream.SendNext(players);
            //stream.SendNext(startCond);
        }
        else{// Network player, receive data
            for(var i=0;i<this.players.Length;i++)this.players[i].nick=(string)stream.ReceiveNext();
            //this.players=(PlayerSession[])stream.ReceiveNext();
            //this.startCond=(GameStartConditions)stream.ReceiveNext();
        }
    }
}
