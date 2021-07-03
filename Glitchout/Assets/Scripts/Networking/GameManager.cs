using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks{
    [SerializeField] GameObject playerPrefab;
    void Start(){
        PhotonNetwork.SerializationRate=6000;
        if(GameSession.instance.offlineMode){PhotonNetwork.OfflineMode=true;PhotonNetwork.CreateRoom("");}
        if(PhotonNetwork.IsConnectedAndReady){PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}
        if(PhotonNetwork.OfflineMode){PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}//Instantiate second player
    }
    void Update(){
        
    }
    public override void OnConnectedToMaster(){
        Debug.Log("OfflineMode: "+PhotonNetwork.OfflineMode);
    }
}
