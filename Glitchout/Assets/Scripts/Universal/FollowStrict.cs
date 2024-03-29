﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowStrict : MonoBehaviour{
    public Vector2 targetPos;
    //new Vector2 selfPos;
    [SerializeField] GameObject target;
    [SerializeField] string targetTag;
    //public float dist;
    [SerializeField] public float xx;
    [SerializeField] public float yy;
    //[SerializeField] Quaternion rotation;

    //Player player;
    public GameObject targetObj;
    //Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        if(targetObj==null){
            if(target!=null){if(GameObject.Find(target.name)!=null){targetObj = GameObject.Find(target.name);} else{targetObj=GameObject.Find(target.name+"(Clone)");} }
            else{if(targetTag!="")targetObj = GameObject.FindGameObjectWithTag(targetTag); }
        }
        //rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetObj==null){Destroy(gameObject);}
        targetPos = new Vector2(targetObj.transform.position.x+xx, targetObj.transform.position.y+yy);
        //selfPos = new Vector2(transform.position.x, transform.position.y);
        //dist=Vector2.Distance(targetPos, selfPos);
        transform.position = targetPos;
    }
}
