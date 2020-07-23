using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSkin : MonoBehaviour{
    public int playerID=-1;
    public int skinID;
    public Sprite[] skins=new Sprite[7];
    void Start(){
        StartCoroutine(StartI());
        
    }
    IEnumerator StartI(){
        //GameSession.instance.speedChanged=true;
        //GameSession.instance.gameSpeed=1;

        if(GetComponent<Image>()!=null){//Set SkinID from Player
            foreach(Player player in FindObjectsOfType<Player>()){if(player.playerNum==this.playerID){this.skinID=player.GetComponent<PlayerSkin>().skinID;}}
        }
        for(var i=0;i<skins.Length;i++){//Set all skins
            skins[i]=GameAssets.instance.Spr("player"+i.ToString());
        }
        //Set skinID from current sprite
        string[] num=new string[2];
        if(GetComponent<SpriteRenderer>()!=null)num=GetComponent<SpriteRenderer>().sprite.name.Split('r');
        if(GetComponent<Image>()!=null)num=GetComponent<Image>().sprite.name.Split('r');
        skinID=int.Parse(num[1]);

        
        GameSession.instance.speedChanged=true;
        GameSession.instance.gameSpeed=0;
        yield return new WaitForSecondsRealtime(0.005f);
    }
    void Update(){
        if(skinID>=0&&skinID<skins.Length){
            if(GetComponent<Player>()!=null){//Set SkinID for Player from config
                foreach(PlayerSkin skin in FindObjectsOfType<PlayerSkin>()){if(skin.playerID==GetComponent<Player>().playerNum){GetComponent<PlayerSkin>().skinID=skin.skinID;}}
            }

            if(GetComponent<SpriteRenderer>()!=null)GetComponent<SpriteRenderer>().sprite=skins[skinID];
            if(GetComponent<Image>()!=null)GetComponent<Image>().sprite=skins[skinID];
        }

        //Wrap numbers out of index
        if(skinID<0){skinID=skins.Length-Mathf.Abs(skinID);}//-1 | 6-1=5
        if(skinID>=skins.Length){skinID=0+skinID-skins.Length;}//7 | 7-7=0
    }
    public void SkinPrev(){
        skinID--;
        //foreach(PlayerSkin skin in FindObjectsOfType<PlayerSkin>()){if(skin.skinID!=skinID-1){skinID--;}else{skinID-=2;}}
    }
    public void SkinNext(){
        skinID++;
        //foreach(PlayerSkin skin in FindObjectsOfType<PlayerSkin>()){if(skin.skinID!=skinID+1){skinID++;}else{skinID+=2;}}
    }
}
