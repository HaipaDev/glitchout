using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour{
    public int perkID=0;
    public perks perkEnum;
    void Start(){
        if(perkID>=0)perkEnum=(perks)perkID;
        //GetComponent<Button>().onClick.AddListener(delegate{StartMenu.instance.SetPerk(perkID);});
    }

    void Update(){
        var player=GameSession.instance.players[StartMenu.instance.editPerksID];
        if(player.GetComponent<PlayerPerks>().playPerks.Contains(perkEnum)){GetComponent<Image>().color=Color.green;}
        else{GetComponent<Image>().color=Color.white;}
    }
    public void SetPerk(){
        StartMenu.instance.SetPerk(perkEnum);
    }
}
