using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum perks{
    empty,
    tank,
    lizard,
    unstable,
    split,
    bubble,
    illusion,
    undead
}
public class PlayerPerks : MonoBehaviour{
    public List<perks> playPerks;
    public float splitTime=5;
    public float splitTimer=-4;
    void Start(){
        
    }

    void Update(){
        if(Time.timeScale>0.0001f){
        foreach(perks perk in playPerks){
            if(perk==perks.split){
                if(splitTimer==-4)splitTimer=splitTime;
                if(splitTimer>0)splitTimer-=Time.deltaTime;
                if(splitTimer<=0&&splitTimer!=-4){
                    
                    
                    splitTimer=splitTime;
                }
            }
        }
        }
    }
}
