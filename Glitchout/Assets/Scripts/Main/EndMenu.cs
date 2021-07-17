using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class EndMenu : MonoBehaviour{
    public static EndMenu instance;
    [SerializeField] GameObject endMenuUI;
    [SerializeField] TMPro.TextMeshProUGUI txt;

    void Start(){
        instance=this;
        if(endMenuUI==null){endMenuUI=transform.GetChild(0).gameObject;}
    }
    public void Open(){
        if(!endMenuUI.activeSelf){
            endMenuUI.SetActive(true);
            ChangeWinnerTxt();
            AudioManager.instance.Play("Victory");
        }
    }
    void Update(){ChangeWinnerTxt();}
    public void Close(){endMenuUI.SetActive(false);}
    void ChangeWinnerTxt(){
        if(txt!=null){
            if(GameConditions.instance.winningPlayer!=-1){string value="";
                if(!string.IsNullOrEmpty(GameManager.instance.players[GameConditions.instance.winningPlayer].nick)){value=GameManager.instance.players[GameConditions.instance.winningPlayer].nick;txt.text=txt.text.Replace("_",txt.text.Split('_')[0]);}
                else{value="Player"+(GameConditions.instance.winningPlayer+1).ToString();txt.text=txt.text.Replace("_",value);}
                
            }else{txt.text="DRAW!";}
        }
    }
    public void Restart(){PhotonView.Get(GameManager.instance).RPC("RestartGame",RpcTarget.All);}
    public void Menu(){
        //GameSession.instance.gameSpeed=prevGameSpeed;
        GameSession.instance.speedChanged=false;
        GameSession.instance.gameSpeed=1f;
        SceneManager.LoadScene("Menu");
    }
}