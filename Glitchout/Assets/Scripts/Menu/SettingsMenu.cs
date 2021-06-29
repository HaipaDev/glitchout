using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour{
    //[SerializeField]GameObject qualityDropdopwn;
    [SerializeField]GameObject fullscreenToggle;
    [SerializeField]GameObject pprocessingToggle;
    [SerializeField]GameObject masterSlider;
    [SerializeField]GameObject soundSlider;
    [SerializeField]GameObject musicSlider;
    public AudioMixer audioMixer;
    [SerializeField]GameObject pprocessingPrefab;
    public PostProcessVolume postProcessVolume;
    void Start(){
        //qualityDropdopwn.GetComponent<Dropdown>().value=SaveSerial.instance.quality;
        fullscreenToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.fullscreen;
        pprocessingToggle.GetComponent<Toggle>().isOn=SaveSerial.instance.settingsData.pprocessing;

        masterSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.masterVolume;
        soundSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.soundVolume;
        musicSlider.GetComponent<Slider>().value=SaveSerial.instance.settingsData.musicVolume;
    }
    void Update(){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null){
            if(SaveSerial.instance.settingsData.pprocessing==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}
            if(SaveSerial.instance.settingsData.pprocessing==true&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume.enabled=true;}
            if(SaveSerial.instance.settingsData.pprocessing==false&&FindObjectOfType<PostProcessVolume>()!=null){postProcessVolume=FindObjectOfType<PostProcessVolume>();postProcessVolume.enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
            if(SaveSerial.instance.settingsData.masterVolume<=-40){SaveSerial.instance.settingsData.masterVolume=-80;}
            if(SaveSerial.instance.settingsData.soundVolume<=-40){SaveSerial.instance.settingsData.soundVolume=-80;}
            if(SaveSerial.instance.settingsData.musicVolume<=-40){SaveSerial.instance.settingsData.musicVolume=-80;}
        }
    }
    public void BackButton(){
        SaveSerial.instance.SaveSettings();
        if(SceneManager.GetActiveScene().name=="Game"){PauseMenu.instance.PauseOpen();}
        else{Level.instance.LoadStartMenu();}
    }
    public void SetMasterVolume(float volume){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.masterVolume=volume;
    }public void SetSoundVolume(float volume){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.soundVolume=volume;
    }
    public void SetMusicVolume(float volume){
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.musicVolume=volume;
    }
    /*public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        if(SaveSerial.instance!=null){
            SaveSerial.instance.settingsData.quality=qualityIndex;
            if(qualityIndex<=1){
                SaveSerial.instance.settingsData.pprocessing=false;
                SaveSerial.instance.settingsData.dmgPopups=false;
                pprocessingToggle.GetComponent<Toggle>().isOn=false;
                dmgPopupsToggle.GetComponent<Toggle>().isOn=false;
            }if(qualityIndex==0){
                SaveSerial.instance.settingsData.screenshake=false;
                SaveSerial.instance.settingsData.particles=false;
                SaveSerial.instance.settingsData.screenflash=false;
                screenshakeToggle.GetComponent<Toggle>().isOn=false;
                particlesToggle.GetComponent<Toggle>().isOn=false;
                screenflashToggle.GetComponent<Toggle>().isOn=false;
            }if(qualityIndex>1){
                SaveSerial.instance.settingsData.particles=true;
                particlesToggle.GetComponent<Toggle>().isOn=true;
            }if(qualityIndex>4){
                SaveSerial.instance.settingsData.pprocessing=true;
                pprocessingToggle.GetComponent<Toggle>().isOn=true;
            }
        }
    }*/
    public void SetFullscreen(bool isOn){
        Screen.fullScreen=isOn;
        if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.fullscreen=isOn;
        Screen.SetResolution(Display.main.systemWidth,Display.main.systemHeight,isOn,60);
    }
    public void SetPostProcessing(bool isOn){
        postProcessVolume=FindObjectOfType<PostProcessVolume>();
        if(SaveSerial.instance!=null)if(SaveSerial.instance!=null)SaveSerial.instance.settingsData.pprocessing=isOn;
        if(isOn==true&&postProcessVolume==null){postProcessVolume=Instantiate(pprocessingPrefab,Camera.main.transform).GetComponent<PostProcessVolume>();}//FindObjectOfType<Level>().RestartScene();}
        if(isOn==true&&postProcessVolume!=null){postProcessVolume.enabled=true;}
        if(isOn==false&&FindObjectOfType<PostProcessVolume>()!=null){FindObjectOfType<PostProcessVolume>().enabled=false;}//Destroy(FindObjectOfType<PostProcessVolume>());}
    }
    public void PlayDing(){if(Application.isPlaying)GetComponent<AudioSource>().Play();}
}
