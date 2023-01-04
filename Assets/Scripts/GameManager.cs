using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private GameObject canvas_main_game;
    private GameObject canvas_campagne;
    private GameObject scene_campagne;

    public GameObject main_camera;
    public GameObject score;
    public TextMeshProUGUI text_scoreMax;

    public TextMeshProUGUI gold;
    private GameObject background_menu;
    private GameObject menu_tuto;
    private GameObject menu_continue;

    private GameObject pause;

    private Transform PosParticleWin;

    

    private bool perdu;


    public TextMeshProUGUI plusNb;

    GameObject player;


    //EVENT1
    public Material sky;
    public Material ground;
    public GameObject cacheVue;
    GameObject input_score;


    public AudioSource mBase;
    public AudioSource m1;
    public AudioSource m2;
    public AudioSource m3;

    private const int prix1=10
    ,prix2=30
    ,prix3=50
    ,prix4=100
    ,prix5=100
    ,prix6 =200
    ,prix7=300
    ,prix8=400
    ,prix9=500
    ,prix10=600
    ,prix11=600
    ,prix12=700
    ,prix13=700
    ,prix14=800
;

    private const int sMax1=100
    ,sMax2=300
    ,sMax3=500
    ,sMax4=1000
    ,sMax5=1500
    ,sMax6 =2000
    ,sMax7=2600
    ,sMax8=3000
    ,sMax9=4000
    ,sMax10=4600
    ,sMax11=5000
    ,sMax12=5400
    ,sMax13=5700
    ,sMax14=6000
