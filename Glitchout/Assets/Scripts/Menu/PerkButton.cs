using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerkButton : MonoBehaviour{
    public int perkID=0;
    public perks perkEnum;
    public string txtTitle="TITLE";
    public string txt="DESCRIPTION";
    void Start(){
        if(perkID>=0)perkEnum=(perks)perkID;
        //GetComponent<Button>().onClick.AddListener(delegate{StartMenu.instance.SetPerk(perkID);});

        if(perkEnum!=perks.empty)txtTitle=perkEnum.ToString();
        if(perkEnum==perks.refined)txt="MORE STARTING HEALTH, SLOWER SPEED";
        if(perkEnum==perks.autofix)txt="YOU GET SLOWLY HEALED WHEN NOT DAMAGED";
        if(perkEnum==perks.unstable)txt="HIGHER DAMAGE, BUT RANDOM SPAZ";
        if(perkEnum==perks.split)txt="YOU SPLIT INTO RGB BULLETS";
        if(perkEnum==perks.spectre)txt="YOU CREATE FAKE COPIES OF YOURSELF";
        if(perkEnum==perks.recovery)txt="YOU GET A SHORT SECOND LIFE";

        //transform.GetChild(0).GetComponent<ClampUI>().ID=int.Parse(gameObject.name.Split('_')[1]);
    }

    void Update(){
        var txtTitleGo=transform.GetChild(0).GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
        var txtGo=transform.GetChild(0).GetChild(1).GetComponent<TMPro.TextMeshProUGUI>();
        txtTitleGo.text=txtTitle;
        txtGo.text=txt;

        var player=GameSession.instance.players[StartMenu.instance.editPerksID];
        if(player.GetComponent<PlayerPerks>().playPerks.Contains(perkEnum)){GetComponent<Image>().color= new Color(177,255,0);}
        else{GetComponent<Image>().color=Color.white;}
    }
    public void SetPerk(){
        StartMenu.instance.SetPerk(perkEnum);
    }
}
