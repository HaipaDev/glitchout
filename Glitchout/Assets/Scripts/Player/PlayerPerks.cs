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
    [HeaderAttribute("Spectre")]
    public GameObject[] spectres=new GameObject[4];
    public float spectreSwitchTime=8f;
    public float spectreSwitchTimer=-4;
    [HeaderAttribute("Recovery")]
    public float recoveryHealth=16f;
    public float recoveryLifetime=3.4f;
    public float recoveryLifetimer=-4;

    int splitId;

    PlayerScript player;
    void Start(){
        player=GetComponent<PlayerScript>();
    }
    void OnValidate() {
        var allPerks=Enum.GetValues(typeof(perks));
        if(playPerks.Count==0)foreach(var pk in allPerks)if((perks)pk!=perks.empty)playPerks.Add(perks.empty);//Resize playPerks
    }

    void Update(){
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
            if(perk==perks.spectre){
                if(spectreSwitchTimer==-4)spectreSwitchTimer=spectreSwitchTime;
                if(spectreSwitchTimer>0)spectreSwitchTimer-=Time.deltaTime;
                if(spectreSwitchTimer<=0&&spectreSwitchTimer!=-4){
                int rndm=UnityEngine.Random.Range(0,spectres.Length-1);
                Debug.Log(rndm);
                if(spectres[rndm]!=null){   
                    Vector2 selfPos=transform.position;
                    Vector2 spectrePos=spectres[rndm].transform.position;
                    player.xpos=spectrePos.x;
                    player.ypos=spectrePos.y;
                    foreach(GameObject spectre in spectres){var spectrePosC=spectre.transform.position;spectre.GetComponent<FollowStrict>().xx=spectrePos.x-spectrePosC.x;spectre.GetComponent<FollowStrict>().yy=spectrePos.y-spectrePosC.y;}
                    spectres[rndm].transform.position=selfPos;
                    //spectres[rndm].GetComponent<FollowStrict>().targetObj=null;
                    //spectres[rndm].GetComponent<FollowStrict>().targetPos=selfPos;
                    /*foreach(GameObject spectre in spectres){if(spectre!=spectres[rndm]){spectre.GetComponent<FollowStrict>().targetObj=spectres[rndm];}}*/
                }
                spectreSwitchTimer=spectreSwitchTime;
                }
            }if(perk==perks.recovery){
                if(recoveryLifetimer>0)recoveryLifetimer-=Time.deltaTime;
                if(recoveryLifetimer<=0&&recoveryLifetimer!=-4&&player.health>0){
                    player.Death();
                    recoveryLifetimer=-4;
                }
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
    public void RespawnPerks(){
        foreach(perks perk in playPerks){
            if(perk==perks.spectre){
                GameObject parent=new GameObject();
                //GameObject parent=Instantiate(new GameObject(),transform);
                parent.name="SpectresParent"+(player.playerNum+1).ToString();
                for(var i=0;i<spectres.Length;i++){
                    GameObject go=Instantiate(GameAssets.instance.Get("Spectre"),parent.transform);
                    spectres[i]=go;
                    go.name="Spectre"+(i+1).ToString();
                    go.GetComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
                    go.GetComponent<Spectre>().playerID=player.playerNum;

                    Vector2 pos=transform.position;
                    float xs=0.3f;float xm=1.2f;
                    float xx=0;float yy=0;
                    if(i==0){xx=UnityEngine.Random.Range(xs,xm);yy=UnityEngine.Random.Range(xs,xm);}
                    if(i==1){xx=-UnityEngine.Random.Range(xs,xm);yy=UnityEngine.Random.Range(xs,xm);}
                    if(i==2){xx=UnityEngine.Random.Range(xs,xm);yy=-UnityEngine.Random.Range(xs,xm);}
                    if(i==3){xx=-UnityEngine.Random.Range(xs,xm);yy=-UnityEngine.Random.Range(xs,xm);}
                    go.transform.position=new Vector2(pos.x+xx,pos.y+yy);
                    go.GetComponent<FollowStrict>().targetObj=this.gameObject;
                    go.GetComponent<FollowStrict>().xx=xx;
                    go.GetComponent<FollowStrict>().yy=yy;
                }
            }
        }
    }

    void MakeSplit(int id){
        //for(var i=0;i<3;i++){
            GameObject go=Instantiate(GameAssets.instance.Get("SplitBt"),transform.position,Quaternion.identity);
            go.GetComponent<SplitBullet>().playerID=player.playerNum;
            go.GetComponent<SplitBullet>().coords=GameSession.instance.players.Where(x => x.playerScript.playerNum != player.playerNum).SingleOrDefault().playerScript.transform.position;
            go.GetComponent<SpriteRenderer>().sprite=GetComponent<SpriteRenderer>().sprite;
            if(id==0)go.GetComponent<SpriteRenderer>().color=Color.red;
            if(id==1)go.GetComponent<SpriteRenderer>().color=Color.green;
            if(id==2)go.GetComponent<SpriteRenderer>().color=Color.blue;
        //}
    }
}
