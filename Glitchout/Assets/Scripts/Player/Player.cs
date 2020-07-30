using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*public enum playerNum{
    One,
    Two
}*/
public class Player : MonoBehaviour{
    [HeaderAttribute("Setup")]
    //[SerializeField]public playerNum playerNum=playerNum.One;
    [SerializeField]public int playerNum;
    [SerializeField]public float rotationSpeed=30;
    [SerializeField]public float xspeed=0.1f;
    [SerializeField]public float yspeed=0.1f;
    [SerializeField]public float xRange=0.8f;
    [SerializeField]public float yRange=0.8f;
    [SerializeField]float xBound=4f;
    [SerializeField]float yBound=6.8f;
    [SerializeField]public float maxHealth=100f;
    [SerializeField]public float dmgFreq=0.38f;
    [SerializeField]public float hitTimerMax=3f;
    [HeaderAttribute("Current Values")]
    public float health;
    public float damage=-4;
    public float angle;
    public float xpos;
    public float ypos;
    public float dmgTimer;
    public float hitTimer;

    bool keyUp;
    bool keyDown;
    bool keyLeft;
    bool keyRight;
    bool hMove;
    bool vMove;
    bool moving;
    public bool hidden;

    Shake shake;
    GameObject glowVFX;
    void Start(){
        shake = GameObject.FindObjectOfType<Shake>();
        
        health=maxHealth;
        if(damage==-4)damage=GetComponent<DamageDealer>().GetDmgPlayer();

        glowVFX=Instantiate(GameAssets.instance.GetVFX("PlayerGlow"),transform);
        var ps=glowVFX.GetComponent<ParticleSystem>().main;
        if(playerNum==0)ps.startColor=Color.cyan;
        if(playerNum==1)ps.startColor=Color.magenta;
    }

    void Update(){
        if(Time.timeScale>0.0001f&&hidden==false){
            health=Mathf.Clamp(health,0,maxHealth);
            if(dmgTimer>0)dmgTimer-=Time.deltaTime;
            if(hitTimer>0)hitTimer-=Time.deltaTime;
            else hitTimer=-4;
            Move();
            Die();
        }
        var ps=glowVFX.GetComponent<ParticleSystem>().main;
        var em=glowVFX.GetComponent<ParticleSystem>().emission;
        if(health>0){
        var dur=Mathf.Clamp(0.05f/(health/maxHealth)*Mathf.Clamp((0.3f/(health/maxHealth)),0,1),0.05f,1.5f);
        if(ps.duration!=dur){glowVFX.GetComponent<ParticleSystem>().Stop();ps.duration=dur;glowVFX.GetComponent<ParticleSystem>().Play();}
        ps.startLifetime=(float)System.Math.Round(Mathf.Clamp(0.5f/(health/maxHealth),0.5f,1.5f),1);
        ps.maxParticles=(int)Mathf.Clamp((10*(float)System.Math.Round((health/maxHealth),1)),1,10);
        if(health<maxHealth/2){em.rateOverTime=1;}
        else{em.rateOverTime=2;}
        }
    }

    private void Move(){
        //if(playerNum==playerNum.One){
        if(playerNum==0){
            keyUp=Input.GetKey(KeyCode.W);
            keyDown=Input.GetKey(KeyCode.S);
            keyLeft=Input.GetKey(KeyCode.A);
            keyRight=Input.GetKey(KeyCode.D);
        }//else if(playerNum==playerNum.Two){
        else if(playerNum==1){
            keyUp=Input.GetKey(KeyCode.UpArrow);
            keyDown=Input.GetKey(KeyCode.DownArrow);
            keyLeft=Input.GetKey(KeyCode.LeftArrow);
            keyRight=Input.GetKey(KeyCode.RightArrow);
        }
        if(keyLeft||keyRight)hMove=true;
        else hMove=false;
        if(keyUp||keyDown)vMove=true;
        else vMove=false;
        if(hMove||vMove){moving=true;AudioManager.instance.Play("Spinning");}
        else moving=false;

        //ypos=transform.position.y;
        //xpos=transform.position.x;

        if(keyUp&&!hMove){
            ypos+=yspeed*Time.timeScale;
            angle+=rotationSpeed*Time.timeScale;
        }if(keyDown&&!hMove){
            ypos-=yspeed*Time.timeScale;
            angle-=rotationSpeed*Time.timeScale;
        }
        if(keyLeft){
            xpos-=xspeed*Time.timeScale;
            angle-=rotationSpeed*Time.timeScale;
            if(keyUp){ypos+=yspeed*Time.timeScale;}
            if(keyDown){ypos-=yspeed*Time.timeScale;}
        }if(keyRight){
            xpos+=xspeed*Time.timeScale;
            angle+=rotationSpeed*Time.timeScale;
            if(keyUp){ypos+=yspeed*Time.timeScale;}
            if(keyDown){ypos-=yspeed*Time.timeScale;}
        }

        transform.position=new Vector2(xpos,ypos);
        
        if(angle>=360)angle=0;
        if(angle<0)angle=360;
        //Vector3 to=new Vector3(0,0,angle);
        //Quaternion myQuat=Quaternion.identity;
        //myQuat.eulerAngles =new Vector3(0,0,angle);
        transform.eulerAngles=new Vector3(0, 0, angle);
    }

