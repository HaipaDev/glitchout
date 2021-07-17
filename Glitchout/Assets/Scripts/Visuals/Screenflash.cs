using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Screenflash : MonoBehaviour{
    [SerializeField] Color damageFlashColor;
    [SerializeField] float damageFlashSpeed;
    [SerializeField] Color healFlashColor;
    [SerializeField] float healedFlashSpeed;
    [SerializeField] Color shadowFlashColor;
    [SerializeField] float shadowFlashSpeed;
    [SerializeField] Color flameFlashColor;
    [SerializeField] float flameFlashSpeed;
    [SerializeField] Color electrcFlashColor;
    [SerializeField] float electrcFlashSpeed;
    PlayerScript PlayerScript;
    Image image;
    // Start is called before the first frame update
    void Start(){
        PlayerScript=FindObjectOfType<PlayerScript>().GetComponent<PlayerScript>();
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update(){
        /*if(PlayerScript.damaged==true){image.color = damageFlashColor;}
        else { image.color = Color.Lerp(image.color, Color.clear, damageFlashSpeed * Time.deltaTime); }
        if(PlayerScript.healed==true){image.color = healFlashColor;}
        else { image.color = Color.Lerp(image.color, Color.clear, healedFlashSpeed * Time.deltaTime); }
        if (PlayerScript.shadowed==true){image.color = shadowFlashColor;}
        else { image.color = Color.Lerp(image.color, Color.clear, shadowFlashSpeed * Time.deltaTime); }
        if (PlayerScript.flamed==true){image.color = flameFlashColor;}
        else { image.color = Color.Lerp(image.color, Color.clear, flameFlashSpeed * Time.deltaTime); }
        if (PlayerScript.electricified==true){image.color = electrcFlashColor;}
        else { image.color = Color.Lerp(image.color, Color.clear, electrcFlashSpeed * Time.deltaTime); }
        PlayerScript.damaged = false;
        PlayerScript.healed = false;
        PlayerScript.shadowed = false;
        PlayerScript.flamed = false;
        PlayerScript.electricified = false;*/
    }
}
