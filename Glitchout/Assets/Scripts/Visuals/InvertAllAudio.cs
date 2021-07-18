using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InvertAllAudio : MonoBehaviour{
    public bool revertMusic;
    float offTimer;

    void Update(){
        //if(invert){
            foreach(AudioSource sound in FindObjectsOfType<AudioSource>()){
            if(sound!=null){
                GameObject snd=sound.gameObject;
                //if(sound!=MusicPlayer.instance){
                if(snd.GetComponent<MusicPlayer>()==null){//If not MusicPlayer
                    //if(sound.GetComponent<AudioSource>()!=null){
                    //var tempAudioTime=snd.GetComponent<AudioSource>().clip.length-0.025f;
                    if(revertMusic!=true){snd.GetComponent<AudioSource>().loop=true;snd.GetComponent<AudioSource>().pitch=-1;}
                    else{snd.GetComponent<AudioSource>().loop=false;}
                    //snd.GetComponent<AudioSource>().time=tempAudioTime;
                    //}
                }else{
                    if(revertMusic!=true){
                        if(MusicPlayer.instance.GetComponent<AudioSource>().pitch!=-1)MusicPlayer.instance.GetComponent<AudioSource>().pitch=-1;}
                    //else{MusicPlayer.instance=FindObjectOfType<MusicPlayer>();MusicPlayer.instance.GetComponent<AudioSource>().pitch=1;}}
                if(revertMusic==true){if(sound!=MusicPlayer.instance){sound.loop=false;sound.Stop();}MusicPlayer.instance.GetComponent<AudioSource>().pitch=1;offTimer=1f;}
                }
            }
        }
        //else{MusicPlayer.instance.GetComponent<AudioSource>().pitch=1;}
        if(FindObjectOfType<PlayerScript>()!=null){
            //if(FindObjectOfType<PlayerScript>().inverter!=true){revertMusic=true;}//this.enabled=false;}
        }else{
            revertMusic=true;
            offTimer=0;
            GetComponent<SpriteRenderer>().enabled=false;
            this.enabled=false;
        }
        if(offTimer>0)offTimer-=Time.deltaTime;
        if(offTimer<=0&&revertMusic==true)this.enabled=false;
    }
}
