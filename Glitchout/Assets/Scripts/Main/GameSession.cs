using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;
public class GameSession : MonoBehaviour{
    public static GameSession instance;
    [HeaderAttribute("Current PlayerScript Values")]
    public PlayerSession[] players;
    [HeaderAttribute("Score Values")]
    public int score_kill=20;
    public int score_assist=15;
    public int score_death=10;
    public int score_staying=5;
    public float stayingTimeReq=4f;
    [HeaderAttribute("Settings")]
    public float respawnTime;
    [Range(0.0f, 10.0f)] public float gameSpeed=1f;
    public bool speedChanged;
    [HeaderAttribute("Other")]
    public bool offlineMode=true;
    public bool cheatmode;
    public bool dmgPopups=true;
    [HideInInspector]public bool resize;

    PlayerScript PlayerScript;
    PostProcessVolume postProcessVolume;
    //public string gameVersion;

    void Awake(){if(instance!=null){Destroy(gameObject);}else{instance=this;DontDestroyOnLoad(gameObject);}}

    void Start(){
        
    }
    void Update(){
        if(SceneManager.GetActiveScene().name=="Game"&&resize==false){resize=true;}
        if(SceneManager.GetActiveScene().name=="Game"&&resize==true){
            PlayerScript[] allPlayers=FindObjectsOfType<PlayerScript>();
            Array.Resize(ref players,allPlayers.Length);
            foreach(PlayerScript player in allPlayers){players[player.playerNum].playerScript=player;}

            for(var i=0;i<players.Length;i++){
                if(players[i].respawnTimer==0)players[i].respawnTimer=-4;
            }
            resize=false;
        }else{
            Array.Resize(ref players,0);
        }

        for(var i=0;i<players.Length;i++){
            players[i].score=Mathf.Clamp(players[i].score,0,99999);
            if(players[i].respawnTimer>0)players[i].respawnTimer-=Time.deltaTime;
            if(players[i].respawnTimer<=0&&players[i].respawnTimer!=-4){players[i].playerScript.Respawn();players[i].respawnTimer=-4;}
        }

        if(SceneManager.GetActiveScene().name=="Game"&&PauseMenu.GameIsPaused==true){gameSpeed=0;}

        Time.timeScale=gameSpeed;
        //Set speed to normal
        if(speedChanged!=true){gameSpeed=1;}
        //if(SceneManager.GetActiveScene().name!="Game"){gameSpeed=1;}
        //if(Shop.shopOpen==false&&Shop.shopOpened==false){gameSpeed=1;}
        //if(FindObjectOfType<PlayerScript>()==null){gameSpeed=1;}
        
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

    public void Die(int playerNum, float hitTimer){
        if(playerNum==0){
            AddSubScore(playerNum,score_death,false);
            players[playerNum].respawnTimer=respawnTime;
            if(hitTimer>0){AddSubScore(playerNum+1,score_kill);players[playerNum+1].kills++;}
        }else if(playerNum==1){
            AddSubScore(playerNum,score_death,false);
            players[playerNum].respawnTimer=respawnTime;
            if(hitTimer>0){AddSubScore(playerNum-1,score_kill);players[playerNum-1].kills++;}
        }
    }

    public void AddSubScore(int i,int scoreValue,bool add=true){
        if(add)players[i].score+=scoreValue;//Mathf.RoundToInt(scoreValue*scoreMulti);
        else players[i].score-=scoreValue;//Mathf.RoundToInt(scoreValue*scoreMulti);
    }

    public void MultiplyScore(int i,float multipl){
        int result=Mathf.RoundToInt(players[i].score*multipl);
        players[i].score=result;
    }

    public void ResetScore(){
        Array.Clear(players,0,players.Length);
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
    public void ResetMusicPitch(){
        if(FindObjectOfType<MusicPlayer>()!=null)FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().pitch=1;
    }
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
}

[System.Serializable]
public class PlayerSession{
    public PlayerScript playerScript;
    public int score;
    public int kills;
    public float respawnTimer;
}