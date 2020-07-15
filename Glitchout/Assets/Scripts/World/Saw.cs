using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour{
    [SerializeField] float rotationSpeed=25;
    void Start(){
        
    }

    void Update(){
        transform.Rotate(0,0,rotationSpeed);
    }
}
