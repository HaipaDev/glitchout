using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class HealthPack : MonoBehaviour, IPunObservable{
    public float timerMax=5f;
    public float timer;
    public bool played;
    void Start(){
        timer=timerMax;
    }
    void Update(){
        if(timer>0){
            timer-=Time.deltaTime;
            GetComponentInChildren<Image>().fillAmount=1/timer;
            played=false;
        }else {GetComponentInChildren<Image>().fillAmount=1;if(played==false){GetComponent<AudioSource>().Play();played=true;}}
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
        if(stream.IsWriting){// We own this player: send the others our data
            stream.SendNext(timer);
        }
        else{// Network player, receive data
            this.timer=(float)stream.ReceiveNext();
        }
    }
}
