//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class NetworkController : MonoBehaviourPunCallbacks{
    [Header("Lobby")]
    [SerializeField] GameObject loginPanel;
    [SerializeField] TMPro.TMP_InputField  playerNameInput;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject lobbyConnectButton;
    [SerializeField] TMPro.TMP_InputField  roomNameInput;
    public string roomName;
    [SerializeField] int roomSize;
    List<RoomInfo> roomListings;
    [SerializeField] Transform roomsContainer;
    [SerializeField] GameObject roomListingPrefab;
    [Header("Room")]
    [SerializeField] GameObject roomPanel;
    [SerializeField] Transform playersContainer;
    [SerializeField] GameObject playerListingPrefab;
    [SerializeField] TMPro.TextMeshProUGUI roomNameDisplay;
    [SerializeField] GameObject startButton;

    void Start(){
        loginPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(false);
        lobbyConnectButton.SetActive(false);
        if(PlayerPrefs.HasKey("NickName")){
            if(!string.IsNullOrEmpty(PlayerPrefs.GetString("NickName"))){
               playerNameInput.text=PlayerPrefs.GetString("NickName");
            }
        }
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster(){
        Debug.Log("We are now connected to the "+PhotonNetwork.CloudRegion+" server!");
        PhotonNetwork.AutomaticallySyncScene=true;
        lobbyConnectButton.SetActive(true);
        roomListings=new List<RoomInfo>();
        PhotonNetwork.NickName=playerNameInput.text;
    }
    
    public void JoinLobby(){
        loginPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        if(!string.IsNullOrEmpty(playerNameInput.text))PhotonNetwork.NickName="Player"+Random.Range(0,1000);
        else PlayerPrefs.SetString("NickName","Player"+Random.Range(0,1000));
        PhotonNetwork.JoinLobby();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList){
        int tempIndex;
        foreach(RoomInfo room in roomList){
            if(roomListings!=null){
                tempIndex=roomListings.FindIndex(ByName(room.Name));
            }else{
                tempIndex=-1;
            }
            if(tempIndex!=-1){
                roomListings.RemoveAt(tempIndex);
            }
            if(room.PlayerCount>0){
                roomListings.Add(room);
                ListRooms(room);
            }
        }
    }
    void ListRooms(RoomInfo room){
        if(room.IsOpen&&room.IsVisible){
            GameObject tempListing=Instantiate(roomListingPrefab,roomsContainer);
            RoomButton tempButton=tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name,room.MaxPlayers,room.PlayerCount);
        }
    }
    public void PlayerNameUpdate(string nameInput){PhotonNetwork.NickName=nameInput;PlayerPrefs.SetString("NickName",nameInput);}
    public void OnRoomNameChanged(string nameIn){roomName=nameIn;}
    public void OnRoomSizeChanged(string sizeIn){roomSize=int.Parse(sizeIn);}
    public void CreateRoom(){
        RoomOptions roomOps=new RoomOptions(){IsVisible=true, IsOpen=true, MaxPlayers=(byte)roomSize};
        if(!string.IsNullOrEmpty(roomNameInput.text))roomName=roomNameInput.text;
        else roomName="Room"+Random.Range(0,1000);
        PhotonNetwork.CreateRoom(roomName,roomOps);
        Debug.Log("Creating room "+roomName);
    }
    public override void OnCreateRoomFailed(short returnCode,string message){
        Debug.Log("Tried to create a new room, but failed, there probably exists a room with the same name");
    }
    public void LeaveLobby(){
        loginPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
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
        if(PhotonNetwork.IsMasterClient)startButton.SetActive(true);
        else startButton.SetActive(false);
        ClearPlayerListings();
        ListPlayers();
    }
    public override void OnPlayerEnteredRoom(Player newPlayer){
        Debug.Log(newPlayer.NickName+" just joined"+PhotonNetwork.CurrentRoom.Name);
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
            PhotonNetwork.LoadLevel("Game");
        }
    }
    IEnumerator rejoinLobby(){
        yield return new WaitForSeconds(1);
        PhotonNetwork.JoinLobby();
    }
    public void LeaveRoom(){
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LeaveLobby();
        StartCoroutine(rejoinLobby());
    }


    static System.Predicate<RoomInfo> ByName(string name){return delegate(RoomInfo room){return room.Name==name;};}
}
