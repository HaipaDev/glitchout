using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomButton : MonoBehaviour{
    [SerializeField] TMPro.TextMeshProUGUI nameText;
    [SerializeField] TMPro.TextMeshProUGUI sizeText;
    string roomName;
    int roomSize;
    int playerCount;
    public void JoinRoomOnClick(){
        PhotonNetwork.JoinRoom(roomName);
    }
    public void SetRoom(string nameInput, int sizeInput, int countInput){
        roomName=nameInput;
        roomSize=sizeInput;
        playerCount=countInput;
        nameText.text=nameInput;
        sizeText.text=countInput+"/"+sizeInput;
    }
}
