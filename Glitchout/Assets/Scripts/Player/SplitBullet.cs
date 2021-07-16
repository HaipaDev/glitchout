using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitBullet : MonoBehaviour{
    [HeaderAttribute("Config")]
    public float speed=8;
    public float rotSpeed=19;
    public float lifeTime=2f;
    [HeaderAttribute("Params")]
    public int playerID;
    public Vector2 coords;

    void Update(){
        var step=speed*Time.deltaTime;
        if(coords!=null){//Vector2 coordsFar=coords*3;
            transform.position=Vector2.MoveTowards(transform.position,coords,step);}
        if((Vector2)transform.position==coords){Destroy(gameObject);}
        lifeTime-=Time.deltaTime;
        if(lifeTime<=0){Destroy(gameObject);}

        float angle=0;
        angle+=rotSpeed*Time.timeScale;
        if(angle>=360)angle=0;
        if(angle<0)angle=360;
        transform.eulerAngles=new Vector3(0, 0, angle);
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Barrier")){Destroy(gameObject);}
    }
}