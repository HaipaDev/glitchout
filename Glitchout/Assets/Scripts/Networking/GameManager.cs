using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks{
    [SerializeField] GameObject playerPrefab;
    void Start(){
        PhotonNetwork.SerializationRate=6000;//60/s
        if(GameSession.instance.offlineMode){PhotonNetwork.OfflineMode=true;PhotonNetwork.CreateRoom("");}
        if(PhotonNetwork.IsConnectedAndReady){var go=PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}//go.name="Player"+go.GetComponent<PlayerScript>().playerNum;}
        if(PhotonNetwork.OfflineMode){var go=PhotonNetwork.Instantiate(playerPrefab.name,Vector2.zero,Quaternion.identity);}//Instantiate second player on OfflineMode
    }
    void Update(){
        
    }
    public override void OnConnectedToMaster(){
        Debug.Log("OfflineMode: "+PhotonNetwork.OfflineMode);
    }
}