;




    void Awake(){

        Time.timeScale = 1;

        mBase = GameObject.FindGameObjectsWithTag ("gameMusic")[0].GetComponent<AudioSource>();
        m1 = GameObject.FindGameObjectsWithTag ("gameMusic1")[0].GetComponent<AudioSource>();
        m2= GameObject.FindGameObjectsWithTag ("gameMusic2")[0].GetComponent<AudioSource>();
        m3 = GameObject.FindGameObjectsWithTag ("gameMusic3")[0].GetComponent<AudioSource>();

        main_camera = GameObject.Find("Main Camera");

        pause = GameObject.Find("pause");
        canvas_main_game = GameObject.Find("main_game");
        canvas_campagne = GameObject.Find("campagne");
        scene_campagne = GameObject.Find("Scene_campagne");


        menu_tuto=GameObject.Find("tuto");
        background_menu = GameObject.Find("background_menu");
        menu_continue = GameObject.Find("playAgain");

        PosParticleWin = GameObject.Find("PosParticleWin").transform;
        input_score = GameObject.Find("edit_score");

        menu_continue.transform.localScale = new Vector3(0,0,0);




        
    
        text_scoreMax.text = PlayerPrefs.GetInt("scoreMax",0).ToString();

        score.transform.localScale = new Vector3(1,1,1);
        plusNb.transform.localScale = new Vector3(0,0,0);



        PlayerPrefs.SetInt("shop_open",0);
        PlayerPrefs.SetInt("extra_open",0);
        PlayerPrefs.SetInt("event",0);
        PlayerPrefs.SetInt("campagneOpen",0);




    }

    void Start(){

       

        Invoke("fps",1);
        print(System.DateTime.Now);


        canvas_main_game.transform.localScale = new Vector3(1,1,1); //ACTIVER LE BON CANVAS
        canvas_campagne.SetActive(false);
        //scene_campagne.SetActive(false);//DESACTIVER SCENE INUTILE


        DateTime now = System.DateTime.Now;
        long temp = Convert.ToInt64(PlayerPrefs.GetString("daysName","0"));
        DateTime oldDate = DateTime.FromBinary(temp);
        TimeSpan difference = now.Subtract(oldDate);

        print(difference);
        if(difference.Days >= 1){//si ça fait un jour que ça a changé on peut re changer le pseudo
            PlayerPrefs.SetInt("namechanged",0);
            print("REINITIALISATION");
        }


        


        perdu = false;
        input_score.GetComponent<TMP_InputField>().keyboardType = TouchScreenKeyboardType.NumberPad;

        PlayerPrefs.SetInt("shop_open",0);
        PlayerPrefs.SetInt("extra_open",0);
        PlayerPrefs.SetInt("tools_open",0);
        PlayerPrefs.SetInt("leaderboard_open",0);
        PlayerPrefs.SetInt("leaderBoardEvent",0);
        PlayerPrefs.SetInt("isPaused",0);
        PlayerPrefs.SetInt("screenWinLooseOpen",0);

        string namePref = PlayerPrefs.GetString("name","");
        if(namePref != ""){
            GameObject.Find("player_name_input").GetComponent<TMP_InputField>().text = namePref;
        }
        


        ////////////////////////////////////////////////:








        CheckShop(); //on met au score bonus activé ; avant de recuperer l'ancien si l'utilisateur a regardé une pub + actualiser le shop
        actuMusic();
        actuCoins();
        CheckExtra();
        ActuAlphaExtra();

        if(PlayerPrefs.GetInt("restarted",0)==1){

            GameObject point = GameObject.Find("score");
            score.transform.localScale = new Vector3(1,1,1);
            point.GetComponent<TextMeshProUGUI>().text = PlayerPrefs.GetInt("scoreBefore",0).ToString();
            iTween.PunchScale(point,new Vector3(1.3f,1.3f,1.3f),0.7f);

            PlayerPrefs.SetInt("restarted",0);
        }
        else{
            //on remet à zero NbPub;
            PlayerPrefs.SetInt("nbAdd",2);

        }

        gold.text = PlayerPrefs.GetInt("gold",0).ToString();

        player = GameObject.FindWithTag("Player");


        InvokeRepeating("updatePlayer",0.2f,0.2f);


        //Hide event layout
        GameObject.Find("parentEvent").transform.localScale = new Vector3(0,0,0);
        pause.transform.localScale = new Vector3(0,0,0);
        GameObject.Find("menu_pause").transform.localScale = new Vector3(0,0,0);

        if(PlayerPrefs.GetInt("event_loose",0)==1){pause.transform.localScale = new Vector3(0,0,0);
            PlayerPrefs.SetInt("event_loose",0);
            StartEvent();
            
        }


        


    }
    void Update(){

        if(player==null && !perdu){
            //perdu
            if(PlayerPrefs.GetInt("event",0)==1){//event
                // save in event bdd
                int scoreASave = int.Parse(GameObject.Find("score").GetComponent<TextMeshProUGUI>().text);
                saveBestInBddEvent(scoreASave);

                Invoke("looseEvent",2);
            }
            else if(PlayerPrefs.GetInt("campagneOpen",0)==1 && GameObject.Find("campagne_screens")==null && PlayerPrefs.GetInt("screenWinLooseOpen",0)==0){ //Campagne

                print("ok update");

                GameObject.Find("CampagneManager").GetComponent<CampagneManager>().screenCampagne(0); //appel à la fonction loose
            }
            else if(PlayerPrefs.GetInt("event",0)==0 && PlayerPrefs.GetInt("campagneOpen",0)==0){//normal game
                

                pause.transform.localScale = new Vector3(0,0,0);
                Invoke("ShowContinue",1);
                perdu = true; 
            }
            
            
        }
        
    }
    
    void looseEvent(){
            PlayerPrefs.SetInt("event_loose",1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name); //reload scene
    }
    void updatePlayer(){
        player = GameObject.FindWithTag("Player");
    }

    void fps(){
        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }


    public void ReloadScene(){

        PlayerPrefs.SetInt("nbAdd",2);

        int scoreActu =int.Parse(GameObject.Find("score").GetComponent<TextMeshProUGUI>().text);
        int scoreMax = PlayerPrefs.GetInt("scoreMax",0);
        if(scoreActu > scoreMax){
            PlayerPrefs.SetInt("scoreMax",scoreActu);
        }

        saveBestInBdd(scoreActu);
        

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ShowContinue(){
        background_menu.transform.localScale = new Vector3(1,1,1);
        menu_tuto.transform.localScale = new Vector3(0,0,0);
        menu_continue.transform.localScale = new Vector3(1,1,1);
        

        Invoke("CountDownContinue",1);
        

    }

    public void CountDownContinue(){
        GameObject timer = GameObject.Find("textCompteur");
        int time = int.Parse(timer.GetComponent<TextMeshProUGUI>().text)-1;
        if(time == 0) Invoke("disableButtonPub",0.5f);
        if(time ==-1){
            ReloadScene();
        }
        else{
            iTween.PunchScale(timer,new Vector3(1.3f,1.3f,1.3f),0.7f);
            timer.GetComponent<TextMeshProUGUI>().text = time.ToString();
            Invoke("CountDownContinue",1.0f);
        }
    }
    public void disableButtonPub(){
        GameObject.Find("playAgain").GetComponent<Button>().interactable = false;
    }
    public void timerMoins1(){
        if(GameObject.FindWithTag("Player")==null){
            GameObject timer = GameObject.Find("textCompteur");
            int time = int.Parse(timer.GetComponent<TextMeshProUGUI>().text)-1;
            if(time >-1){
                timer.GetComponent<TextMeshProUGUI>().text = time.ToString();
                if(time == 0) Invoke("disableButtonPub",0.5f);
                
            }
        }
        else if(PlayerPrefs.GetInt("shop_open",0)==1){ //shop open
            if(PlayerPrefs.GetInt("extra_open")==1){
                closeExtra();
            }
            else closeShop();
        }
        else if(PlayerPrefs.GetInt("tools_open",0)==1){
            if(PlayerPrefs.GetInt("leaderboard_open",0)==1){
                closeLeaderBoard();
            }
            else closeTools();
        }
        else if(PlayerPrefs.GetInt("leaderBoardEvent",0)==1){
            closeLeaderBoardEvent();
        }
        
    }
    public void OpenPrivacy(){
        Application.OpenURL("https://pages.flycricket.io/geometry-runner/privacy.html");
    }

    public void HideCoins(){
        Invoke("Hide",2);
    }
    public void Hide(){
        GameObject shop = GameObject.Find("shop");
        shop.transform.localScale = new Vector3(0,0,0);
        TextMeshProUGUI plus = GameObject.Find("plusNb").GetComponent<TextMeshProUGUI>();
        plus.transform.localScale = new Vector3(0,0,0);
    }

    public void animateShop(){
        if(PlayerPrefs.GetInt("tools_open",0)!=1){
        GameObject.Find("Button_main").transform.localScale = new Vector3(0,0,0);
        GameObject.Find("parent").transform.localScale = new Vector3(200,200,200); //aparition menu
        PlayerPrefs.SetInt("shop_open",1);
        GameObject.Find("playerParent").GetComponent<Animator>().Play("shop");
        GameObject.Find("parent").GetComponent<Animator>().Play("shop2");
        GameObject.Find("handMove").transform.localScale = new Vector3(0,0,0);
        }
        
    }
    public void closeShop()
{


        PlayerPrefs.SetInt("shop_open",0);
   
        GameObject.Find("playerParent").GetComponent<Animator>().Play("shop_close");
        GameObject.Find("parent").GetComponent<Animator>().Play("shop2_close");
        Invoke("closeAfterTime",0.2f);
        Invoke("hideMenu",0.3f);
        
}    
void hideMenu(){GameObject.Find("parent").transform.localScale = new Vector3(0,0,0);}

    public void extra_open(){
        PlayerPrefs.SetInt("extra_open",1);
        GameObject.Find("parent").GetComponent<Animator>().Play("extra_open");
        
    }
    public void closeExtra(){
        PlayerPrefs.SetInt("extra_open",0);
        GameObject.Find("parent").GetComponent<Animator>().Play("extra_close");
        CheckExtra();
    }



    
    void CheckShop(){ //ajoute les points en fonction des prefs et change le texte de la carte dans le menu


        int golds = PlayerPrefs.GetInt("gold",0);
        int scoreMax = PlayerPrefs.GetInt("scoreMax",0);
        
        TextMeshProUGUI scor = score.GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI t = GameObject.Find("extra_main_prix").GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI texte = GameObject.Find("extra_main_txt").GetComponent<TextMeshProUGUI>();

        int prix=100;
        int scoreMin=100;



        if(PlayerPrefs.GetInt("shop_extra1")==1){
            scor.text = sMax1.ToString();
            texte.text = "Start at "+sMax2+" points";
            t.text = prix2+" Gold";

            prix =prix2;
            scoreMin=sMax2;

            if(PlayerPrefs.GetInt("shop_extra2")==1){
                scor.text = sMax2.ToString();
                texte.text = "Start at "+sMax3+" points";
                t.text = prix3+" Gold";

                prix =prix3;
                scoreMin=sMax3;

            if(PlayerPrefs.GetInt("shop_extra3")==1){
                scor.text = sMax3.ToString();
                texte.text = "Start at "+sMax4+" points";
                t.text = prix4+" Gold";
                prix =prix4;
                scoreMin=sMax4;

            if(PlayerPrefs.GetInt("shop_extra4")==1){
                scor.text = sMax4.ToString();
                texte.text = "Start at "+sMax5+" points";
                t.text = prix5+" Gold";
                prix =prix5;
                scoreMin=sMax5;

            if(PlayerPrefs.GetInt("shop_extra5")==1){
                scor.text = sMax5.ToString();
                texte.text = "Start at "+sMax6+" points";
                t.text = prix6+" Gold";
                prix =prix6;
                scoreMin=sMax6;

            if(PlayerPrefs.GetInt("shop_extra6")==1){
                scor.text = sMax6.ToString();
                texte.text = "Start at "+sMax7+" points";
                t.text = prix7+" Gold";
                prix =prix7;
                scoreMin=sMax7;

            if(PlayerPrefs.GetInt("shop_extra7")==1){
                scor.text = sMax7.ToString();
                texte.text = "Start at "+sMax8+" points";
                t.text = prix8+" Gold";
                prix =prix8;
                scoreMin=sMax8;

            if(PlayerPrefs.GetInt("shop_extra8")==1){
                scor.text = sMax8.ToString();
                texte.text = "Start at "+sMax9+" points";
                t.text = prix9+" Gold";
                prix =prix9;
                scoreMin=sMax9;

            if(PlayerPrefs.GetInt("shop_extra9")==1){
                scor.text = sMax9.ToString();  
                texte.text = "Start at "+sMax10+" points";
                t.text = prix10+" Gold";
                prix =prix10;
                scoreMin=sMax10;

            if(PlayerPrefs.GetInt("shop_extra10")==1){
                scor.text = sMax10.ToString();  
                texte.text = "Start at "+sMax11+" points";
                t.text = prix11+" Gold";
                prix =prix11;
                scoreMin=sMax11;

            if(PlayerPrefs.GetInt("shop_extra11")==1){
                scor.text = sMax11.ToString();  
                texte.text = "Start at "+sMax12+" points";
                t.text = prix12+" Gold";
                prix =prix12;
                scoreMin=sMax12;

            if(PlayerPrefs.GetInt("shop_extra12")==1){
                scor.text = sMax12.ToString();  
                texte.text = "Start at "+sMax13+" points";
                t.text = prix13+" Gold";
                prix =prix13;
                scoreMin=sMax13;

            if(PlayerPrefs.GetInt("shop_extra13")==1){
                scor.text = sMax13.ToString();  
                texte.text = "Start at "+sMax14+" points";
                t.text = prix14+" Gold";
                prix =prix14;
                scoreMin=sMax14;

            if(PlayerPrefs.GetInt("shop_extra14")==1){
                scor.text = sMax14.ToString();  
                texte.text = "Soon";
                t.text = "? Gold";

                
        }
        }
        }
        }
        }
        }
        }
        }
        }
        }
        }
        }
        }
        }
        else{//rien
            scor.text = "0";
            texte.text = "Start at "+sMax1+" points";
            t.text = prix1+" Gold";
            
            prix =prix1;
            scoreMin=sMax1;
            
        }

        if(golds < prix || scoreMax < scoreMin){
                t.faceColor = new Color32(255,255,255,80);  //TESTE SI ON A ASSSEZ D'ARGENT ET QUE LE SCORE EST SUPERIEUR A NOTRE SCORE MAXIMAL zzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz
            }
            else{
                t.faceColor = new Color32(255,255,255,255);
            }


        }


        public void openTools(){

        if(PlayerPrefs.GetInt("shop_open",0)!=1 && PlayerPrefs.GetInt("tools_open",0)!=1){

        
        PlayerPrefs.SetInt("tools_open",1);
        GameObject.Find("Button_main").transform.localScale = new Vector3(0,0,0);
        GameObject.Find("playerParent").GetComponent<Animator>().Play("shop");
        GameObject.Find("parentTools").GetComponent<Animator>().Play("tools");
        GameObject.Find("handMove").transform.localScale = new Vector3(0,0,0);
        GameObject.Find("tuto").GetComponent<TextMeshProUGUI>().faceColor = new Color32(255,255,255,0);// -texte
        }
        }

    public void closeTools()
{


        PlayerPrefs.SetInt("tools_open",0);
        GameObject.Find("playerParent").GetComponent<Animator>().Play("shop_close");
        GameObject.Find("parentTools").GetComponent<Animator>().Play("close_tools");
        Invoke("closeAfterTime",0.2f);
        
        
}    

void closeAfterTime(){
        GameObject.Find("Button_main").transform.localScale = new Vector3(1,1,1);
        GameObject.Find("handMove").transform.localScale = new Vector3(1,1,1);
        GameObject.Find("tuto").GetComponent<TextMeshProUGUI>().faceColor = new Color32(255,255,255,255);// +texte
}

public void openLeaderBoard(){
    PlayerPrefs.SetInt("leaderboard_open",1);
    GameObject.Find("tuto").GetComponent<TextMeshProUGUI>().faceColor = new Color32(255,255,255,0);// -texte
    GameObject.Find("TextscoreMax").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("parentTools").GetComponent<Animator>().Play("close_tools");
    GameObject.Find("leaderBoard").GetComponent<Animator>().Play("leaderBoard_open");
}
public void closeLeaderBoard(){
    PlayerPrefs.SetInt("leaderboard_open",0);
    GameObject.Find("TextscoreMax").transform.localScale = new Vector3(1,1,1);
    GameObject.Find("parentTools").GetComponent<Animator>().Play("tools");
    GameObject.Find("leaderBoard").GetComponent<Animator>().Play("leaderBoard_close");
}

public void changeName(){
    if(PlayerPrefs.GetInt("namechanged",0)==0 && GameObject.Find("player_name_input").GetComponent<TMP_InputField>().text != ""){
        PlayerPrefs.SetInt("namechanged",1);

        PlayerPrefs.SetString("daysName",System.DateTime.Now.ToBinary().ToString());
        PlayerPrefs.SetString("name",GameObject.Find("player_name_input").GetComponent<TMP_InputField>().text);
    }
}

public void sanitazeStr(){
    String chaine = GameObject.Find("player_name_input").GetComponent<TMP_InputField>().text;
    chaine = chaine.Replace("/","");
    chaine = chaine.Replace("\\","");
    chaine = chaine.Replace("%","");
    GameObject.Find("player_name_input").GetComponent<TMP_InputField>().text = chaine;

}
public void saveBestInBdd(int score){
    string pseudo = PlayerPrefs.GetString("name","player");

    HighScores.UploadScore(pseudo,score);
}
public void saveBestInBddEvent(int score){
    string pseudo = PlayerPrefs.GetString("name","player");

    HighScoresEvent.UploadScore(pseudo,score);
}



public void buy_bonus(GameObject buy){ //achat extra : 

    int prix = 0;
    int scoreMax =0;

    TextMeshProUGUI texte =buy.GetComponent<TextMeshProUGUI>();
    TextMeshProUGUI t = GameObject.Find("extra_main_prix").GetComponent<TextMeshProUGUI>();
        if(texte.text == "Start at 100 points"){
            prix = prix1;
            scoreMax=sMax1;
            
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix && PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra1",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop
                

            }
        }

        else if(texte.text == "Start at 300 points"){
            prix = prix2;
            scoreMax=sMax2;
           
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra2",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop

            }
        }

        else if(texte.text == "Start at 500 points"){
            prix = prix3;
            scoreMax=sMax3;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra3",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop
            }
        }


        else if(texte.text == "Start at 1000 points"){
            prix = prix4;
            scoreMax=sMax4;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra4",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop

            }
        }


        else if(texte.text == "Start at 1500 points"){
            prix = prix5;
            scoreMax=sMax5;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra5",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
               t.text = "enabled"; //juste pour changer le texte et declencher checkshop

            }
        }

        else if(texte.text == "Start at 2000 points"){
            prix = prix6;
            scoreMax=sMax6;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra6",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop

            }
        }

        else if(texte.text == "Start at 2600 points"){
            prix = prix7;
            scoreMax=sMax7;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra7",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop

            }
        }
        else if(texte.text == "Start at 3000 points"){
            prix = prix8;
            scoreMax=sMax8;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra8",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop

            }
        }

        else if(texte.text == "Start at 4000 points"){
            prix = prix9;
            scoreMax=sMax9;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra9",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop


            }
        }
        else if(texte.text == "Start at 4600 points"){
            prix = prix10;
            scoreMax=sMax10;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra10",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop


            }
        }
        else if(texte.text == "Start at 5000 points"){
            prix = prix11;
            scoreMax=sMax11;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra11",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop


            }
        }
        else if(texte.text == "Start at 5400 points"){
            prix = prix12;
            scoreMax=sMax12;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra12",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop


            }
        }
        else if(texte.text == "Start at 5700 points"){
            prix = prix13;
            scoreMax=sMax13;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra13",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop


            }
        }
        else if(texte.text == "Start at 6000 points"){
            prix = prix14;
            scoreMax=sMax14;
            if(t.text == prix+ " Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=prix&& PlayerPrefs.GetInt("scoreMax",0) >=scoreMax){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
            

                PlayerPrefs.SetInt("shop_extra14",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-prix);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - prix).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                //On passe à la prochaine
                t.text = "enabled"; //juste pour changer le texte et declencher checkshop


            }
        }




        if(t.text != "CONFIRM ?") CheckShop();
        ActuAlphaExtra();
}

