using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundScroller : MonoBehaviour{
    [SerializeField] bool horizontal;
    [SerializeField] float bgSpeed = 0.2f;
    Material myMat;
    Vector2 offSet;
    void Start(){
        myMat=GetComponent<Renderer>().material;
        if(horizontal!=true)offSet=new Vector2(0f,bgSpeed);
        else{offSet=new Vector2(bgSpeed,0f);}
    }

    void Update(){
        if((SceneManager.GetActiveScene().name=="Game"&&GameManager.GameIsStarted&&!PauseMenu.GameIsPaused)||SceneManager.GetActiveScene().name!="Game")myMat.mainTextureOffset+=offSet*Time.deltaTime;
    }
}
