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
    PlayerScript[] players;
    void Start(){
        Array.Resize(ref players,FindObjectsOfType<PlayerScript>().Length);
    }

    void Update(){
        PlayerScript[] allPlayers=FindObjectsOfType<PlayerScript>();
        foreach(PlayerScript player in allPlayers){
            //if(PlayerScript.playerNum==playerNum.One){player1=PlayerScript;}
            //if(PlayerScript.playerNum==playerNum.Two){player2=PlayerScript;}
            //if(PlayerScript.playerNum==0){player1=PlayerScript;}
            //if(PlayerScript.playerNum==1){player2=PlayerScript;}
            //Array.Resize(ref players,allPlayers.Length);
            players[player.playerNum]=player;
        }
        //if(valueName=="health_p1"){value=player1.health;maxValue=player1.maxHealth;}
        //if(valueName=="health_p2"){value=player2.health;maxValue=player2.maxHealth;}
        if(valueName.Contains("health_")){string[] x=valueName.Split('_');int xx=int.Parse(x[1]);if(players[xx].hidden!=true){value=players[xx].health;maxValue=players[xx].maxHealth;}else{value=GameSession.instance.respawnTimer[xx]; maxValue=GameSession.instance.respawnTime;}} 
        if(valueName.Contains("score_")){string[] x=valueName.Split('_');int xx=int.Parse(x[1]);value=GameSession.instance.score[xx];maxValue=GameSession.instance.score[xx];}

        if(barType==barType.Horizontal){transform.localScale=new Vector2(value/maxValue,1);}
        if(barType==barType.Vertical){transform.localScale=new Vector2(1,value/maxValue);}
        if(barType==barType.Fill){GetComponent<Image>().fillAmount=value/maxValue;}
    }
}