public void LaunchGame(){
    if(background_menu.transform.localScale == new Vector3(1,1,1)){ //Si visible
                    background_menu.transform.localScale = new Vector3(0,0,0);
                    GameObject.Find("shop").transform.localScale = new Vector3(0,0,0); //on cache les golds une fois
                }
                if(score.transform.localScale == new Vector3(0,0,0)){
                    score.transform.localScale = new Vector3(1,1,1);
                }
                GameObject.FindWithTag("Player").GetComponent<Rigidbody>().useGravity = true;
                GameObject.FindWithTag("Player").GetComponent<Animation>().Play("Run");
                GameObject.Find("shop_txt").transform.localScale = new Vector3(0,0,0);
                GameObject.Find("Button_main").transform.localScale = new Vector3(0,0,0);

                pause.transform.localScale = new Vector3(1,1,1);

                if(PlayerPrefs.GetInt("event",0)==1){
                    GameObject.Find("parentEvent").transform.localScale = new Vector3(0,0,0);
                    GameObject.Find("score").GetComponent<TextMeshProUGUI>().text = "0";
                }

                if(PlayerPrefs.GetInt("campagneOpen",0)==0){  //ON INVOKE CHECKSCORE SEULEMENT SI ON LNACE UNE PARTIE ; il sera desactiver au reload de la scene (toujours apres la partie)
                    GameObject.Find("Spawner").GetComponent<spawner>().startInvokeCheckScore();
                }
}
public void switchMusic(TextMeshProUGUI swap){

    if(swap.text == "ON"){
        PlayerPrefs.SetInt("music",0);
    }
    else{
        PlayerPrefs.SetInt("music",1);
    }
    actuMusic();
}
void actuMusic(){
    if(PlayerPrefs.GetInt("music",1)==1){ //activer
        GameObject.Find("background_music_mBase").GetComponent<AudioSource>().mute = false;
        GameObject.Find("background_music_m1").GetComponent<AudioSource>().mute = false;
        GameObject.Find("background_music_m2").GetComponent<AudioSource>().mute = false;
        GameObject.Find("background_music_m3").GetComponent<AudioSource>().mute = false;

        GameObject.Find("music_switch").GetComponent<TextMeshProUGUI>().text = "ON";
    }
    else{ //desactiver
        GameObject.Find("background_music_mBase").GetComponent<AudioSource>().mute = true;
        GameObject.Find("background_music_m1").GetComponent<AudioSource>().mute = true;
        GameObject.Find("background_music_m2").GetComponent<AudioSource>().mute = true;
        GameObject.Find("background_music_m3").GetComponent<AudioSource>().mute = true;
        GameObject.Find("music_switch").GetComponent<TextMeshProUGUI>().text = "OFF";
    }
}

