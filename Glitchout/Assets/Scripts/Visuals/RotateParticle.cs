using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParticle : MonoBehaviour{
    [SerializeField]bool parent;
    void Start(){
        
    }

    void Update(){
        var ps=GetComponent<ParticleSystem>().main;
        if(parent!=true)ps.startRotation=transform.rotation.z;
        if(parent==true)ps.startRotation=transform.parent.rotation.z;
    }
}
