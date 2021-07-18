using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
using Photon.Pun;
using Photon.Realtime;
public class GameSession : MonoBehaviour{
    public static GameSession instance;
    [HeaderAttribute("Game Values")]
    public int score_kill=20;
    public int score_assist=15;
    public int score_death=10;
    public int score_staying=5;
    public float stayingTimeReq=4f;
    public float respawnTime=3f;
    [HeaderAttribute("Settings")]
    [Range(0.0f, 10.0f)] public float gameSpeed=1f;
    public bool speedChanged;
    [HeaderAttribute("Other")]
    public bool offlineMode=true;
    public bool cheatmode;
    public bool dmgPopups=true;
    public bool resize;

    PostProcessVolume postProcessVolume;
    //public string gameVersion;

    void Awake(){if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}
    void Update(){
        if(SceneManager.GetActiveScene().name!="Game")Time.timeScale=gameSpeed;
        else Time.timeScale=GameManager.instance.gameSpeed;
        if(speedChanged!=true){gameSpeed=1;}
        //if(SceneManager.GetActiveScene().name=="Game"&&PauseMenu.GameIsPaused==true){gameSpeed=0;}
        
        
        //Restart with R or Space/Resume with Space
        /*if(SceneManager.GetActiveScene().name=="Game"){
        if((GameOverCanvas.instance==null||GameOverCanvas.instance.gameOver==false)&&PauseMenu.GameIsPaused==false){restartTimer=-4;}
        if(PauseMenu.GameIsPaused==true){if(restartTimer==-4)restartTimer=0.5f;}
        if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true){if(restartTimer==-4)restartTimer=1f;}
        else if(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.Space)){FindObjectOfType<PauseMenu>().Resume();}
        if(restartTimer>0)restartTimer-=Time.unscaledDeltaTime;
        if(restartTimer<=0&&restartTimer!=-4){if(Input.GetKeyDown(KeyCode.R)||(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Space))){Level.instance.RestartGame();restartTimer=-4;}}
        if(GameOverCanvas.instance!=null&&GameOverCanvas.instance.gameOver==true&&Input.GetKeyDown(KeyCode.Escape)){Level.instance.LoadStartMenu();}
        }*/

        //Postprocessing
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        //if(SaveSerial.instance.pprocessing==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(SaveSerial.instance.settingsData.pprocessing==true && postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(SaveSerial.instance.settingsData.pprocessing==false && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}


        CheckCodes(".",".");
    }
    public void SaveSettings(){SaveSerial.instance.SaveSettings();}
    public void Save(){ /*SaveSerial.instance.Save();*/ SaveSerial.instance.SaveSettings(); }
    public void Load(){ /*SaveSerial.instance.Load();*/ SaveSerial.instance.LoadSettings(); }
    public void DeleteAllShowConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((false)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((true)); }
    public void DeleteAllHideConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((true)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((false)); }
    public void DeleteAll(){ResetSettings();Level.instance.LoadStartMenu();}
    public void ResetSettings(){
        SaveSerial.instance.ResetSettings();
        Level.instance.RestartScene();
        SaveSerial.instance.SaveSettings();
        var s=FindObjectOfType<SettingsMenu>();
    }
    public void ResetMusicPitch(){MusicPlayer.instance.GetComponent<AudioSource>().pitch=1;} 
    #region
    public void CheckCodes(string fkey, string nkey){
        //if(fkey=="0"&&nkey=="0"){}
        if(Input.GetKey(KeyCode.Delete) || fkey=="Del"){
            if(Input.GetKeyDown(KeyCode.Alpha0) || nkey=="0"){
                cheatmode=true;
            }if(Input.GetKeyDown(KeyCode.Alpha9) || nkey=="9"){
                cheatmode=false;
            }
        }
        if(cheatmode==true){
            /*if(Input.GetKey(KeyCode.Alpha1) || fkey=="1"){
                PlayerScript=PlayerScript.instance;
                if(Input.GetKeyDown(KeyCode.Q) || nkey=="Q"){PlayerScript.health=PlayerScript.maxHP;}
                if(Input.GetKeyDown(KeyCode.W) || nkey=="W"){PlayerScript.energy=PlayerScript.maxEnergy;}
                if(Input.GetKeyDown(KeyCode.E) || nkey=="E"){PlayerScript.gclover=true;PlayerScript.gcloverTimer=PlayerScript.gcloverTime;}
                if(Input.GetKeyDown(KeyCode.R) || nkey=="R"){PlayerScript.health=0;}
            }
            if(Input.GetKey(KeyCode.Alpha2) || fkey=="2"){
                if(Input.GetKeyDown(KeyCode.Q) || nkey=="Q"){AddToScoreNoEV(100);}
                if(Input.GetKeyDown(KeyCode.W) || nkey=="W"){AddToScoreNoEV(1000);}
                if(Input.GetKeyDown(KeyCode.E) || nkey=="E"){EVscore=EVscoreMax;}
                if(Input.GetKeyDown(KeyCode.R) || nkey=="R"){coins+=1;shopScore=shopScoreMax;}
                if(Input.GetKeyDown(KeyCode.T) || nkey=="T"){AddXP(100);}
                if(Input.GetKeyDown(KeyCode.Y) || nkey=="Y"){coins+=100;cores+=100;}
                if(Input.GetKeyDown(KeyCode.U) || nkey=="U"){FindObjectOfType<UpgradeMenu>().total_UpgradesLvl+=10;}
                if(Input.GetKeyDown(KeyCode.I) || nkey=="I"){foreach(PowerupsSpawner ps in FindObjectsOfType<PowerupsSpawner>())ps.timer=0.01f;}
                if(Input.GetKeyDown(KeyCode.O) || nkey=="O"){foreach(PowerupsSpawner ps in FindObjectsOfType<PowerupsSpawner>())ps.enemiesCount=100;}
            }
            if(Input.GetKey(KeyCode.Alpha3) || fkey==""){
                PlayerScript=PlayerScript.instance;
                if(Input.GetKeyDown(KeyCode.Q) || nkey=="Q"){PlayerScript.powerup="laser3";}
                if(Input.GetKeyDown(KeyCode.W) || nkey=="W"){PlayerScript.powerup="mlaser";}
                if(Input.GetKeyDown(KeyCode.E) || nkey=="E"){PlayerScript.powerup="lsaber";}
                if(Input.GetKeyDown(KeyCode.R) || nkey=="R"){PlayerScript.powerup="cstream";}
                if(Input.GetKeyDown(KeyCode.T) || nkey=="T"){PlayerScript.powerup="plaser";}
            }*/
        }
    }

    /*void ScorePopUpHUD(float score){
        GameObject scpopupHud=GameObject.Find("ScoreDiffParrent");
        scpopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //scpupupHud.GetComponent<Animator>().SetTrigger(0);
        scpopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+score.ToString();
    }void XPPopUpHUD(float xp){
        GameObject xppopupHud=GameObject.Find("XPDiffParrent");
        xppopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //xppopupHud.GetComponent<Animator>().SetTrigger(0);
        xppopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="+"+xp.ToString();
    }void XPSubPopUpHUD(float xp){
        GameObject xppopupHud=GameObject.Find("XPDiffParrent");
        xppopupHud.GetComponent<AnimationOn>().AnimationSet(true);
        //xppopupHud.GetComponent<Animator>().SetTrigger(0);
        xppopupHud.GetComponentInChildren<TMPro.TextMeshProUGUI>().text="-"+Mathf.Abs(xp).ToString();
    }*/
    //public void PlayDenySFX(){AudioManager.instance.Play("Deny");}
    #endregion
}