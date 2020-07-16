using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum dmgType{
    normal,
    silent,
    flame,
    shadow,
    decay,
    electr,
    heal
}
public class DamageDealer : MonoBehaviour{
    //public static DamageDealer instance;
    float dmg = 5;
    float dmgZone = 6;
    float dmgPlayer = 5;
    float dmgPlayerStay = 1;
    float dmgLaser = 2;
    float dmgSaw = 0.1f;

    public float GetDmg(){return dmg;}
    public float GetDmgZone(){return dmgZone;}

    public float GetDmgPlayer(){return dmgPlayer;}
    public float GetDmgPlayerStay(){return dmgPlayerStay;}
    public float GetDmgLaser(){return dmgLaser;}
    public float GetDmgSaw(){return dmgSaw;}
}
