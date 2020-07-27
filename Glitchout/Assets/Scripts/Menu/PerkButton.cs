using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour{
    public int perkID=0;
    public perks perkEnum;
    public string txt;
    void Start(){
        if(perkID>=0)perkEnum=(perks)perkID;
        //GetComponent<Button>().onClick.AddListener(delegate{StartMenu.instance.SetPerk(perkID);});

        var txtGo=transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        if(perkEnum==perks.split)txt="THIS IS A SPLIT PERK\r\nIT MAKES YOU SPLIT";

        txtGo.text=txt;
        //transform.GetChild(0).GetComponent<ClampUI>().ID=int.Parse(gameObject.name.Split('_')[1]);
    }

    void Update(){
        var player=GameSession.instance.players[StartMenu.instance.editPerksID];
        if(player.GetComponent<PlayerPerks>().playPerks.Contains(perkEnum)){GetComponent<Image>().color= new Color(177,255,0);}
        else{GetComponent<Image>().color=Color.white;}
    }
    public void SetPerk(){
        StartMenu.instance.SetPerk(perkEnum);
    }
}
