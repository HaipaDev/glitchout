using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomMatchmakingLobbyController : MonoBehaviourPunCallbacks{
    [SerializeField] GameObject lobbyConnectButton;
    [SerializeField] GameObject lobbyPanel;
    [SerializeField] GameObject mainPanel;
    [SerializeField] TMPro.TMP_InputField  playerNameInput;
    string roomName;
    [SerializeField] int roomSize;
    List<RoomInfo> roomListings;
    [SerializeField] Transform roomsContainer;
    [SerializeField] GameObject roomListingPrefab;
    public override void OnConnectedToMaster(){
        PhotonNetwork.AutomaticallySyncScene=true;
        lobbyConnectButton.SetActive(true);
        roomListings=new List<RoomInfo>();

        if(PlayerPrefs.HasKey("NickName")){
            if(PlayerPrefs.GetString("NickName")==""){
                PhotonNetwork.NickName="Player"+Random.Range(0,1000);
            }else{
                PhotonNetwork.NickName=PlayerPrefs.GetString("NickName");
            }
        }else{
            PhotonNetwork.NickName="Player"+Random.Range(0,1000);
        }
        playerNameInput.text=PhotonNetwork.NickName;
    }
    public void PlayerNameUpdate(string nameInput){
        PhotonNetwork.NickName=nameInput;
        PlayerPrefs.SetString("NickName",nameInput);
    }
    public void JoinLobbyOnClick(){
        mainPanel.SetActive(false);
        lobbyPanel.SetActive(true);
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
                ListRoom(room);
            }
        }
    }
    static System.Predicate<RoomInfo> ByName(string name){
        return delegate(RoomInfo room){
            return room.Name==name;
        };
    }
    void ListRoom(RoomInfo room){
        if(room.IsOpen&&room.IsVisible){
            GameObject tempListing=Instantiate(roomListingPrefab,roomsContainer);
            RoomButton tempButton = tempListing.GetComponent<RoomButton>();
            tempButton.SetRoom(room.Name,room.MaxPlayers,room.PlayerCount);
        }
    }
    public void OnRoomNameChanged(string nameIn){
        roomName=nameIn;
    }
    public void OnRoomSizeChanged(string sizeIn){
        roomSize=int.Parse(sizeIn);
    }
    public void CreateRoom(){
        Debug.Log("Creating room now");
        RoomOptions roomOps=new RoomOptions(){IsVisible=true, IsOpen=true, MaxPlayers=(byte)roomSize};
        PhotonNetwork.CreateRoom(roomName,roomOps);
    }
    public override void OnCreateRoomFailed(short returnCode,string message){
        Debug.Log("Tried to create a new room, but failed, there probably exists a room with the same name");
    }
    public void MatchMakingCancel(){
        mainPanel.SetActive(true);
        lobbyPanel.SetActive(false);
        PhotonNetwork.LeaveLobby();
    }
}
