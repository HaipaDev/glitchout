using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PerksList : MonoBehaviour{
    public Sprite[] perkList;
    public GameObject elementPrefab;
    [SerializeField] int rowMax=5;
    void Start(){
        var i=0;
        var ii=1;
        foreach(Sprite perk in perkList){
            GameObject go=Instantiate(elementPrefab,transform);
            go.GetComponent<Image>().sprite=perk;
            i++;
            string nam=go.name.Replace("_01(Clone)",("_"+i.ToString()));
            go.name=nam;
            //go.GetComponent<Button>().onClick.AddListener(StartMenu.SetPerk(i));
            //go.GetComponent<Button>().onClick.AddListener(delegate{StartMenu.instance.SetPerk(i);});
            go.GetComponent<PerkButton>().perkID=i;
            go.transform.GetChild(0).GetComponent<ClampUI>().ID=ii;
            go.transform.GetChild(0).GetComponent<ClampUI>().SetMaxID(rowMax);
            if(ii<rowMax)ii++;
            else{ii=1;}
        }
        foreach(PlayerPerks playerPerks in FindObjectsOfType<PlayerPerks>()){
            if(playerPerks.playPerks.Count>perkList.Length)playerPerks.playPerks.RemoveAt(playerPerks.playPerks.Count-1);
            //Array.Resize(ref playerPerks.playPerks,perkList.Length);
        }
    }

    void Update(){
        foreach(PlayerPerks playerPerks in FindObjectsOfType<PlayerPerks>()){
            if(playerPerks.playPerks.Count>perkList.Length)playerPerks.playPerks.RemoveAt(playerPerks.playPerks.Count-1);
            //Array.Resize(ref playerPerks.playPerks,perkList.Length);
        }
    }
}
