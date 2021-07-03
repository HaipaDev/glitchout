using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class PauseMenu : MonoBehaviourPunCallbacks{
    public static PauseMenu instance;
    public static bool GameIsPaused=false;
    public GameObject pauseMenuUI;
    public GameObject optionsUI;
    [HideInInspector]public float prevGameSpeed=1f;

    //Shop shop;
    void Start(){
        instance=this;
        Resume();
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)&&StartMenu.GameIsStarted){
            if(GameIsPaused&&!optionsUI.activeSelf){
                Resume();
            }else if(optionsUI.activeSelf){
                PauseOpen();
            }else{
                Pause();
            }
        }if(GameIsPaused&&Input.GetKeyDown(KeyCode.R)){
            Level.instance.RestartGame();
        }
    }
    public void Resume(){
        transform.GetChild(0).gameObject.SetActive(false);
        optionsUI.SetActive(false);
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        GameSession.instance.gameSpeed=prevGameSpeed;
        GameIsPaused=false;
    }
    [PunRPC]
    public void Pause(){
        if(PhotonNetwork.IsMasterClient){
            prevGameSpeed=GameSession.instance.gameSpeed;
            transform.GetChild(0).gameObject.SetActive(true);
            PauseOpen();
            //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
            GameIsPaused=true;
            GameSession.instance.speedChanged=true;
            GameSession.instance.gameSpeed=0f;
        }
    }
    public void PauseOpen(){
        pauseMenuUI.SetActive(true);
        optionsUI.SetActive(false);
    }
    public void Options(){
        optionsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    
    public void PreviousGameSpeed(){GameSession.instance.gameSpeed=prevGameSpeed;}
}