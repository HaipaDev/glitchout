using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum barType{
    Horizontal,
    Vertical,
    Fill
}
public class BarValue : MonoBehaviour{
    [SerializeField] barType barType=barType.Horizontal;
    [SerializeField] string valueName;
    [SerializeField] float value;
    //[SerializeField] string maxValueName;
    [SerializeField] float maxValue;
    [SerializeField]PlayerScript[] players;

    void Update(){
        PlayerScript[] allPlayers=FindObjectsOfType<PlayerScript>();
        if(allPlayers.Length>0){
            if(players.Length!=allPlayers.Length)Array.Resize(ref players,allPlayers.Length);
            foreach(PlayerScript player in allPlayers){
                players[player.playerNum]=player;
            }
            //if(valueName=="health_p1"){value=player1.health;maxValue=player1.maxHealth;}
            //if(valueName=="health_p2"){value=player2.health;maxValue=player2.maxHealth;}
            if(valueName.Contains("health_")){string[] x=valueName.Split('_');int xx=int.Parse(x[1]); if(players.Length>xx)if(players[xx].hidden!=true){value=players[xx].health;maxValue=players[xx].maxHealth;}
                else{ if(GameSession.instance.respawnTimer.Length>xx)value=GameSession.instance.respawnTimer[xx];maxValue=GameSession.instance.respawnTime;}} 
            if(valueName.Contains("score_")){string[] x=valueName.Split('_');int xx=int.Parse(x[1]); if(GameSession.instance.score.Length>xx)value=GameSession.instance.score[xx];maxValue=GameSession.instance.score[xx];}
        }
        
        if(barType==barType.Horizontal){transform.localScale=new Vector2(value/maxValue,1);}
        if(barType==barType.Vertical){transform.localScale=new Vector2(1,value/maxValue);}
        if(barType==barType.Fill){GetComponent<Image>().fillAmount=value/maxValue;}
    }
}