public void switchCoin(TextMeshProUGUI swap){

    if(swap.text == "ON"){
        PlayerPrefs.SetInt("coin_active",0);
    }
    else{
        PlayerPrefs.SetInt("coin_active",1);
    }
    actuCoins();
}
void actuCoins(){
    if(PlayerPrefs.GetInt("coin_active",1)==1){ //activer
        GameObject.Find("coin_switch").GetComponent<TextMeshProUGUI>().text = "ON";
    }
    else{ //desactiver
        GameObject.Find("coin_switch").GetComponent<TextMeshProUGUI>().text = "OFF";
    }
}
public void VerifCodeBonus(){
    TMP_InputField text = GameObject.Find("codeBonus").GetComponent<TMP_InputField>();

    if(text.text == "" && PlayerPrefs.GetInt("bonus1000",0)==0){ //Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité
        PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+1000);//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité//Codes supprimées par mesure de sécurité
        PlayerPrefs.SetInt("bonus1000",1);
    }
    if(text.text == "" && PlayerPrefs.GetInt("passgold1500",0)==0){
        PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+1500);
        PlayerPrefs.SetInt("passgold1500",1);
    }

    if(text.text == "" && PlayerPrefs.GetInt("code100",0)==0){
        PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+100);
        PlayerPrefs.SetInt("code100",1);
    }
    if(text.text == "" && PlayerPrefs.GetInt("pwd500",0)==0){
        PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+500);
        PlayerPrefs.SetInt("pwd500",1);
    }
    if(text.text.Contains(",")){
        String[] split = text.text.Split(",");
        GameObject.Find("score").GetComponent<TextMeshProUGUI>().text = split[1];
    }

}
public void VerifCodeBonusGold(){
    TMP_InputField text = GameObject.Find("codeBonus").GetComponent<TMP_InputField>();

    if(text.text.Contains(",")){
        String[] split = text.text.Split(",");
        PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)+int.Parse(split[1]));

    }

}


