﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;
using UnityEngine.Audio;
//using UnityEngine.Rendering.PostProcessing;

public class SettingsMenu : MonoBehaviour{
    //Settings settings;
    GameSession gameSession;
    SaveSerial saveSerial;
    //public int quality;
    public bool fullscreen;
    //public bool pprocessing;
    public bool scbuttons;
    public float masterVolume;
    public float soundVolume;
    public float musicVolume;
    //[SerializeField]GameObject qualityDropdopwn;
    [SerializeField]GameObject fullscreenToggle;
    //[SerializeField]GameObject pprocessingToggle;
    [SerializeField]GameObject scbuttonsToggle;
    //[SerializeField]GameObject steeringToggle;
    [SerializeField]GameObject masterSlider;
    [SerializeField]GameObject soundSlider;
    [SerializeField]GameObject musicSlider;
    [SerializeField]AudioSource audioSource;
    public AudioMixer audioMixer;
    //[SerializeField]GameObject pprocessingPrefab;
    private void Start(){
        //settings = FindObjectOfType<Settings>();
        gameSession = FindObjectOfType<GameSession>();
        saveSerial = FindObjectOfType<SaveSerial>();
        audioSource = GetComponent<AudioSource>();
        
        /*quality = qualityDropdopwn.GetComponent<Dropdown>().value;
        fullscreen = fullscreenToggle.GetComponent<Toggle>().isOn;
        moveByMouse = steeringToggle.GetComponent<Toggle>().isOn;*/

        //qualityDropdopwn.GetComponent<Dropdown>().value = saveSerial.quality;
        fullscreenToggle.GetComponent<Toggle>().isOn = saveSerial.fullscreen;
        //pprocessingToggle.GetComponent<Toggle>().isOn = saveSerial.pprocessing;
        scbuttonsToggle.GetComponent<Toggle>().isOn = saveSerial.scbuttons;

        masterSlider.GetComponent<Slider>().value = saveSerial.masterVolume;
        soundSlider.GetComponent<Slider>().value = saveSerial.soundVolume;
        musicSlider.GetComponent<Slider>().value = saveSerial.musicVolume;
    }
    private void Update() {
        //if(pprocessing==true && FindObjectOfType<PostProcessVolume>()==null){Instantiate(pprocessingPrefab,Camera.main.transform);}
        //if(pprocessing==false && FindObjectOfType<PostProcessVolume>()!=null){Destroy(FindObjectOfType<PostProcessVolume>());}
    }
    public void SetMasterVolume(float volume){
        audioMixer.SetFloat("MasterVolume", volume);
        masterVolume = volume;
    }public void SetSoundVolume(float volume){
        audioMixer.SetFloat("SoundVolume", volume);
        soundVolume = volume;
    }
    public void SetMusicVolume(float volume){
        audioMixer.SetFloat("MusicVolume", volume);
        musicVolume = volume;
    }
    /*public void SetQuality(int qualityIndex){
        QualitySettings.SetQualityLevel(qualityIndex);
        quality = qualityIndex;
    }*/
    public void SetFullscreen (bool isFullscreen){
        Screen.fullScreen = isFullscreen;
        fullscreen = isFullscreen;
    }/*public void SetPostProcessing (bool isPostprocessed){
        //gameSession.pprocessing = isPostprocessed;
        pprocessing = isPostprocessed;
        if(isPostprocessed==true && FindObjectOfType<PostProcessVolume>()==null){Instantiate(pprocessingPrefab,Camera.main.transform);}//FindObjectOfType<Level>().RestartScene();}
        if(isPostprocessed==false && FindObjectOfType<PostProcessVolume>()!=null){Destroy(FindObjectOfType<PostProcessVolume>());}
    }*/public void SetOnScreenButtons (bool onscbuttons){
        scbuttons = onscbuttons;
        if(onscbuttons){Debug.Log(scbuttons);scbuttons=true;}
    }
    public void PlayDing(){
        audioSource.Play();
    }
}
