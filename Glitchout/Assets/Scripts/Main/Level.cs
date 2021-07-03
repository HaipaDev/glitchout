using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour{
    public static Level instance;
    [SerializeField]ParticleSystem transition;
    [SerializeField]Animator transitioner;
    [SerializeField]float transitionTime=0.35f;
    //float prevGameSpeed;
    private void Awake(){
        if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}
        GameSession.instance.gameSpeed=1f;
    }
    void Start(){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //prevGameSpeed = GameSession.instance.gameSpeed;
    }
    void Update(){
        CheckESC();
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
    }

    public void LoadStartMenu(){
        /*GameSession.instance.SaveHighscore();
        GameSession.instance.ResetScore();
        FindObjectOfType<SaveSerial>().Save();
        GameSession.instance.ResetMusicPitch();*/
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
        SceneManager.LoadScene("Menu");
        //LoadLevel("Menu");
    }
    public void LoadGameScene(){
        GameSession.instance.offlineMode=true;
        SceneManager.LoadScene("Game");
        GameSession.instance.resize=true;
        GameSession.instance.ResetScore();
        GameSession.instance.gameSpeed=1f;
    }
    public void LoadOnlineScene(){GameSession.instance.offlineMode=false;SceneManager.LoadScene("OnlineMatchmaking");}
    public void LoadOptionsScene(){SceneManager.LoadScene("Options");}
    public void LoadInventoryScene(){SceneManager.LoadScene("Inventory");}
    public void RestartGame(){
        //PauseMenu.GameIsPaused=false;
        /*GameSession.instance.SaveHighscore();
        GameSession.instance.ResetMusicPitch();*/
        GameSession.instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.gameSpeed=1f;
    }public void RestartScene(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GameSession.instance.gameSpeed=1f;
    }
    public void QuitGame(){
        Application.Quit();
    }
    public void Restart(){
        SceneManager.LoadScene("Loading");
        GameSession.instance.gameSpeed=1f;
    }

    void CheckESC(){
    if(Input.GetKeyDown(KeyCode.Escape)){
        var scene=SceneManager.GetActiveScene().name;
        if(scene=="Options"){
            LoadStartMenu();
        }
        if(scene=="OnlineMatchmaking"){
            Photon.Pun.PhotonNetwork.Disconnect();
            LoadStartMenu();
        }
    }}

    void LoadLevel(string sceneName){
        //StartCoroutine(LoadTransition(sceneName));
        LoadTransition(sceneName);
    }
    void LoadTransition(string sceneName){
        //transition=FindObjectOfType<Tag_Transition>().GetComponent<ParticleSystem>();
        //transitioner=FindObjectOfType<Tag_Transition>().GetComponent<Animator>();
        
        //transition.Play();
        transitioner.SetTrigger("Start");

        //yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneName);
    }

    public void OpenURL(string url){Application.OpenURL(url);}
}
