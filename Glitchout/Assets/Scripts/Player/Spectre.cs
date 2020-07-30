using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spectre : MonoBehaviour{
    public int playerID;
    GameObject glowVFX;
    Player player;
    void Start(){
        glowVFX=Instantiate(GameAssets.instance.GetVFX("PlayerGlow"),transform);
        var ps=glowVFX.GetComponent<ParticleSystem>().main;
        var em=glowVFX.GetComponent<ParticleSystem>().emission;
        ps.maxParticles=5;
        em.rateOverTime=1;
        if(playerID==0)ps.startColor=Color.cyan;
        if(playerID==1)ps.startColor=Color.magenta;
    }

    void Update(){
        player=GameSession.instance.players.Where(x => x.GetComponent<Player>().playerNum == playerID).SingleOrDefault();
        transform.rotation=player.transform.rotation;
        if(player.hidden!=true){
            var ps=glowVFX.GetComponent<ParticleSystem>().main;
            var em=glowVFX.GetComponent<ParticleSystem>().emission;
            /*if(player.health>0){
            var dur=Mathf.Clamp(0.05f/(player.health/player.maxHealth)*Mathf.Clamp((0.3f/(player.health/player.maxHealth)),0,1),0.05f,1.5f);
            if(ps.duration!=dur){glowVFX.GetComponent<ParticleSystem>().Stop();ps.duration=dur;glowVFX.GetComponent<ParticleSystem>().Play();}
            ps.startLifetime=(float)System.Math.Round(Mathf.Clamp(0.5f/(player.health/player.maxHealth),0.5f,1.5f),1);
            ps.maxParticles=(int)Mathf.Clamp((10*(float)System.Math.Round((player.health/player.maxHealth),1)),1,10);
            if(player.health<player.maxHealth/2){em.rateOverTime=1;}
            else{em.rateOverTime=2;}
            }*/
        }
    }
}
