using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class collectables : MonoBehaviour
{

    private GameObject shop;
    private TextMeshProUGUI text_scoreMax;
    private GameObject plusNb;

    void Start()
    {
        shop = GameObject.Find("shop");
        text_scoreMax = GameObject.Find("gold").GetComponent<TextMeshProUGUI>();
        plusNb = GameObject.Find("plusNb");

       
    
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void AddCoins(){
        //on affiche les golds:
        shop.transform.localScale=new Vector3(1,1,1);

        if(gameObject.tag=="Silver"){
            PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+1);
            //show +1
        
            plusNb.GetComponent<TextMeshProUGUI>().text = "+1";
            
        }
        else if(gameObject.tag =="Gold"){
            PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+2);
            //show +2
            plusNb.GetComponent<TextMeshProUGUI>().text = "+2";
            
        }
        else if(gameObject.tag =="Orange"){
            PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+3);
            //show +2
            plusNb.GetComponent<TextMeshProUGUI>().text = "+3";
            
        }
        plusNb.transform.localScale = new Vector3(1,1,1);
        iTween.PunchScale(plusNb,new Vector3(1.3f,1.3f,1.3f),0.7f);
        

        iTween.MoveTo(this.gameObject, iTween.Hash(
            "position",GameObject.Find("gold_image").transform.position,
                "time",2f,
                "easetype", iTween.EaseType.easeInOutSine,
                "ignoretimescale", false,
                "name", "monItween",
                "onstart", "OnStart",
                "onupdate", "OnUpdate",
                "oncomplete", "OnComplete")
        );

        text_scoreMax.text = PlayerPrefs.GetInt("gold").ToString(); //maj gold



        GameObject.Find("GameManager").GetComponent<GameManager>().HideCoins(); //cacher les pieces dans gameManager car piece detruite !

    }

    void OnTriggerEnter(Collider collisionInfo){
        if(collisionInfo.gameObject.tag == "Player"){
            //gameObject.SetActive(false);
            AddCoins();
        }
    }
}//SCRIPT DETRUIT DONC PAS DE SUITE AUX ACTIONS : NE PAS DETRUIRE LA PIECE MAIS LA CACHER
