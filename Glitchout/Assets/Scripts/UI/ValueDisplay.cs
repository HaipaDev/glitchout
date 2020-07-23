using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    [SerializeField]string value;
    [SerializeField]bool update=true;
    string txt;
    public Player[] players;
    void Start(){
        Array.Resize(ref players,FindObjectsOfType<Player>().Length);
        if(value=="timerSetting"){var timer=GameConditions.instance.timer; float min=timer/60; float sec=Mathf.RoundToInt(timer-(float)(System.Math.Truncate(min)*60f)); if(sec>=60){min+=1;sec=0;} string textSec=sec.ToString(); if(sec<10){textSec="0"+sec;} txt=System.Math.Truncate(min).ToString()+"."+textSec;}
        if(txt!=""){
            if(GetComponent<TMPro.TextMeshProUGUI>()!=null)GetComponent<TMPro.TextMeshProUGUI>().text=txt;
            if(GetComponent<TMPro.TMP_InputField>()!=null)GetComponent<TMPro.TMP_InputField>().text=txt;
        }
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
        if(value.Contains("score_")){string[] x=value.Split('_');if(GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled==true){value="kills_"+x[1];}  int xx=int.Parse(x[1]);txt=GameSession.instance.score[xx].ToString();}//GameSession.instance.score[int.Parse(value.Split(["_"]))];}
        if(value.Contains("kills_")){string[] x=value.Split('_');if(GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled!=true){value="score_"+x[1];}  int xx=int.Parse(x[1]);txt=GameSession.instance.kills[xx].ToString();}
        if(value.Contains("scoreDesc")){if((GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled!=true)||GameConditions.instance.scoreEnabled){txt="Score:";}else if((GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled)||GameConditions.instance.killsEnabled){txt="Kills:";}}

        if(value=="timer"){GetComponent<TMPro.TextMeshProUGUI>().enabled=GameConditions.instance.timerEnabled;
            var timer=GameConditions.instance.timer;if(timer>0){float min=timer/60; float sec=Mathf.RoundToInt(timer-(float)(System.Math.Truncate(min)*60f)); if(sec>=60){min+=1;sec=0;} string textSec=sec.ToString(); if(sec<10){textSec="0"+sec;} txt=System.Math.Truncate(min).ToString()+":"+textSec;}else{txt="0:00";}}//160/60=2\.6 | 160-2*60=160-120=40
        
        if(update==true){
            if(GetComponent<TMPro.TextMeshProUGUI>()!=null)GetComponent<TMPro.TextMeshProUGUI>().text=txt;
            if(GetComponent<TMPro.TMP_InputField>()!=null)GetComponent<TMPro.TMP_InputField>().text=txt;
        }
    }
}
