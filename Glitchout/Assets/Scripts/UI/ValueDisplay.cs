using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    [SerializeField]string value;
    string txt;
    public Player[] players;
    void Start(){
        Array.Resize(ref players,FindObjectsOfType<Player>().Length);
    }

    void Update(){
        Player[] allPlayers=FindObjectsOfType<Player>();
        foreach(Player player in allPlayers){
            //if(player.playerNum==playerNum.One){player1=player;}
            //if(player.playerNum==playerNum.Two){player2=player;}
            //Array.Resize(ref players,allPlayers.Length);
            players[player.playerNum]=player;
        }
        if(value.Contains("health_")){string[] x=value.Split('_');int xx=int.Parse(x[1]); if(players[xx].hidden!=true){txt=Mathf.RoundToInt(players[xx].health).ToString();}else{txt=Math.Round(GameSession.instance.respawnTimer[xx],1).ToString();}}
        if(value.Contains("score_")){string[] x=value.Split('_');int xx=int.Parse(x[1]);txt=GameSession.instance.score[xx].ToString();}//GameSession.instance.score[int.Parse(value.Split(["_"]))];}

        GetComponent<TMPro.TextMeshProUGUI>().text=txt;
    }
}