public void StartEvent(){
   
    GameObject.Find("Button_main").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("parent").transform.localScale = new Vector3(200,200,200); //aparition menu
    GameObject.Find("handMove").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("tuto").GetComponent<TextMeshProUGUI>().faceColor = new Color32(255,255,255,0);// -texte
    GameObject.Find("TextscoreMax").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("shop").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("tools").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("parentEvent").transform.localScale = new Vector3(1,1,1);
    GameObject.Find("parentEvent2").GetComponent<Animator>().Play("event1_open");
    PlayerPrefs.SetInt("coin_active",0);
    PlayerPrefs.SetInt("event",1);
    score.transform.localScale = new Vector3(0,0,0);
    GameObject.Find("edit_score").transform.localScale = new Vector3(0,0,0);

    
    GameObject.Find("Skybox").GetComponent<MeshRenderer>().material = sky;
    GameObject.Find("Ground").GetComponent<MeshRenderer>().material = ground;
    cacheVue.SetActive(true);
    GameObject.Find("score").GetComponent<TextMeshProUGUI>().text = "0";
        

}
public void ExitEvent(){
    PlayerPrefs.SetInt("coin_active",1);
    PlayerPrefs.SetInt("event",0);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
public void openLeaderBoardEvent(){
    GameObject.Find("Canvas").GetComponent<HighScoresEvent>().DownloadScores();
    PlayerPrefs.SetInt("leaderBoardEvent",1);
    GameObject.Find("parentEvent2").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("leaderBoard_show_event").GetComponent<Animator>().Play("leaderBoard_open");
    GameObject.Find("event_button_start").transform.localScale = new Vector3(0,0,0);
    GameObject.Find("event_button_quit").transform.localScale = new Vector3(0,0,0);
}
public void closeLeaderBoardEvent(){
    PlayerPrefs.SetInt("leaderBoardEvent",0);
    GameObject.Find("parentEvent2").transform.localScale = new Vector3(1,1,1);
    GameObject.Find("leaderBoard_show_event").GetComponent<Animator>().Play("leaderBoard_close");
    GameObject.Find("event_button_start").transform.localScale = new Vector3(1,1,1);
    GameObject.Find("event_button_quit").transform.localScale = new Vector3(1,1,1);
}
int findMaxScoreStart(){

   int maxi=0;

    if(PlayerPrefs.GetInt("shop_extra1")==1){
        maxi = 100;
            
            if(PlayerPrefs.GetInt("shop_extra2")==1){
                maxi = 300;

            if(PlayerPrefs.GetInt("shop_extra3")==1){
                maxi = 500;

            if(PlayerPrefs.GetInt("shop_extra4")==1){
                maxi = 1000;

            if(PlayerPrefs.GetInt("shop_extra5")==1){
               maxi = 1500;

            if(PlayerPrefs.GetInt("shop_extra6")==1){
               maxi = 2000;

            if(PlayerPrefs.GetInt("shop_extra7")==1){
              maxi = 2600;

            if(PlayerPrefs.GetInt("shop_extra8")==1){
               maxi = 3000;

            if(PlayerPrefs.GetInt("shop_extra9")==1){
                maxi = 4000;
            if(PlayerPrefs.GetInt("shop_extra10")==1){
                maxi = 4600;
            if(PlayerPrefs.GetInt("shop_extra11")==1){
                maxi = 5000;

            if(PlayerPrefs.GetInt("shop_extra12")==1){
                maxi = 5400;

            if(PlayerPrefs.GetInt("shop_extra13")==1){
                maxi = 5700;

            if(PlayerPrefs.GetInt("shop_extra14")==1){
                maxi = 6000;
        }
        }
        }
        }

        }}}}}}}}}}

   return maxi;
}
public void checkAndChangeScore(TMP_InputField text){ //check si le score entré est valide et change le score en fct
    int number = int.Parse(text.text);
    int maxi = findMaxScoreStart();
    
    if(number <= maxi){
        GameObject.Find("score").GetComponent<TextMeshProUGUI>().text = number.ToString();
    }
    text.text = "";


    
}














 public void buy_extra(GameObject buy){ //achat extra : 
        if(buy.name == "m1_prix"){
            TextMeshProUGUI t =buy.GetComponent<TextMeshProUGUI>();
            if(t.text == "200 Gold"){

                if(PlayerPrefs.GetInt("gold",0)>=200){
                    t.text = "CONFIRM ?";
                }
                
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
                t.text = "Enabled";
                PlayerPrefs.SetInt("m1_active",1);
                PlayerPrefs.SetInt("m2_active",0);
                PlayerPrefs.SetInt("m3_active",0);

                PlayerPrefs.SetInt("m1",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-200);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - 200).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);
                CheckExtra();

            }
            else if(t.text =="Disabled"){ //pour l'activer
                PlayerPrefs.SetInt("m1_active",1);
                t.text = "Enabled";

                PlayerPrefs.SetInt("m2_active",0); //desactiver les autres bonus
                PlayerPrefs.SetInt("m3_active",0);
                CheckExtra();

            }
            else if(t.text == "Enabled"){
                PlayerPrefs.SetInt("m1_active",0);
                t.text = "Disabled";
                CheckExtra();
                
            }


            
        }




        else if(buy.name == "m2_prix"){
            TextMeshProUGUI t =buy.GetComponent<TextMeshProUGUI>();
            if(t.text =="200 Gold"){
                if(PlayerPrefs.GetInt("gold",0)>=200){
                    t.text = "CONFIRM ?";
                }
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
                t.text = "Enabled";
                
                PlayerPrefs.SetInt("m1_active",0);
                PlayerPrefs.SetInt("m2_active",1);
                PlayerPrefs.SetInt("m3_active",0);

                PlayerPrefs.SetInt("m2",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-200);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - 200).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);
                CheckExtra();
                
                
            }
            else if(t.text =="Disabled"){//pour l'activer
                PlayerPrefs.SetInt("m2_active",1);
                t.text = "Enabled";

                PlayerPrefs.SetInt("m1_active",0); //desactiver les autres bonus
                PlayerPrefs.SetInt("m3_active",0);
                CheckExtra();

                
            }
            else if(t.text == "Enabled"){
                PlayerPrefs.SetInt("m2_active",0);
                t.text = "Disabled";
                CheckExtra();
                
            }

            
        }




        else if(buy.name == "m3_prix"){
            TextMeshProUGUI t =buy.GetComponent<TextMeshProUGUI>();
            if(t.text == "200 Gold"){
                if(PlayerPrefs.GetInt("gold",0)>=200){
                    t.text = "CONFIRM ?";
                }
            }
            else if(t.text == "CONFIRM ?"){//c'est confirmé
                t.text = "Enabled";
                PlayerPrefs.SetInt("m1_active",0);
                PlayerPrefs.SetInt("m2_active",0);
                PlayerPrefs.SetInt("m3_active",1);

                PlayerPrefs.SetInt("m3",1);
                PlayerPrefs.SetInt("gold",PlayerPrefs.GetInt("gold",0)-200);
                GameObject argent = GameObject.Find("gold");
                argent.GetComponent<TextMeshProUGUI>().text = (int.Parse(argent.GetComponent<TextMeshProUGUI>().text) - 200).ToString();
                iTween.PunchScale(argent,new Vector3(1.3f,1.3f,1.3f),0.7f);

                CheckExtra();

                
                
            }
            else if(t.text =="Disabled"){//pour l'activer
                PlayerPrefs.SetInt("m3_active",1);
                t.text = "Enabled";

                PlayerPrefs.SetInt("m1_active",0); //desactiver les autres bonus
                PlayerPrefs.SetInt("m2_active",0);
                CheckExtra();

               
            }
            else if(t.text == "Enabled"){
                PlayerPrefs.SetInt("m3_active",0);
                t.text = "Disabled";
                CheckExtra();
                
            }

            
        }
        
        ActuAlphaExtra();
        
    }





    void CheckExtra(){ //met enable et disable en fct des prefs et joue la bonne musique
        if(PlayerPrefs.GetInt("m1")==1){
            if(PlayerPrefs.GetInt("m1_active")==1){
                GameObject.Find("m1_prix").GetComponent<TextMeshProUGUI>().text = "Enabled";
                //change music
                if(!m1.isPlaying){
                    mBase.Stop();m2.Stop();m3.Stop();
                    m1.Play();

                }
                

            }
            else{
                GameObject.Find("m1_prix").GetComponent<TextMeshProUGUI>().text = "Disabled";
            }
        }
        if(PlayerPrefs.GetInt("m2")==1){
            if(PlayerPrefs.GetInt("m2_active")==1){
                GameObject.Find("m2_prix").GetComponent<TextMeshProUGUI>().text = "Enabled";
                //change music
                if(!m2.isPlaying){
                    mBase.Stop();m1.Stop();m3.Stop();
                    m2.Play();
                }
                
            }
            else{
                GameObject.Find("m2_prix").GetComponent<TextMeshProUGUI>().text = "Disabled";
            }
        }
        if(PlayerPrefs.GetInt("m3")==1){
            if(PlayerPrefs.GetInt("m3_active")==1){
                GameObject.Find("m3_prix").GetComponent<TextMeshProUGUI>().text = "Enabled";
                //change music
                if(!m3.isPlaying){
                    mBase.Stop();m1.Stop();m2.Stop();
                    m3.Play();
                }
                
            }
            else{
                GameObject.Find("m3_prix").GetComponent<TextMeshProUGUI>().text = "Disabled";
            }
        }

        if(PlayerPrefs.GetInt("m1_active")==0 && PlayerPrefs.GetInt("m2_active")==0 && PlayerPrefs.GetInt("m3_active")==0){
            //play music base
            if(!mBase.isPlaying){
                m1.Stop();m2.Stop();m3.Stop();
                mBase.Play();
            }
            
        }
        }
        public void ListenMusic(TextMeshProUGUI music){
            if(music.text == "MUSIC 1"){
                if(!m1.isPlaying){
                    mBase.Stop();m2.Stop();m3.Stop();
                    m1.Play();

                }
            }
            else if(music.text == "MUSIC 2"){
                if(!m2.isPlaying){
                    mBase.Stop();m1.Stop();m3.Stop();
                    m2.Play();
                }
            }
            else if(music.text == "MUSIC 3"){
                if(!m3.isPlaying){
                    mBase.Stop();m1.Stop();m2.Stop();
                    m3.Play();
                }
            }
        }

        void ActuAlphaExtra(){ //met l'alpha des prix en fct des golds qu'on a
        int money = PlayerPrefs.GetInt("gold",0); 

        TextMeshProUGUI e1 = GameObject.Find("m1_prix").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI e2 = GameObject.Find("m2_prix").GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI e3 = GameObject.Find("m3_prix").GetComponent<TextMeshProUGUI>();

        if(money >=1000){
            e1.faceColor = new Color32(255,255,255,255);
            e2.faceColor = new Color32(255,255,255,255);
            e3.faceColor = new Color32(255,255,255,255);
        }
        else{
            if(PlayerPrefs.GetInt("m1",0)==0) e1.faceColor = new Color32(255,255,255,80);
            if(PlayerPrefs.GetInt("m2",0)==0) e2.faceColor = new Color32(255,255,255,80);
            if(PlayerPrefs.GetInt("m3",0)==0) e3.faceColor = new Color32(255,255,255,80);
        }
    }

    public void pauseGame(){
        if(Time.timeScale ==1){

            Time.timeScale = 0;
            GameObject.Find("menu_pause").transform.localScale = new Vector3(1,1,1);
            PlayerPrefs.SetInt("isPaused",1);

            }
        else{
            
            Time.timeScale = 1;
            GameObject.Find("menu_pause").transform.localScale = new Vector3(0,0,0);
            PlayerPrefs.SetInt("isPaused",0);
            
            }
    }

    public void openCampagne(){

        //activer le canvas
        canvas_campagne.SetActive(true);
        canvas_main_game.transform.localScale = new Vector3(0,0,0);


        //rendre invisible les objets genants pour les animations
        
        invisibleVisibleForAnim('c','i');

        //Animations
        main_camera.GetComponent<Animator>().Play("cam90degres");  //tourner camera vers la gauche
        //canvas_main_game.GetComponent<Animator>().Play("back_main_game");
        canvas_campagne.GetComponent<Animator>().Play("push_campagne");

        //rendre visible à nouveau

        Invoke("visibleCampagneTIMED",0.4f);

        //desactiver le mauvais canvas

        Invoke("desactiverCanvasMain",0.4f);


        //PLAYER PREFS POUR DIRE QUE LA CAMPAGNE EST OUVERTE:

        PlayerPrefs.SetInt("campagneOpen",1);


    }

    public void closeCampagne(){



        //rendre invisible les objets genants pour les animations
        invisibleVisibleForAnim('c','i');

        //Animations
        main_camera.GetComponent<Animator>().Play("cam-90degres");  //tourner camera vers la gauche
        canvas_campagne.GetComponent<Animator>().Play("back_campagne");

        //rendre visible à nouveau le canvas

        Invoke("activerCanvasMain",0.5f);

        //desactiver le mauvais canvas

        Invoke("desactiverCanvasCampagne",0.4f);


        PlayerPrefs.SetInt("campagneOpen",0);



    }

    private void desactiverCanvasMain(){  //DESACTIVER / ACTIVER LE CANVAS APRES UN TEMPS DONNE
        canvas_main_game.transform.localScale = new Vector3(0,0,0);
    }
    private void activerCanvasMain(){  //DESACTIVER / ACTIVER LE CANVAS APRES UN TEMPS DONNE
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //RELOAD DE LA SCENE
    }
    public void desactiverCanvasCampagneMenu(){  //DESACTIVER / ACTIVER LE CANVAS APRES UN TEMPS DONNE
        GameObject.Find("campagne_menu").transform.localScale = new Vector3(0,0,0);
        //desactiver le swipe detector
        GameObject.Find("SwipeDetector").SetActive(false);
    }

    public void invisibleVisibleForAnim(char canvas,char mode){//canvas : 'c' : campagne , 'g' : main game     ;    mode : 'v' : visible  , 'i' : invisible
        
        
        if(canvas=='c'){

            if(mode=='v'){
                
                GameObject.Find("campagne_screen_2").transform.localScale = new Vector3(1,1,1);
                GameObject.Find("campagne_screen_1").transform.localScale = new Vector3(1,1,1);
            }
            else if(mode=='i'){
                int cubeScreen = GameObject.Find("CampagneManager").GetComponent<CampagneManager>().cubeAtScreen();
                if(cubeScreen == 1){
                    GameObject.Find("campagne_screen_2").transform.localScale = new Vector3(0,0,0);
                }
                else if(cubeScreen == 2){
                    GameObject.Find("campagne_screen_1").transform.localScale = new Vector3(0,0,0);
                }
                else{
                    print("erreur 'invisibleVisibleForAnim'  cube at screen");
                    GameObject.Find("campagne_screen_2").transform.localScale = new Vector3(0,0,0);
                }
                
            }
            else{
                print("erreur 'invisibleVisibleAfterAnim' mode invalide");
            }

        }
        else if(canvas=='g'){

            if(mode=='v'){
                GameObject.Find("parentTools").transform.localScale = new Vector3(1,1,1);
                GameObject.Find("parent").transform.localScale = new Vector3(1,1,1);
            }
            else if(mode=='i'){

                GameObject.Find("parentTools").transform.localScale = new Vector3(0,0,0);
                GameObject.Find("parent").transform.localScale = new Vector3(0,0,0);
            }


            else{
                print("erreur 'invisibleVisibleAfterAnim' mode invalide");
            }

        }
        else{
            print("erreur 'invisibleVisibleAfterAnim' canvas invalide");
        }
    }

    private void visibleCampagneTIMED(){
        invisibleVisibleForAnim('c','v');
        

    }
}


