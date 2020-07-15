using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ValueDisplay : MonoBehaviour{
    [SerializeField]string value;
    string txt;
    Player player1;
    Player player2;
    void Start(){
        
    }

    void Update(){
        Player[] players=FindObjectsOfType<Player>();
        foreach(Player player in players){
            if(player.playerNum==playerNum.One){player1=player;}
            if(player.playerNum==playerNum.Two){player2=player;}
        }
        if(value=="health_p1")txt=player1.health.ToString();
        if(value=="health_p2")txt=player2.health.ToString();

        GetComponent<TMPro.TextMeshProUGUI>().text=txt;
    }
}
