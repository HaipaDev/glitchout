using Photon.Pun;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class GameSetupController : MonoBehaviour{
    //int playerCount=-1;
    void Start(){
        CreatePlayer();
    }

    private void CreatePlayer(){
        Debug.Log("Creating Player");
        GameObject player=PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerMulti"),Vector3.zero,Quaternion.identity,0);
        //glitchout.Player playerComp=player.GetComponent<glitchout.Player>();
        //glitchout.GameSession.instance.players.Add(player.GetComponent<glitchout.Player>());
        //glitchout.GameSession.instance.players=FindObjectsOfType<glitchout.Player>().ToList();
        //if(glitchout.GameSession.instance.players.Count()>0)playerComp.playerNum=glitchout.GameSession.instance.players.Count()-1;
        //playerCount+=1;
        //playerComp.playerNum=playerCount;
        //player.GetComponent<glitchout.Player>().playerNum=glitchout.GameSession.instance.players.FindIndex(x => x == playerComp);//glitchout.GameSession.instance.players.Where(x => x == player).SingleOrDefault().playerNum;
    }
}
