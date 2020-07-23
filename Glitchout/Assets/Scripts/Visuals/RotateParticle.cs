using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateParticle : MonoBehaviour{
    [SerializeField]bool parent;
    void Start(){
        
    }

    void Update(){
        if(parent!=true)GetComponent<ParticleSystem>().startRotation=transform.rotation.z;
        if(parent==true)GetComponent<ParticleSystem>().startRotation=transform.parent.rotation.z;
    }
}
