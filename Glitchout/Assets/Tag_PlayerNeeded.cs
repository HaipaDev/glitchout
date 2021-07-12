using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_PlayerNeeded : MonoBehaviour{
    [SerializeField] int ID;
    void Update(){
        bool isActive;
        if(GameManager.instance.players.Length>ID){if(GameManager.instance.players[ID].playerScript!=null){isActive=true;}else{isActive=false;}}else{isActive=false;}
        foreach(Transform t in transform){t.gameObject.SetActive(isActive);}
    }
}
