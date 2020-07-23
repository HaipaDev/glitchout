using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour{
    public static EndMenu instance;
    public GameObject endMenuUI;
    public float prevGameSpeed = 1f;

    GameSession gameSession;
    //Shop shop;
    void Start(){
        instance=this;
        gameSession = FindObjectOfType<GameSession>();
        if(endMenuUI==null){endMenuUI=transform.GetChild(0).gameObject;}
        //Open();
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        if(gameSession==null)gameSession = FindObjectOfType<GameSession>();
    }
    public void Open(){
        prevGameSpeed = gameSession.gameSpeed;
        endMenuUI.SetActive(true);
        StartCoroutine(ChangeWinnerTxt());
        //foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){player.SetActive(false);}
        //foreach(GameObject obj in GameObject.FindGameObjectsWithTag("World")){obj.SetActive(false);}
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        gameSession.speedChanged=true;
        gameSession.gameSpeed = 0f;
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }
    IEnumerator ChangeWinnerTxt(){
        TMPro.TextMeshProUGUI txt=null;
        yield return new WaitForSecondsRealtime(0.25f);
        if(GameObject.Find("WinnerText")!=null)txt=GameObject.Find("WinnerText").GetComponent<TMPro.TextMeshProUGUI>();
        if(txt!=null){
            if(GameConditions.instance.winningPlayer!=-1){int num=GameConditions.instance.winningPlayer+=1;txt.text=txt.text.Replace("_",num.ToString());}
            else{txt.text="DRAW!";}
            /*if(!GameConditions.instance.timeKillsEnabled){
                if(GameSession.instance.score[0]>GameSession.instance.score[1]){txt.text=txt.text.Replace("_","1");}
                else if(GameSession.instance.score[1]>GameSession.instance.score[0]){txt.text=txt.text.Replace("_","2");}
                else{txt.text="DRAW!";}
            }else{
                if(GameSession.instance.kills[0]>GameSession.instance.kills[1]){txt.text=txt.text.Replace("_","1");}
                else if(GameSession.instance.kills[1]>GameSession.instance.kills[0]){txt.text=txt.text.Replace("_","2");}
                else{txt.text="DRAW!";}
            }*/
        }
    }
    public void Menu(){
        //gameSession.gameSpeed = prevGameSpeed;
        gameSession.speedChanged = false;
        gameSession.gameSpeed = 1f;
        SceneManager.LoadScene("Menu");
    }

    public void PreviousGameSpeed(){gameSession.gameSpeed = prevGameSpeed;}
}