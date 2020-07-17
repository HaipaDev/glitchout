using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour{
    [SerializeField] bool horizontal;
    [SerializeField] float bgSpeed = 0.2f;
    Material myMat;
    Vector2 offSet;
    // Start is called before the first frame update
    void Start(){
        myMat = GetComponent<Renderer>().material;
        if(horizontal!=true)offSet = new Vector2(0f,bgSpeed);
        else{offSet=new Vector2(bgSpeed,0f);}
    }

    // Update is called once per frame
    void Update(){
        myMat.mainTextureOffset += offSet * Time.deltaTime;
    }
}
