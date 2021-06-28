using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour{
    public static PauseMenu instance;
    public static bool GameIsPaused = false;
    public GameObject pauseMenuUI;
    public float prevGameSpeed = 1f;

    GameSession gameSession;
    //Shop shop;
    void Start(){
        instance=this;
        gameSession = FindObjectOfType<GameSession>();
        if(pauseMenuUI==null){pauseMenuUI=transform.GetChild(0).gameObject;}
        Resume();
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if(Input.GetKeyDown(KeyCode.Escape)&&StartMenu.GameIsStarted){
            if(GameIsPaused){
                Resume();
            }else{
                //if(Shop.shopOpened!=true && UpgradeMenu.UpgradeMenuIsOpen!=true)
                Pause();
            }
        }if(GameIsPaused&&Input.GetKeyDown(KeyCode.R)){
            Level.instance.RestartGame();
        }
    }
    public void Resume(){
        pauseMenuUI.SetActive(false);
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed = prevGameSpeed;
        GameIsPaused = false;
    }
    public void Pause(){
        prevGameSpeed = gameSession.gameSpeed;
        pauseMenuUI.SetActive(true);
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameIsPaused = true;
        gameSession.speedChanged=true;
        gameSession.gameSpeed = 0f;
        
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }
    public void Menu(){
        //gameSession.gameSpeed = prevGameSpeed;
        gameSession.speedChanged = false;
        gameSession.gameSpeed = 1f;
        SceneManager.LoadScene("Menu");
    }
    public void PreviousGameSpeed(){gameSession.gameSpeed = prevGameSpeed;}
}