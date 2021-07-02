using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    [SerializeField]string value;
    [SerializeField]bool update=true;
    string txt;
    public PlayerScript[] players;
    void Start(){
        Array.Resize(ref players,FindObjectsOfType<PlayerScript>().Length);
        //if(value=="timerSetting"){var timer=GameConditions.instance.timer; float min=timer/60; float sec=Mathf.RoundToInt(timer-(float)(System.Math.Truncate(min)*60f)); if(sec>=60){min+=1;sec=0;} string textSec=sec.ToString(); if(sec<10){textSec="0"+sec;} txt=System.Math.Truncate(min).ToString()+"."+textSec;}
        if(value=="timerMin"){txt=StartMenu.instance.timerMin.ToString();}
        if(value=="timerSec"){txt=StartMenu.instance.timerMin.ToString();}
        //if(value=="perkCount_"){txt="0/"+PerksList.instance.perkList.Length+" PERKS";}
        if(txt!=""){
            if(GetComponent<TMPro.TextMeshProUGUI>()!=null)GetComponent<TMPro.TextMeshProUGUI>().text=txt;
            if(GetComponent<TMPro.TMP_InputField>()!=null)GetComponent<TMPro.TMP_InputField>().text=txt;
        }
    }

    void Update(){
        if(value.Contains("gameVersion")){txt=SaveSerial.instance.settingsData.gameVersion;}
        PlayerScript[] allPlayers=FindObjectsOfType<PlayerScript>();
        foreach(PlayerScript player in allPlayers){
            //if(PlayerScript.playerNum==playerNum.One){player1=PlayerScript;}
            //if(PlayerScript.playerNum==playerNum.Two){player2=PlayerScript;}
            //Array.Resize(ref players,allPlayers.Length);
            players[player.playerNum]=player;
        }
        if(value.Contains("health_")){string x=value.Split('_')[1];int xx=int.Parse(x); if(players[xx].hidden!=true){txt=Mathf.RoundToInt(players[xx].health).ToString();}else{txt=Math.Round(GameSession.instance.respawnTimer[xx],1).ToString();}}
        if(value.Contains("score_")){string x=value.Split('_')[1];if(GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled==true){value="kills_"+x;}  int xx=int.Parse(x);txt=GameSession.instance.score[xx].ToString();}//GameSession.instance.score[int.Parse(value.Split(["_"]))];}
        if(value.Contains("kills_")){string x=value.Split('_')[1];if(GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled!=true){value="score_"+x;}  int xx=int.Parse(x);txt=GameSession.instance.kills[xx].ToString();}
        if(value=="scoreDesc"){if((GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled!=true)||GameConditions.instance.scoreEnabled){txt="Score:";}else if((GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled)||GameConditions.instance.killsEnabled){txt="Kills:";}}
        if(value=="perksFor"){txt="CHANGE PERKS FOR PlayerScript "+(StartMenu.instance.editPerksID+1).ToString();}
        if(value.Contains("perkCount_")){string x=value.Split('_')[1];int xx=int.Parse(x);var pCount=players[xx].GetComponent<PlayerPerks>().playPerks.FindAll((p) => p != perks.empty);
        if(pCount.Count>0){
        txt=pCount.Count.ToString()+"/"+
        PerksList.instance.perkList.Length+" PERKS";}else{txt="PERKS";}}

        if(value=="timer"){GetComponent<TMPro.TextMeshProUGUI>().enabled=GameConditions.instance.timerEnabled;
            var timer=GameConditions.instance.timer;if(timer>0){float min=timer/60; float sec=Mathf.RoundToInt(timer-(float)(System.Math.Truncate(min)*60f)); if(sec>=60){min+=1;sec=0;} string textSec=sec.ToString(); if(sec<10){textSec="0"+sec;} txt=System.Math.Truncate(min).ToString()+":"+textSec;}else{txt="0:00";}}//160/60=2\.6 | 160-2*60=160-120=40
        
        if(update==true){
            if(GetComponent<TMPro.TextMeshProUGUI>()!=null)GetComponent<TMPro.TextMeshProUGUI>().text=txt;
            if(GetComponent<TMPro.TMP_InputField>()!=null)GetComponent<TMPro.TMP_InputField>().text=txt;
        }
    }
}
