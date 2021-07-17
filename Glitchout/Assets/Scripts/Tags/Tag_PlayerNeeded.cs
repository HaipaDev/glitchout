using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_PlayerNeeded : MonoBehaviour{
    [SerializeField] int ID;
    void Update(){
        if(GameManager.instance.players.Length>ID){if(GameManager.instance.players[ID].playerScript!=null){TurnAllChildren(true);}else{TurnAllChildren(false);}}else{TurnAllChildren(false);}
    }
    void TurnAllChildren(bool on){foreach(Transform t in transform){t.gameObject.SetActive(on);}}
}