using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ClampUI : MonoBehaviour{
    RectTransform rectTransform;
    public int ID;
    [SerializeField] int maxID=5;
    [SerializeField] float moveAmnt=40;
    int clamp;

    private void Start() {
        rectTransform=GetComponent<RectTransform>();
        if(ID==1)clamp=1;
        if(ID==maxID)clamp=2;

        float xx=0;
        if(clamp==1){xx=moveAmnt;}
        if(clamp==2){xx=-moveAmnt;}
        rectTransform.localPosition=new Vector2(rectTransform.localPosition.x+xx,rectTransform.localPosition.y);
    }
    public void SetMaxID(int rowLimit){
        maxID=rowLimit;
    }

    /*RectTransform rectTransform;
    [SerializeField] RectTransform canvas;
    [SerializeField] string canvasName;
    void Start(){
        rectTransform=GetComponent<RectTransform>();
        if(canvasName=="")canvas=rectTransform.root.GetComponent<RectTransform>();
        if(canvas==null&&canvasName!="")canvas=GameObject.Find(canvasName).GetComponent<RectTransform>();
    }

    void Update(){
        //KeepOnScreen();
        ClampToWindow();
    }
    private void KeepOnScreen(){
     var refRes = (canvas.transform as RectTransform).sizeDelta;
     var goalX = rectTransform.anchoredPosition.x;
     var goalY = rectTransform.anchoredPosition.y;
 
     // This works if you change the pivot but don't change the anchors (or the scale).
     if (IsTooHigh (refRes))
         goalY = (refRes.y / 2f) - (rectTransform.sizeDelta.y * (1f - rectTransform.pivot.y));
     if (IsTooLow (refRes))
         goalY = -(refRes.y / 2f) + (rectTransform.sizeDelta.y * rectTransform.pivot.y);
     if (IsTooFarRight (refRes))
         goalX = (refRes.x / 2f) - (rectTransform.sizeDelta.x * (1f - rectTransform.pivot.x));
     if (IsTooFarLeft (refRes))
         goalX = -(refRes.x / 2f) + (rectTransform.sizeDelta.x * rectTransform.pivot.x);
 
     rectTransform.anchoredPosition = new Vector2 (goalX, goalY);
    }
    private bool IsTooHigh (Vector2 refRes)
    {
        return (rectTransform.anchoredPosition.y + (rectTransform.sizeDelta.y * (1f - rectTransform.pivot.y)) > refRes.y / 2f);
    }
    private bool IsTooLow (Vector2 refRes)
    {
        return (rectTransform.anchoredPosition.y - (rectTransform.sizeDelta.y * rectTransform.pivot.y) < -refRes.y / 2f);
    }
    private bool IsTooFarRight (Vector2 refRes)
    {
        return (rectTransform.anchoredPosition.x + (rectTransform.sizeDelta.x * (1f - rectTransform.pivot.x)) > refRes.x / 2f);
    }
    private bool IsTooFarLeft (Vector2 refRes)
    {
        return (rectTransform.anchoredPosition.x - (rectTransform.sizeDelta.x * rectTransform.pivot.x) < -refRes.x / 2f);
    }
    // Clamp panel to area of parent
    void ClampToWindow(){
        Vector3 pos = rectTransform.localPosition;
 
        Vector3 minPosition = canvas.rect.min - rectTransform.rect.min;
        Vector3 maxPosition = canvas.rect.max - rectTransform.rect.max;
 
        pos.x = Mathf.Clamp(rectTransform.localPosition.x, minPosition.x, maxPosition.x);
        pos.y = Mathf.Clamp(rectTransform.localPosition.y, minPosition.y, maxPosition.y);
 
        rectTransform.localPosition = pos;
    }*/
}
