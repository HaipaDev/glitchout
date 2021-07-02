using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfNotMulti : MonoBehaviour{
    void Awake() {//Check if multiplayer
        if(FindObjectOfType<NetworkController>()==null){
            Destroy(GetComponent<PhotonView>());
        }
    }
}
