using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour{
    public float animSpeed=1;
    public bool shaked;
    public float shakeStrength=1;
    public float shakeSpeed=1;
    Shake shake;
    void Start(){
        shake=FindObjectOfType<Shake>();
        GetComponent<Animator>().Play("LogoAnim2");
        GetComponent<Animator>().speed=animSpeed;
    }

    void Update(){
        if(shaked==true){
            shake.CamShake(shakeStrength,shakeSpeed);
            shaked=false;
        }
    }
}