/*
    private void openCampagneInstant(){
        //activer le canvas
        canvas_campagne.SetActive(true);
        canvas_main_game.transform.localScale = new Vector3(0,0,0);



        //Animations
        main_camera.transform.localRotation = Quaternion.Euler(0,-90,0);  //tourner camera vers la gauche
       
        canvas_campagne.transform.position = new Vector3(0,0,0);

        //desactiver le mauvais canvas

        Invoke("desactiverCanvasMain",0.4f);


        //PLAYER PREFS POUR DIRE QUE LA CAMPAGNE EST OUVERTE:

        PlayerPrefs.SetInt("campagneOpen",1);


    }

}

    

       

        /*public void return_card(GameObject carte){
        if(carte.name == "extra1"){
            
            TextMeshProUGUI texte =GameObject.Find("moy").GetComponent<TextMeshProUGUI>();
            if(texte.text == "Advanced mode"){//tourner

                carte.GetComponent<Animator>().Play("extra_card");

                texte.text = "Start from 50 point";
                GameObject.Find("moy").transform.localRotation= Quaternion.Euler(0,180,0);
                GameObject.Find("moy_prix").transform.localRotation= Quaternion.Euler(0,180,0);
            }
            else{//retourner
                carte.GetComponent<Animator>().Play("extra_card_back");

                texte.text = "Advanced mode";
                GameObject.Find("moy").transform.localRotation= Quaternion.Euler(0,0,0);
                GameObject.Find("moy_prix").transform.localRotation= Quaternion.Euler(0,0,0);
            }
        }
        else if(carte.name =="extra2"){
            TextMeshProUGUI texte =GameObject.Find("dif").GetComponent<TextMeshProUGUI>();
            if(texte.text == "Hard mode"){//tourner

            carte.GetComponent<Animator>().Play("extra_card");

            texte.text = "Start from 200 point";
            GameObject.Find("dif").transform.localRotation= Quaternion.Euler(0,180,0);
            GameObject.Find("dif_prix").transform.localRotation= Quaternion.Euler(0,180,0);
            }
            else{//retourner
                carte.GetComponent<Animator>().Play("extra_card_back");

            GameObject.Find("dif").GetComponent<TextMeshProUGUI>().text = "Hard mode";
            GameObject.Find("dif").transform.localRotation= Quaternion.Euler(0,0,0);
            GameObject.Find("dif_prix").transform.localRotation= Quaternion.Euler(0,0,0);
            }
        }
        else if(carte.name =="extra3"){
            TextMeshProUGUI texte =GameObject.Find("exp").GetComponent<TextMeshProUGUI>();
            if(texte.text == "Expert mode"){ //tourner

            carte.GetComponent<Animator>().Play("extra_card");

            texte.text = "Start from 500 point";
            GameObject.Find("exp").transform.localRotation= Quaternion.Euler(0,180,0);
            GameObject.Find("exp_prix").transform.localRotation= Quaternion.Euler(0,180,0);
            }
            else{//retourner
                carte.GetComponent<Animator>().Play("extra_card_back");

            GameObject.Find("exp").GetComponent<TextMeshProUGUI>().text = "Expert mode";
            GameObject.Find("exp").transform.localRotation= Quaternion.Euler(0,0,0);
            GameObject.Find("exp_prix").transform.localRotation= Quaternion.Euler(0,0,0);
            }
            
        }

    }
    */