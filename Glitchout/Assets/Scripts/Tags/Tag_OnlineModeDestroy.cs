using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tag_OnlineModeDestroy : MonoBehaviour{
    void Start(){
        if(!GameSession.instance.offlineMode){Destroy(gameObject);}
    }
}
