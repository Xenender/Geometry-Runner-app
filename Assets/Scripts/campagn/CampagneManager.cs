using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CampagneManager : MonoBehaviour
{
    // Start is called before the first frame update

    private GameObject parent_cube_1;
    private GameObject parent_cube_2;
    private GameObject textLvl;

    private const int nbLvlInWorld = 9;
    private const int nbWorld = 2;

    private bool canswitchLevel;


    public GameObject campagne_screen;
    public GameObject swipeDetector;
    public GameObject main_camera;

    public GameObject PlayerPrefab;


    void Start()
    {
        parent_cube_1 = GameObject.Find("campagne_screen_1");
        parent_cube_2 = GameObject.Find("campagne_screen_2");
        textLvl = GameObject.Find("campagne_text_lvl_nb");
        canswitchLevel = true;
        checkProgressValue();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


  

    public void switchLevelRight(){

        
        //CHANGER LE CUBE
        if(canswitchLevel    &&   textLvl.GetComponent<TextMeshProUGUI>().text !=(nbWorld+" - "+nbLvlInWorld)){  //SI ON DEPASSE PAS LE NOMBRE MAX DE LEVEL

            canswitchLevel = false;

            int cubeScreen = cubeAtScreen();
            if(cubeScreen== 1){
                putCubeAtPosition(parent_cube_2,'r');

                parent_cube_1.GetComponent<Animator>().Play("level_1_left");
                parent_cube_2.GetComponent<Animator>().Play("level_2_left");

            }
            else if(cubeScreen== 2){
                putCubeAtPosition(parent_cube_1,'r');

                parent_cube_2.GetComponent<Animator>().Play("level_1_left");
                parent_cube_1.GetComponent<Animator>().Play("level_2_left");

            }
            else print("erreur cubeAtScreen in switch level");


            //bouton inclickable pendant l'animation (0.3f secondes)

            GameObject.Find("campagne_lvl_right").GetComponent<Button>().interactable = false;
            GameObject.Find("campagne_lvl_left").GetComponent<Button>().interactable = false;
            Invoke("reactiverbuttons",0.4f);


            //CHANGER LE TEXTE
            Invoke("plusLvl",0.2f);

            //On peut à nouveau changer de niveau
        
            Invoke("canswitch",0.4f);
        }
        
    }
    public void switchLevelLeft(){

        if(canswitchLevel   &&  textLvl.GetComponent<TextMeshProUGUI>().text != "1 - 1"){
            canswitchLevel = false;

            int cubeScreen = cubeAtScreen();
            if(cubeScreen== 1){
                putCubeAtPosition(parent_cube_2,'l');

                parent_cube_2.GetComponent<Animator>().Play("level_1_right");
                parent_cube_1.GetComponent<Animator>().Play("level_2_right");

            }
            else if(cubeScreen== 2){
                putCubeAtPosition(parent_cube_1,'l');

                parent_cube_2.GetComponent<Animator>().Play("level_2_right");
                parent_cube_1.GetComponent<Animator>().Play("level_1_right");

            }
            else print("erreur cubeAtScreen in switch level");


            //bouton inclickable pendant l'animation (0.3f secondes)

            GameObject.Find("campagne_lvl_right").GetComponent<Button>().interactable = false;
            GameObject.Find("campagne_lvl_left").GetComponent<Button>().interactable = false;
            Invoke("reactiverbuttons",0.4f);

            //CHANGER TEXTE

            Invoke("moinsLvl",0.2f);


            //On peut à nouveau changer de niveau
        
            Invoke("canswitch",0.4f);
        }


    }
    private void canswitch(){
        canswitchLevel = true;
    }
    
    private void reactiverbuttons(){

        GameObject.Find("campagne_lvl_right").GetComponent<Button>().interactable = true;
        GameObject.Find("campagne_lvl_left").GetComponent<Button>().interactable = true;
    }

    public int cubeAtScreen(){
        if(parent_cube_1.transform.localPosition == new Vector3(0,0,0)){
            return 1;
        }
        else if(parent_cube_2.transform.localPosition == new Vector3(0,0,0)){
            return 2;
        }
        else return -1;
    }

    private void putCubeAtPosition(GameObject parentCube,char position){

        if(position == 'r'){ //right
            parentCube.transform.localPosition = new Vector3(1000,0,0);
        }
        else if(position=='l'){ //left
            parentCube.transform.localPosition = new Vector3(-1000,0,0);
        }
        else{
            print("erreur position invalide");
        }

    }

    private void plusLvl(){
        changeLevel('p');
        checkProgressValue();
    }
    private void moinsLvl(){
        changeLevel('m');
        checkProgressValue(); //ON MET LA BONNE VALEUR au score maximal
    }

    private void changeLevel(char mode){ //mode = 'p' : plus : lvl+1   ; mode = 'm' : moins : lvl-1

        
        string[] texteActuel = textLvl.GetComponent<TextMeshProUGUI>().text.Split("-");
        int mondeActuel = int.Parse(texteActuel[0]);
        int lvlActuel = int.Parse(texteActuel[1]);

        int newLvl = lvlActuel;
        int newMonde = mondeActuel;


       


        if(mode == 'p'){

            if(lvlActuel+1 <= nbLvlInWorld){
                newLvl = lvlActuel +1;
            }   
            else{
                if(mondeActuel+1 <= nbWorld){
                    newLvl = 1;
                    newMonde = mondeActuel + 1 ;
                }
                else{
                    newMonde= mondeActuel;
                    newLvl = lvlActuel;
                }
            }   
   
   
   }
        else if(mode == 'm'){
            
            if(lvlActuel-1 >= 1){
                newLvl = lvlActuel -1;
            }   
            else{
                if(mondeActuel-1 >= 1){
                    newLvl = nbLvlInWorld;
                    newMonde = mondeActuel - 1 ;
                }
                else{
                    newMonde= mondeActuel;
                    newLvl = lvlActuel;
                }
            }   

        }
        else{
            newMonde= mondeActuel;
            newLvl = lvlActuel;
            print("erreur 'changeLevel' mode incorrect");
        }


    //CHANGER LE TEXTE

    textLvl.GetComponent<TextMeshProUGUI>().text = newMonde + " - "+newLvl;



    }

    






    public void campagne_start_level(){

        //TROUVER LE NIVEAU QU'ON VA LANCER
        string[] texteActuel = textLvl.GetComponent<TextMeshProUGUI>().text.Split("-");
        int mondeActuel = int.Parse(texteActuel[0]);
        int lvlActuel = int.Parse(texteActuel[1]);


        //DESACTIVER LE CANVAS ET REMETTRE LA CAMERA A SA PLACE

        GameManager game = GameObject.Find("GameManager").GetComponent<GameManager>();

        game.invisibleVisibleForAnim('c','i');

        //Animations
        GameObject.Find("Main Camera").GetComponent<Animator>().Play("cam-90degres");  //tourner camera vers la gauche
        GameObject.Find("campagne").GetComponent<Animator>().Play("back_campagne");

        // //activer le canvas main mais le rendre invisible
        // game.activerCanvasMain();
        // GameObject.Find("main_game").transform.localScale = new Vector3(0,0,0);




        //DESACTIVER LE CANVAS CAMPAGNE
        game.desactiverCanvasCampagneMenu();

        //faire apparaitre progressbar

        GameObject.Find("campagne_progress").transform.localPosition = new Vector3(2000,800,0);

        //LANCER LE NIVEAU et appuyer sur "start"

        spawner spawn =GameObject.Find("Spawner").GetComponent<spawner>();
        

       

        spawn.startLevel(mondeActuel,lvlActuel);
        
        game.LaunchGame();//APPUYER SUR START
    }


    public void screenCampagne(int mode){ //mode = 0 : loose ; mode = 1 : win
        
        PlayerPrefs.SetInt("screenWinLooseOpen",1); //DIRE QUE LE SCREEN EST OUVERT POUR NE PAS RE EXECUTER ANIMATION OUVERTURE AVEC UPDATE GAME MANAGER

        GameObject.Find("Spawner").GetComponent<spawner>().StopAllInvoke();


        string nomLvl = GameObject.Find("campagne_name").GetComponent<TextMeshProUGUI>().text;

        //Sauvegarder score si meilleur que le précédent

        

        float valueActuelle = GameObject.Find("campagne_progress").GetComponent<Slider>().value;


        if(valueActuelle > PlayerPrefs.GetFloat(nomLvl,0)){
            PlayerPrefs.SetFloat(nomLvl,valueActuelle);
        }

        campagne_screen.SetActive(true);
        
        GameObject.Find("campagne_screen_progress").GetComponent<Slider>().maxValue = GameObject.Find("campagne_progress").GetComponent<Slider>().maxValue;
        GameObject.Find("campagne_screen_progress").GetComponent<Slider>().value = GameObject.Find("campagne_progress").GetComponent<Slider>().value;


        if(mode == 0)GameObject.Find("campagne_screen_title").GetComponent<TextMeshProUGUI>().text = "Game Over";
        else GameObject.Find("campagne_screen_title").GetComponent<TextMeshProUGUI>().text = "Completed";


        Invoke("animateScreen",1);

    }
    private void animateScreen(){
        campagne_screen.GetComponent<Animator>().Play("screen_push");
    }

    public void closeLevel(){
        
        GameObject.Find("campagne_menu").transform.localScale = new Vector3(1,1,1);

        campagne_screen.GetComponent<Animator>().Play("screen_back"); //close la pop up
        Invoke("invFctWithMode0",0.5f);
        
        

    }
    private void invFctWithMode0(){
        timedDisableAndOpenCampagneAndRespawn(0);
    }
    private void invFctWithMode1(){
        timedDisableAndOpenCampagneAndRespawn(1);
    }

    private void timedDisableAndOpenCampagneAndRespawn(int mode){//mode = 0 : home ; mode = 1 : restart

        campagne_screen.SetActive(false);
        //respawn player
        if(GameObject.FindWithTag("Player")==null){
            Instantiate(PlayerPrefab,new Vector3(0,-0.5000001f,0),Quaternion.identity,GameObject.Find("playerParent").transform);
        }
        Invoke("changementPlayerPrefs",0.2f);
        if(mode == 0){ //Pour home
            GameObject progress = GameObject.Find("campagne_progress");
            progress.transform.localPosition = new Vector3(0,-383,0); //changer position progress et actualiser valeur

            string nomLvl = GameObject.Find("campagne_name").GetComponent<TextMeshProUGUI>().text;
    

            progress.GetComponent<Slider>().value = PlayerPrefs.GetFloat(nomLvl,0);
            GameObject.Find("GameManager").GetComponent<GameManager>().openCampagne();
            swipeDetector.SetActive(true);
        }
        else if(mode == 1){// pour restart
            string[] texteActuel = GameObject.Find("campagne_name").GetComponent<TextMeshProUGUI>().text.Split("-");
            int mondeActuel = int.Parse(texteActuel[0]);
            int lvlActuel = int.Parse(texteActuel[1]);

            GameManager game = GameObject.Find("GameManager").GetComponent<GameManager>();
            spawner spawn =GameObject.Find("Spawner").GetComponent<spawner>();
            //LANCER LE NIVEAU et appuyer sur "start"

            spawn.startLevel(mondeActuel,lvlActuel);
            game.LaunchGame();//APPUYER SUR START
        }
        
    }



    private void changementPlayerPrefs(){
        PlayerPrefs.SetInt("screenWinLooseOpen",0);
    }

    public void restartLevel(){


        //TROUVER LE NIVEAU QU'ON VA LANCER DANS LA fonction qui va lancer
        //desactiver ScreenWinLoose

        campagne_screen.GetComponent<Animator>().Play("screen_back");
        Invoke("invFctWithMode1",0.5f); // = respawn + desactivation screenWInLoose ...


    }

    private void checkProgressValue(){
        string[] levelName = GameObject.Find("campagne_text_lvl_nb").GetComponent<TextMeshProUGUI>().text.Split("-");
        int w = int.Parse(levelName[0]);int l = int.Parse(levelName[1]); //pour supprimer les espaces
        string levelActu = w.ToString() + "-" + l.ToString();

        int dureeLvl = spawner.staticDureeNiveau(w,l);
        float scoreLvl = PlayerPrefs.GetFloat(levelActu,0);

        //actualiser la barre avec les valeurs:
        GameObject.Find("campagne_progress").GetComponent<Slider>().maxValue = dureeLvl;
        GameObject.Find("campagne_progress").GetComponent<Slider>().value = scoreLvl;

    }

    public void reinisialiserPrefs(){
        PlayerPrefs.SetFloat("1-1",0);
        PlayerPrefs.SetFloat("1-2",0);
        PlayerPrefs.SetFloat("1-3",0);
        PlayerPrefs.SetFloat("1-4",0);
        PlayerPrefs.SetFloat("1-5",0);
    }
    
    /*
    DID:

    quand on meurt dans un niveau
    barre de progression
    ecran de game over qui ramene au choix des niveaux
    ecran de réussite et noter réussite dans le menu des niveaux

    

    TODO:

    augmenter difficulté niveau + durée / ajouter niveaux

    baisser prix premiers bonus

    choix des niveaux en vu de tous les niveaux (en petit : une page = un monde)

    coffre tous les 3 niveaux : GOLD

    GAGNER beaucoup d'or dans les niveaux pour doubler à la fin (pieces de 5/10)
    
    (peut etre un bug : score normal qui s'augmente aussi = possibilité nouveau records sans jouer au mode normal = verifier)



    nouveau skin Player : couleur unie
    ajouter objets bonus dans le mode normal : bouclier ; double saut ; +100 pts ; 


    */




}
