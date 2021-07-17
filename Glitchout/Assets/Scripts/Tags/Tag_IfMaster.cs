using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Tag_IfMaster : MonoBehaviour{
    [SerializeField] bool ifNot;
    void Update(){
        if(!ifNot){if(PhotonNetwork.IsMasterClient){TurnAllChildren(true);}else{TurnAllChildren(false);}}
        else{if(PhotonNetwork.IsMasterClient){TurnAllChildren(false);}else{TurnAllChildren(true);}}
    }
    void TurnAllChildren(bool on){foreach(Transform t in transform){t.gameObject.SetActive(on);}}
}