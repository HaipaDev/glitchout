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
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1;
    }
    void Update(){
        if(Input.GetKeyDown(KeyCode.Escape)&&(!PhotonNetwork.OfflineMode||(GameSession.instance.offlineMode&&GameManager.instance.GameIsStarted))){
            if(GameIsPaused&&!optionsUI.activeSelf){
                Resume();
            }else if(GameIsPaused&&optionsUI.activeSelf){
                PauseOpen();
            }else{
                Pause();
            }
        }if(GameIsPaused&&Input.GetKeyDown(KeyCode.R)){
            Level.instance.RestartGame();
        }
    }
    [PunRPC]
    public void Resume(){
        transform.GetChild(0).gameObject.SetActive(false);
        PauseClose();
        GameIsPaused=false;
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        if(PhotonNetwork.IsMasterClient){
            GameSession.instance.speedChanged=false;
            GameSession.instance.gameSpeed=prevGameSpeed;
        }
    }
    [PunRPC]
    public void Pause(){
        transform.GetChild(0).gameObject.SetActive(true);
        PauseOpen();
        GameIsPaused=true;
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        if(PhotonNetwork.IsMasterClient){
            prevGameSpeed=GameSession.instance.gameSpeed;
            GameSession.instance.speedChanged=true;
            GameSession.instance.gameSpeed=0f;
        }
    }
    public void Menu(){
        if(!PhotonNetwork.OfflineMode){PhotonNetwork.LeaveRoom();PhotonNetwork.LeaveLobby();}
        PreviousGameSpeed();
        Level.instance.LoadStartMenu();
    }
    public void PauseOpen(){
        pauseMenuUI.SetActive(true);
        optionsUI.SetActive(false);
    }
    public void Options(){
        optionsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    public void PauseClose(){
        optionsUI.SetActive(true);
        pauseMenuUI.SetActive(false);
    }
    
    public void PreviousGameSpeed(){GameSession.instance.gameSpeed=prevGameSpeed;}
}