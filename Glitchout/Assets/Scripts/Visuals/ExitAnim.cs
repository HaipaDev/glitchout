using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitAnim : MonoBehaviour{
    [SerializeField]float exitTimer=-4;
    void Update(){
        if(exitTimer<=0&&exitTimer!=-4){Level.instance.QuitGame();}
        if(exitTimer>0){MusicPlayer.instance.GetComponent<AudioSource>().volume=(exitTimer/99);}
    }
    public void QuitGameAnim(){GetComponent<Animator>().Play("ExitAnim");AudioManager.instance.Play("TVOff");}
}