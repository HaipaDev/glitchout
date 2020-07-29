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
    [HeaderAttribute("Current Player Values")]
    public List<Player> players;
    public int[] score;
    public float[] kills;
    public float[] respawnTimer;
    [HeaderAttribute("Score Values")]
    public int score_kill=20;
    public int score_assist=15;
    public int score_death=10;
    public int score_staying=5;
    public float stayingTimeReq=4f;
    [HeaderAttribute("Settings")]
    public float respawnTime;
    [Range(0.0f, 10.0f)] public float gameSpeed = 1f;
    public bool speedChanged;
    [HeaderAttribute("Other")]
    public bool cheatmode;
    public bool dmgPopups=true;
    [HideInInspector]public bool resize;

    Player player;
    PostProcessVolume postProcessVolume;
    //public string gameVersion;
    //public bool moveByMouse = true;

    /*public SavableData savableData;
    [System.Serializable]
    public class SavableData{
        public int highscore;
        public SavableData(SavableData data)
        {
            highscore = data.highscore;
        }
        public void Save()
        {
            SaveSystem.SaveData(this);
        }
        public void Load(){
            SavableData data = SaveSystem.LoadData();
            highscore = data.highscore;
        }
    }*/

    private void Awake(){
        instance=this;
        SetUpSingleton();
    }
    private void SetUpSingleton(){
        int numberOfObj = FindObjectsOfType<GameSession>().Length;
        if(numberOfObj > 1){
            Destroy(gameObject);
        }else{
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Start()
    {
        //FindObjectOfType<SaveSerial>().highscore = 0;
    }
    private void Update()
    {
        if(SceneManager.GetActiveScene().name=="Game"&&resize==false){resize=true;}
        if(SceneManager.GetActiveScene().name=="Game"&&resize==true){
            players=FindObjectsOfType<Player>().ToList();
            Array.Resize(ref score,players.Count);
            Array.Resize(ref kills,players.Count);
            Array.Resize(ref respawnTimer,players.Count);
            for(var i=0;i<respawnTimer.Length;i++){
                if(respawnTimer[i]==0)respawnTimer[i]=-4;
            }
            resize=false;
        }else{
            players.Clear();
            Array.Resize(ref score,0);
            Array.Resize(ref kills,0);
            Array.Resize(ref respawnTimer,0);
        }
        Player[] allPlayers=FindObjectsOfType<Player>();
        foreach(Player player in allPlayers){
            players[player.playerNum]=player;
        }

        for(var i=0;i<score.Length;i++){
            score[i]=Mathf.Clamp(score[i],0,99999);
        }
        for(var r=0;r<respawnTimer.Length;r++){
            if(respawnTimer[r]>0)respawnTimer[r]-=Time.deltaTime;
            if(respawnTimer[r]<=0&&respawnTimer[r]!=-4){players[r].Respawn();respawnTimer[r]=-4;}
        }

        if(SceneManager.GetActiveScene().name=="Game"&&PauseMenu.GameIsPaused==true){gameSpeed=0;}

        Time.timeScale = gameSpeed;
        //Set speed to normal
        if(speedChanged!=true){gameSpeed=1;}
        //if(SceneManager.GetActiveScene().name!="Game"){gameSpeed=1;}
        //if(Shop.shopOpen==false&&Shop.shopOpened==false){gameSpeed=1;}
        //if(FindObjectOfType<Player>()==null){gameSpeed=1;}
        
        //Restart with R or Space/Resume with Space
        /*if((GameObject.Find("GameOverCanvas")!=null&&GameObject.Find("GameOverCanvas").activeSelf==true)&&(Input.GetKeyDown(KeyCode.Space)||Input.GetKeyDown(KeyCode.R))
        ||(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.R))){
            FindObjectOfType<Level>().RestartGame();}
        else if(PauseMenu.GameIsPaused==true&&Input.GetKeyDown(KeyCode.Space)){FindObjectOfType<PauseMenu>().Resume();}
        */

        //Postprocessing
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        //if(SaveSerial.instance.pprocessing==true && postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
        if(SaveSerial.instance.pprocessing==true && postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(SaveSerial.instance.pprocessing==false && FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}


        CheckCodes(0,0);
    }

    public void Die(int playerNum, float hitTimer){
        if(playerNum==0){
            SubToScore(playerNum,score_death);
            respawnTimer[playerNum]=respawnTime;
            if(hitTimer>0){AddToScore(playerNum+1,score_kill);kills[playerNum+1]++;}
        }else if(playerNum==1){
            SubToScore(playerNum,score_death);
            respawnTimer[playerNum]=respawnTime;
            if(hitTimer>0){AddToScore(playerNum-1,score_kill);kills[playerNum-1]++;}
        }
    }

    public void AddToScore(int i,int scoreValue){
        score[i] += scoreValue;//Mathf.RoundToInt(scoreValue*scoreMulti);
    }public void SubToScore(int i,int scoreValue){
        score[i] -= scoreValue;//Mathf.RoundToInt(scoreValue*scoreMulti);
    }

    public void MultiplyScore(int i,float multipl)
    {
        int result=Mathf.RoundToInt(score[i] * multipl);
        score[i] = result;
    }

    //public void AddToScoreNoEV(int scoreValue){score += scoreValue;ScorePopUpHUD(scoreValue);}
    //public void AddXP(float xpValue){coresXp += xpValue;coresXpTotal+=xpValue;if(xpValue>0){XPPopUpHUD(xpValue);}else if(xpValue<0){XPSubPopUpHUD(xpValue);}}
    /*public void AddEnemyCount(){enemiesCount++;FindObjectOfType<DisruptersSpawner>().EnemiesCountHealDrone++;
    var ps=FindObjectsOfType<PowerupsSpawner>();
    foreach(PowerupsSpawner p in ps){
        if(p.enemiesCountReq!=-1){
            p.enemiesCount++;
        }    
    }
    }*/

    public void ResetScore(){
        Array.Clear(score,0,score.Length);
        Array.Clear(kills,0,kills.Length);
        Array.Clear(respawnTimer,0,respawnTimer.Length);
    }
    /*
    public void SaveHighscore()
    {
        if (score > FindObjectOfType<SaveSerial>().highscore) FindObjectOfType<SaveSerial>().highscore = score;
        //FindObjectOfType<DataSavable>().highscore = highscore;
    }*/
    public void SaveSettings(){
        var ss=FindObjectOfType<SaveSerial>();
        var sm=FindObjectOfType<SettingsMenu>();
        //ss.moveByMouse = sm.moveByMouse;
        //ss.quality = sm.quality;
        ss.fullscreen = sm.fullscreen;
        ss.scbuttons = sm.scbuttons;
        ss.pprocessing = sm.pprocessing;
        ss.masterVolume = sm.masterVolume;
        ss.soundVolume = sm.soundVolume;
        ss.musicVolume = sm.musicVolume;

        ss.SaveSettings();
    }
    /*public void SaveInventory(){
        FindObjectOfType<SaveSerial>().chameleonColor[0] = FindObjectOfType<Inventory>().chameleonColorArr[0];
        FindObjectOfType<SaveSerial>().chameleonColor[1] = FindObjectOfType<Inventory>().chameleonColorArr[1];
        FindObjectOfType<SaveSerial>().chameleonColor[2] = FindObjectOfType<Inventory>().chameleonColorArr[2];
    }*/
    public void Save(){ /*FindObjectOfType<SaveSerial>().Save();*/ FindObjectOfType<SaveSerial>().SaveSettings(); }
    public void Load(){ /*FindObjectOfType<SaveSerial>().Load();*/ FindObjectOfType<SaveSerial>().LoadSettings(); }
    public void DeleteAllShowConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((false)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((true)); }
    public void DeleteAllHideConfirm(){ GameObject.Find("OptionsUI").transform.GetChild(0).gameObject.SetActive((true)); GameObject.Find("OptionsUI").transform.GetChild(1).gameObject.SetActive((false)); }
    public void DeleteAll(){ FindObjectOfType<SaveSerial>().Delete(); ResetSettings(); FindObjectOfType<Level>().Restart(); Destroy(FindObjectOfType<SaveSerial>().gameObject); Destroy(gameObject);}
    public void ResetSettings(){
        /*FindObjectOfType<SaveSerial>().ResetSettings();
        FindObjectOfType<Level>().RestartScene();
        FindObjectOfType<SaveSerial>().SaveSettings();*/

        /*var s=FindObjectOfType<SettingsMenu>();
        s.moveByMouse=true;
        s.quality=4;
        s.fullscreen=true;
        s.masterVolume=1;
        s.soundVolume=1;
        s.musicVolume=1;*/
    }public void ResetMusicPitch(){
        //if(FindObjectOfType<MusicPlayer>()!=null)FindObjectOfType<MusicPlayer>().GetComponent<AudioSource>().pitch=1;
    }
    public void CheckCodes(int fkey, int nkey){
        if(fkey==0&&nkey==0){}
        if(Input.GetKey(KeyCode.Delete) || fkey==-1){
            if(Input.GetKeyDown(KeyCode.Alpha0) || nkey==0){
                cheatmode=true;
            }if(Input.GetKeyDown(KeyCode.Alpha9) || nkey==9){
                cheatmode=false;
            }
        }
        if(cheatmode==true){
            /*if(Input.GetKey(KeyCode.F1) || fkey==1){
                player=FindObjectOfType<Player>();
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){player.health=player.maxHP;}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){player.energy=player.maxEnergy;}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){player.gclover=true;player.gcloverTimer=player.gcloverTime;}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){player.health=0;}
            }
            if(Input.GetKey(KeyCode.F2) || fkey==2){
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){AddToScoreNoEV(100);}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){AddToScoreNoEV(1000);}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){EVscore=EVscoreMax;}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){coins+=1;shopScore=shopScoreMax;}
                if(Input.GetKeyDown(KeyCode.Alpha5) || nkey==5){AddXP(100);}
                if(Input.GetKeyDown(KeyCode.Alpha6) || nkey==6){coins+=100;cores+=100;}
                if(Input.GetKeyDown(KeyCode.Alpha7) || nkey==7){FindObjectOfType<UpgradeMenu>().total_UpgradesLvl+=10;}
            }
            if(Input.GetKey(KeyCode.F3) || fkey==3){
                player=FindObjectOfType<Player>();
                if(Input.GetKeyDown(KeyCode.Alpha1) || nkey==1){player.powerup="laser3";}
                if(Input.GetKeyDown(KeyCode.Alpha2) || nkey==2){player.powerup="mlaser";}
                if(Input.GetKeyDown(KeyCode.Alpha3) || nkey==3){player.powerup="lsaber";}
                if(Input.GetKeyDown(KeyCode.Alpha4) || nkey==4){player.powerup="lclaws";}
                if(Input.GetKeyDown(KeyCode.Alpha5) || nkey==5){player.powerup="cstream";}
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
