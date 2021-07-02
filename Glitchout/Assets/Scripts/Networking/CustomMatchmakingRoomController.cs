using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Photon{
public class CustomMatchmakingRoomController : MonoBehaviourPunCallbacks{
    [SerializeField] int multiplayerSceneIndex;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject roomPanel;
    [SerializeField] GameObject startButton;
    [SerializeField] Transform playersContainer;
    [SerializeField] GameObject playerListingPrefab;
    [SerializeField] TMPro.TextMeshProUGUI roomNameDisplay;
    
    void ClearPlayerListings(){
        for(int i=playersContainer.childCount-1;i>=0;i--){
            Destroy(playersContainer.GetChild(0).gameObject);
        }
    }
    void ListPlayers(){
        foreach(Player player in PhotonNetwork.PlayerList){
            GameObject tempListing=Instantiate(playerListingPrefab,playersContainer);
            TMPro.TextMeshProUGUI tempText=tempListing.transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>();
            tempText.text=player.NickName;
        }
    }
    public override void OnJoinedRoom(){
        roomPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomNameDisplay.text=PhotonNetwork.CurrentRoom.Name;
        if(PhotonNetwork.IsMasterClient){
            startButton.SetActive(true);
        }else{
            startButton.SetActive(false);
        }
        ClearPlayerListings();
        ListPlayers();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer){
        ClearPlayerListings();
        ListPlayers();
    }
    public override void OnPlayerLeftRoom(Player newPlayer){
        ClearPlayerListings();
        ListPlayers();
        if(PhotonNetwork.IsMasterClient)startButton.SetActive(true);
    }
    public void StartGame(){
        if(PhotonNetwork.IsMasterClient){
            PhotonNetwork.CurrentRoom.IsOpen=false;
            PhotonNetwork.LoadLevel(multiplayerSceneIndex);
        }
    }
    IEnumerator rejoinLobby(){
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }
    public void BackOnClick(){
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }
}
}