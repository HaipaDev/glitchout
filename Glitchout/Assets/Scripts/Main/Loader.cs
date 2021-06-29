using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class Loader : MonoBehaviour{
    public float timer=1f;
    public AudioMixer audioMixer;
    bool loaded;
    void Load(){
        if(!loaded){
        //SaveSerial.instance.Load();
        //SaveSerial.instance.LoadLogin();
        SaveSerial.instance.LoadSettings();
        loaded=true;
        }
        audioMixer.SetFloat("MasterVolume", SaveSerial.instance.settingsData.masterVolume);
        audioMixer.SetFloat("SoundVolume", SaveSerial.instance.settingsData.soundVolume);
        audioMixer.SetFloat("MusicVolume", SaveSerial.instance.settingsData.musicVolume);
    }
    void Update(){
        Load();
        timer-=Time.deltaTime;
        if(timer<=0){if(SceneManager.GetActiveScene().name=="Loading"){SceneManager.LoadScene("Menu");}Destroy(gameObject);}
    }
}
