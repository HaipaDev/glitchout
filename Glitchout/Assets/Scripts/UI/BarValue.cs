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

    void Update(){
        if(GameSession.instance.players.Length>0){
        string x="0";int xx=0;if(valueName.Contains("_")){x=valueName.Split('_')[1];xx=int.Parse(x);}
        if(GameSession.instance.players.Length>xx&&GameSession.instance.players[xx]!=null){
            if(GameSession.instance.players[xx].playerScript!=null){
                if(valueName.Contains("health_")){
                    if(GameSession.instance.players[xx].playerScript.hidden!=true){value=GameSession.instance.players[xx].playerScript.health;maxValue=GameSession.instance.players[xx].playerScript.maxHealth;}
                    else{if(GameSession.instance.players.Length>xx)value=GameSession.instance.players[xx].respawnTimer;maxValue=GameSession.instance.respawnTime;}
                }
                //if(valueName.Contains("score_")){string[] x=valueName.Split('_');int xx=int.Parse(x[1]); if(GameSession.instance.players.Length>xx)value=GameSession.instance.players[xx].score;maxValue=GameSession.instance.players[xx].score;}
            }
        }}
        
        if(barType==barType.Horizontal){transform.localScale=new Vector2(value/maxValue,1);}
        if(barType==barType.Vertical){transform.localScale=new Vector2(1,value/maxValue);}
        if(barType==barType.Fill){GetComponent<Image>().fillAmount=value/maxValue;}
    }
}
