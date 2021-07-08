using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    [SerializeField]string value;
    [SerializeField]bool update=true;
    string txt;
    void Start(){
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
        if(GameSession.instance.players.Length>0){
        string x="0";int xx=0;if(value.Contains("_")){x=value.Split('_')[1];xx=int.Parse(x);}
        if(GameSession.instance.players.Length>xx&&GameSession.instance.players[xx]!=null){
                if(value.Contains("score_")){if(GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled==true){value="kills_"+x;}if(GameSession.instance.players.Length>xx)txt=GameSession.instance.players[xx].score.ToString();}//GameSession.instance.score[int.Parse(value.Split(["_"]))];}
                if(value.Contains("kills_")){if(GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled!=true){value="score_"+x;}if(GameSession.instance.players.Length>xx)txt=GameSession.instance.players[xx].kills.ToString();}
                if(value.Contains("nick_")){if(GameSession.instance.players.Length>xx)if(!String.IsNullOrEmpty(GameSession.instance.players[xx].nick)){txt=GameSession.instance.players[xx].nick;}else{txt="Player"+(xx+1).ToString();}}
                if(GameSession.instance.players[xx].playerScript!=null){
                    if(value.Contains("health_")){if(GameSession.instance.players[xx].playerScript.hidden!=true){txt=Mathf.RoundToInt(GameSession.instance.players[xx].playerScript.health).ToString();}else{txt=Math.Round(GameSession.instance.players[xx].respawnTimer,1).ToString();}}
                    if(value.Contains("perkCount_")){List<perks> pCount=null;
                    if(GameSession.instance.players[xx].playerScript.GetComponent<PlayerPerks>()!=null)pCount=GameSession.instance.players[xx].playerScript.GetComponent<PlayerPerks>().playPerks.FindAll(p=>p!=perks.empty);
                    if(pCount.Count>0){
                    txt=pCount.Count.ToString()+"/"+
                    PerksList.instance.perkList.Length+" PERKS";}else{txt="PERKS";}}
                }else{Debug.LogWarning("No PlayerScript attached to Player"+xx);}
        }}
        if(value.Contains("gameVersion")){txt=SaveSerial.instance.settingsData.gameVersion;}
        if(value=="scoreDesc"){if((GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled!=true)||GameConditions.instance.scoreEnabled){txt="Score:";}else if((GameConditions.instance.timerEnabled&&GameConditions.instance.timeKillsEnabled)||GameConditions.instance.killsEnabled){txt="Kills:";}}
        if(value=="perksFor"){txt="CHANGE PERKS FOR Player "+(StartMenu.instance.editPerksID+1).ToString();}

        if(value=="timer"){GetComponent<TMPro.TextMeshProUGUI>().enabled=GameConditions.instance.timerEnabled;
            var timer=GameConditions.instance.timer;if(timer>0){float min=timer/60; float sec=Mathf.RoundToInt(timer-(float)(System.Math.Truncate(min)*60f)); if(sec>=60){min+=1;sec=0;} string textSec=sec.ToString(); if(sec<10){textSec="0"+sec;} txt=System.Math.Truncate(min).ToString()+":"+textSec;}else{txt="0:00";}}//160/60=2\.6 | 160-2*60=160-120=40
        
        if(update==true){
            if(GetComponent<TMPro.TextMeshProUGUI>()!=null)GetComponent<TMPro.TextMeshProUGUI>().text=txt;
            if(GetComponent<TMPro.TMP_InputField>()!=null)GetComponent<TMPro.TMP_InputField>().text=txt;
        }
    }
}