    public void Damage(float dmg){
        health-=dmg;
        GetComponent<PlayerPerks>().dmgdTimer=GetComponent<PlayerPerks>().dmgdTime*Mathf.Clamp(dmg,0,10);
        
        shake.CamShake(1f*dmg,1f);
    }
    public void GlitchOut(float xRange,float yRange){
        var xx=Random.Range(-xRange,xRange);
        var yy=Random.Range(-yRange,yRange);

        xpos+=xx;
        ypos+=yy;
        GameAssets.instance.VFX("GlitchHit",transform.position,0.2f);
        AudioManager.instance.Play("Hit");
    }
    public void TpMiddle(){
        xpos=0;
        ypos=0;
        AudioManager.instance.Play("Hit");
    }public void TpRandom(){
        xpos=Random.Range(-xBound,xBound);
        ypos=Random.Range(-yBound,yBound);
    }

    private void Die(){
        if(health<=0){
            if(GetComponent<PlayerPerks>().playPerks.Contains(perks.recovery)&&GetComponent<PlayerPerks>().recoveryLifetimer==-4){//Recovery Death
                RecoveryDeath();
            }else{//Normal death
                Death();
            }
        }
    }public void Death(){
        GameSession.instance.Die(playerNum,hitTimer);
        //gameObject.SetActive(false);
        Hide();
        shake.CamShake(4f,0.4f);
        GameAssets.instance.VFX("ExplosionGlitch",transform.position,0.5f);
        AudioManager.instance.Play("Death");
    }public void RecoveryDeath(){
        var pperks=GetComponent<PlayerPerks>();
        health=pperks.recoveryHealth;
        pperks.recoveryLifetimer=pperks.recoveryLifetime;
        //AudioManager.instance.Play("Death");
        AudioManager.instance.Play("Respawn");
        GameAssets.instance.VFX("Respawn",new Vector2(xpos,ypos),0.1f);
    }public void Respawn(){
        health=maxHealth;
        TpRandom();
        UnHide();
        GameAssets.instance.VFX("Respawn",new Vector2(xpos,ypos),0.3f);
        AudioManager.instance.Play("Respawn");
    }
    private void Hide(){
        hidden=true;
        GetComponent<SpriteRenderer>().enabled=false;
        GetComponent<Collider2D>().enabled=false;
    }private void UnHide(){
        hidden=false;
        GetComponent<SpriteRenderer>().enabled=true;
        GetComponent<Collider2D>().enabled=true;
    }

    private void OnTriggerEnter2D(Collider2D other){
        //damage=GetComponent<DamageDealer>().GetDmgPlayer();
        if(CompareTag(other.tag)){
            //Player
            if(other.GetComponent<Player>()!=null){
                if(moving==true){other.GetComponent<Player>().Damage(damage);other.GetComponent<Player>().hitTimer=hitTimerMax;}
                GlitchOut(xRange,yRange);
            }if(other.GetComponent<SplitBullet>()!=null){//SplitBullet
            if(other.GetComponent<SplitBullet>().playerID!=playerNum){
                Damage(other.GetComponent<DamageDealer>().GetDmgSplit());
                GlitchOut(xRange,yRange);
            }
            }
        }else{
            var dmgDealer=other.GetComponent<DamageDealer>();
            if(other.GetComponent<HealthPack>()!=null&&other.GetComponent<HealthPack>().timer<=0){health+=25;other.GetComponent<HealthPack>().timer=other.GetComponent<HealthPack>().timerMax;AudioManager.instance.Play("HPCollect");}
            if(other.GetComponent<Saw>()!=null){Damage(dmgDealer.GetDmgSaw()*2);AudioManager.instance.Play("SawHit");int i=Random.Range(0,2);GameAssets.instance.VFX(i.ToString(),transform.position,0.2f);}
            if(other.GetComponent<Tag_Barrier>()!=null){Damage(dmgDealer.GetDmgZone());TpMiddle();}
        }
        dmgTimer=0;
    }
    private void OnTriggerStay2D(Collider2D other){
        if(dmgTimer<=0){
            //damage=GetComponent<DamageDealer>().GetDmgPlayerStay();
            if(CompareTag(other.tag)){
                if(other.GetComponent<Player>()!=null){
                    if(moving==true)other.GetComponent<Player>().Damage(damage/5);
                    GlitchOut(xRange,yRange);
                }
            }else{
                var dmgDealer=other.GetComponent<DamageDealer>();
                if(other.GetComponent<Saw>()!=null){Damage(dmgDealer.GetDmgSaw());AudioManager.instance.Play("SawHit");int i=Random.Range(0,2);GameAssets.instance.VFX(i.ToString(),transform.position,0.2f);}
            }
            dmgTimer=dmgFreq;
        }
    }
}
