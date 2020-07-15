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
    Player player1;
    Player player2;
    void Start(){
        
    }

    void Update(){
        foreach(Player player in FindObjectsOfType<Player>()){
            if(player.playerNum==playerNum.One){player1=player;}
            if(player.playerNum==playerNum.Two){player2=player;}
        }
        if(valueName=="health_p1"){value=player1.health;maxValue=player1.maxHealth;}
        if(valueName=="health_p2"){value=player2.health;maxValue=player2.maxHealth;}

        if(barType==barType.Horizontal){transform.localScale=new Vector2(value/maxValue,1);}
        if(barType==barType.Vertical){transform.localScale=new Vector2(1,value/maxValue);}
        if(barType==barType.Fill){GetComponent<Image>().fillAmount=value/maxValue;}
    }
}
