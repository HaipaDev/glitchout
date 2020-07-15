using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum playerNum{
    One,
    Two
}
public class Player : MonoBehaviour{
    [HeaderAttribute("Setup")]
    [SerializeField]public playerNum playerNum=playerNum.One;
    [SerializeField]float rotationSpeed=30;
    [SerializeField]float xspeed=0.1f;
    [SerializeField]float yspeed=0.1f;
    [SerializeField]float xRange=0.8f;
    [SerializeField]float yRange=0.8f;
    [SerializeField]public float maxHealth=100f;
    [HeaderAttribute("Current Values")]
    public float health;
    public float damage;
    public float angle;
    public float xpos;
    public float ypos;
    bool keyUp;
    bool keyDown;
    bool keyLeft;
    bool keyRight;
    bool hMove;
    bool vMove;
    bool moving;
    void Start(){
        health=maxHealth;
        damage=GetComponent<DamageDealer>().GetDmgPlayer();
    }

    void Update(){
        health=Mathf.Clamp(health,0,maxHealth);
        Move();
        Die();
    }

    private void Move(){
        if(playerNum==playerNum.One){
            keyUp=Input.GetKey(KeyCode.W);
            keyDown=Input.GetKey(KeyCode.S);
            keyLeft=Input.GetKey(KeyCode.A);
            keyRight=Input.GetKey(KeyCode.D);
        }else if(playerNum==playerNum.Two){
            keyUp=Input.GetKey(KeyCode.UpArrow);
            keyDown=Input.GetKey(KeyCode.DownArrow);
            keyLeft=Input.GetKey(KeyCode.LeftArrow);
            keyRight=Input.GetKey(KeyCode.RightArrow);
        }
        if(keyLeft||keyRight)hMove=true;
        else hMove=false;
        if(keyUp||keyDown)vMove=true;
        else vMove=false;
        if(hMove||vMove)moving=true;
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

    public void GlitchOut(){
        var xx=Random.Range(-xRange,xRange);
        var yy=Random.Range(-yRange,yRange);

        xpos+=xx;
        ypos+=yy;
    }

    private void Die(){
        if(health<=0){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        damage=GetComponent<DamageDealer>().GetDmgPlayer();
        if(CompareTag(other.tag)){
            if(moving==true)other.GetComponent<Player>().health-=damage;
            GlitchOut();
        }else{
            if(other.gameObject.name.Contains("HealthPack")){health+=25;}
            if(other.gameObject.name.Contains("Saw")){health-=0.1f;}
        }
    }
    private void OnTriggerStay2D(Collider2D other) {
        damage=GetComponent<DamageDealer>().GetDmgPlayerStay();
        if(CompareTag(other.tag)){
            if(moving==true)other.GetComponent<Player>().health-=damage;
            GlitchOut();
        }else{
            if(other.gameObject.name.Contains("Saw")){health-=0.1f;}
        }
    }
}
