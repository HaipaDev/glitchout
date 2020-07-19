using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour{
    [SerializeField] float rotationSpeed=25;
    void Start(){
        
    }

    void Update(){
        if(Time.timeScale>0.0001f)transform.Rotate(0,0,rotationSpeed*Time.timeScale);
    }
}
