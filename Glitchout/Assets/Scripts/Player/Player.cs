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
    [SerializeField]float rotationSpeed=30;
    [SerializeField]float xspeed=0.1f;
    [SerializeField]float yspeed=0.1f;
    [SerializeField]float xRange=0.8f;
    [SerializeField]float yRange=0.8f;
    [SerializeField]float xBound=4f;
    [SerializeField]float yBound=6.8f;
    [SerializeField]public float maxHealth=100f;
    [SerializeField]public float dmgFreq=0.38f;
    [SerializeField]public float hitTimerMax=3f;
    [HeaderAttribute("Current Values")]
    public float health;
    public float damage;
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
    void Start(){
        shake = GameObject.FindObjectOfType<Shake>();
        
        health=maxHealth;
        damage=GetComponent<DamageDealer>().GetDmgPlayer();
    }

    void Update(){
        if(hidden==false){
            health=Mathf.Clamp(health,0,maxHealth);
            if(dmgTimer>0)dmgTimer-=Time.deltaTime;
            if(hitTimer>0)hitTimer-=Time.deltaTime;
            else hitTimer=-4;
            Move();
            Die();
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
            ypos+=yspeed;
            angle+=rotationSpeed;
        }if(keyDown&&!hMove){
            ypos-=yspeed;
            angle-=rotationSpeed;
        }
        if(keyLeft){
            xpos-=xspeed;
            angle-=rotationSpeed;
            if(keyUp){ypos+=yspeed;}
            if(keyDown){ypos-=yspeed;}
        }if(keyRight){
            xpos+=xspeed;
            angle+=rotationSpeed;
            if(keyUp){ypos+=yspeed;}
            if(keyDown){ypos-=yspeed;}
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
        
        shake.CamShake(1f*dmg,1f);
    }
    public void GlitchOut(){
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
            GameSession.instance.Die(playerNum,hitTimer);
            //gameObject.SetActive(false);
            Hide();
            shake.CamShake(4f,0.4f);
            GameAssets.instance.VFX("ExplosionGlitch",transform.position,0.5f);
            AudioManager.instance.Play("Death");
        }
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
        damage=GetComponent<DamageDealer>().GetDmgPlayer();
        if(CompareTag(other.tag)){
            if(moving==true){other.GetComponent<Player>().Damage(damage);other.GetComponent<Player>().hitTimer=hitTimerMax;}
            GlitchOut();
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
            damage=GetComponent<DamageDealer>().GetDmgPlayerStay();
            if(CompareTag(other.tag)){
                if(moving==true)other.GetComponent<Player>().Damage(damage);
                GlitchOut();
            }else{
                var dmgDealer=other.GetComponent<DamageDealer>();
                if(other.GetComponent<Saw>()!=null){Damage(dmgDealer.GetDmgSaw());AudioManager.instance.Play("SawHit");int i=Random.Range(0,2);GameAssets.instance.VFX(i.ToString(),transform.position,0.2f);}
            }
            dmgTimer=dmgFreq;
        }
    }
}
