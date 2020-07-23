using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour{
    public static StartMenu instance;
    public static bool GameIsStarted = false;
    public GameObject startMenuUI;
    public float prevGameSpeed = 1f;

    GameSession gameSession;
    //Shop shop;
    void Start(){
        instance=this;
        gameSession = FindObjectOfType<GameSession>();
        if(startMenuUI==null){startMenuUI=transform.GetChild(0).gameObject;}
        Open();
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
        if (Input.GetKeyDown(KeyCode.Escape)){
            if(!GameIsStarted){
                Level.instance.LoadStartMenu();
            }
        }
    }
    public void StartGame(){
        startMenuUI.SetActive(false);
        //foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){player.SetActive(true);}
        //foreach(GameObject obj in GameObject.FindGameObjectsWithTag("World")){obj.SetActive(true);}
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=false;
        gameSession.gameSpeed = prevGameSpeed;
        GameIsStarted = true;
    }
    public void Open(){
        prevGameSpeed = gameSession.gameSpeed;
        startMenuUI.SetActive(true);
        //foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){player.SetActive(false);}
        //foreach(GameObject obj in GameObject.FindGameObjectsWithTag("World")){obj.SetActive(false);}
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameIsStarted = false;
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

    /*public void SetTimeLimitTxt(TMPro.TMP_InputField txt){
        var timer=GameConditions.instance.timer;
        var timerSet=GameConditions.instance.timerSet;
        //if(Mathf.RoundToInt(timer)==timerSet){
        /*float min=(float)System.Math.Truncate((timer/60));//140/60=2\3
        float sec=Mathf.Abs((timer/60)-min);//140/60=2.3 | 2.3-2=0.3
        txt.text=(System.Math.Round(((min+sec)*10),2)).ToString();/
        //txt.text=(Mathf.Round((timer/60f)*100)/100).ToString();
        //}
    }*/
    public void SetGameTimeLimit(TMPro.TMP_InputField txt){
        txt.text=System.Math.Round((float.Parse(txt.text)),2).ToString();
        float min=(float)System.Math.Truncate(float.Parse(txt.text));
        float sec=float.Parse(txt.text)-min;
        GameConditions.instance.timer=(min*60)+(sec*100); //float.Parse(txt.text);*/
    }
    public void SetScoreLimit(TMPro.TMP_InputField txt){
        GameConditions.instance.scoreLimit=int.Parse(txt.text);
    }public void SetKillsLimit(TMPro.TMP_InputField txt){
        GameConditions.instance.killsLimit=int.Parse(txt.text);
    }
    public void TimeLimitKillsChange(bool isTimeLimitKills){
        if(GameObject.Find("CheckmarkK")!=null)GameObject.Find("CheckmarkK").GetComponent<TMPro.TextMeshProUGUI>().enabled=isTimeLimitKills;
        if(GameObject.Find("CheckmarkK")!=null)GameObject.Find("CheckmarkS").GetComponent<TMPro.TextMeshProUGUI>().enabled=!isTimeLimitKills;
    }

    public void PreviousGameSpeed(){gameSession.gameSpeed = prevGameSpeed;}
}