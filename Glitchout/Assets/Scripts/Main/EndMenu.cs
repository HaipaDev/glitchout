using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenu : MonoBehaviour{
    public static EndMenu instance;
    public GameObject endMenuUI;
    public float prevGameSpeed=1f;

    //Shop shop;
    void Start(){
        instance=this;
        GameSession.instance = FindObjectOfType<GameSession>();
        if(endMenuUI==null){endMenuUI=transform.GetChild(0).gameObject;}
        //Open();
        //shop=FindObjectOfType<Shop>();
    }
    void Update(){
        if(GameSession.instance==null)GameSession.instance = FindObjectOfType<GameSession>();
    }
    public void Open(){
        prevGameSpeed = GameSession.instance.gameSpeed;
        endMenuUI.SetActive(true);
        StartCoroutine(ChangeWinnerTxt());
        //foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player")){player.SetActive(false);}
        //foreach(GameObject obj in GameObject.FindGameObjectsWithTag("World")){obj.SetActive(false);}
        //GameObject.Find("BlurImage").GetComponent<SpriteRenderer>().enabled=true;
        GameSession.instance.speedChanged=true;
        GameSession.instance.gameSpeed = 0f;
        //ParticleSystem.Stop();
        //var ptSystems = FindObjectOfType<ParticleSystem>();
        //foreach(ptSystem in ptSystems){ParticleSystem.Pause();}
    }
    IEnumerator ChangeWinnerTxt(){
        TMPro.TextMeshProUGUI txt=null;
        yield return new WaitForSecondsRealtime(0.25f);
        if(GameObject.Find("WinnerText")!=null)txt=GameObject.Find("WinnerText").GetComponent<TMPro.TextMeshProUGUI>();
        if(txt!=null){
            if(GameConditions.instance.winningPlayer!=-1){int num=GameConditions.instance.winningPlayer;txt.text=txt.text.Replace("_",num.ToString());}
            else{txt.text="DRAW!";}
        }
    }
    public void Restart(){
        GameConditions.instance.matchFinished=false;
        GameConditions.instance.wonBySKLimit=false;
    }
    public void Menu(){
        //GameSession.instance.gameSpeed=prevGameSpeed;
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
        SceneManager.LoadScene("Menu");
    }

    public void PreviousGameSpeed(){GameSession.instance.gameSpeed=prevGameSpeed;}
}