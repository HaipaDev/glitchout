using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum perks{
    empty,
    hardened,
    autofix,
    unstable,
    split,
    //bubble,
    spectre,
    recovery
}
public class PlayerPerks : MonoBehaviour{
    public List<perks> playPerks;
    [HeaderAttribute("Hardened")]
    public float hardenedHealth=115;
    [HeaderAttribute("Autofix")]
    public float dmgdTime=0.5f;
    public float dmgdTimer=-4;
    public float afixRegAmnt=1;
    public float afixRegTime=1.3f;
    public float afixRegTimer=-4;
    [HeaderAttribute("Unstable")]
    public float unstableDmg=3.3f;
    public float unstableTimeS=4;
    public float unstableTimeE=8;
    public float unstableTimer=-4;
    [HeaderAttribute("Split")]
    public float splitTime=5;
    public float splitTimer=-4;
    public float splitTimeInst=0.38f;
    public float splitTimerInst=-4;

    int splitId;

    Player player;

    private void Awake() {
        
    }
    private void Start(){
        player=GetComponent<Player>();
    }

    private void Update(){
        if(Time.timeScale>0.0001f){
        foreach(perks perk in playPerks){
            if(perk==perks.autofix){
                if(dmgdTimer>0){afixRegTimer=-4;dmgdTimer-=Time.deltaTime;}
                if(dmgdTimer<=0&&dmgdTimer!=-4){afixRegTimer=afixRegTime;dmgdTimer=-4;}
                if(afixRegTimer>0){afixRegTimer-=Time.deltaTime;}
                if(afixRegTimer<=0&&afixRegTimer!=-4){
                if(player.health<player.maxHealth){
                    player.health+=afixRegAmnt;
                    AudioManager.instance.Play("HealAFix");
                    afixRegTimer=afixRegTime;
                }
                }
            }
            if(perk==perks.unstable){
                if(unstableTimer==-4)unstableTimer=UnityEngine.Random.Range(unstableTimeS,unstableTimeE);
                if(unstableTimer>0)unstableTimer-=Time.deltaTime;
                if(unstableTimer<=0&&unstableTimer!=-4){
                    player.Damage(unstableDmg);
                    player.GlitchOut(player.xRange*1.66f,player.yRange*1.66f);
                    unstableTimer=UnityEngine.Random.Range(unstableTimeS,unstableTimeE);
                }
            }
            if(perk==perks.split){
                if(splitTimer==-4)splitTimer=splitTime;
                if(splitTimer>0)splitTimer-=Time.deltaTime;
                if(splitTimer<=0&&splitTimer!=-4){
                    splitTimerInst=splitTimeInst;
                    splitTimer=splitTime;
                }
                if(splitTimerInst>0){splitTimerInst-=Time.deltaTime;}
                if(splitTimerInst<=0&&splitTimerInst!=-4){MakeSplit(splitId);if(splitId<2){splitId++;splitTimerInst=splitTimeInst;}else{splitId=0;splitTimerInst=-4;}}
            }
        }
        }
    }
    public void SetStartParams(){
        foreach(perks perk in playPerks){
            if(perk==perks.hardened){
                player.maxHealth=hardenedHealth;
                player.health=hardenedHealth;
                player.xspeed*=0.75f;
                player.yspeed*=0.75f;
                player.rotationSpeed*=0.75f;
            }if(perk==perks.unstable){
                player.damage=7.7f;
                player.xRange*=1.1f;
                player.yRange*=1.1f;
            }
        }
    }

    void MakeSplit(int id){
        //for(var i=0;i<3;i++){
            GameObject go=Instantiate(GameAssets.instance.Get("SplitBt"),transform.position,Quaternion.identity);
            go.GetComponent<SplitBullet>().playerID=GetComponent<Player>().playerNum;
            go.GetComponent<SplitBullet>().coords=GameSession.instance.players.Where(x => x.GetComponent<Player>().playerNum != GetComponent<Player>().playerNum).SingleOrDefault().transform.position;
            go.GetComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
            if(id==0)go.GetComponent<SpriteRenderer>().color=Color.red;
            if(id==1)go.GetComponent<SpriteRenderer>().color=Color.green;
            if(id==2)go.GetComponent<SpriteRenderer>().color=Color.blue;
        //}
    }
}
