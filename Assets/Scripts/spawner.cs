using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class spawner : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject obstacleDouble;

    public GameObject silver;
    public GameObject gold;

    public GameObject Orange;

    public GameObject arrows;
    public Transform[] pos;
    public Transform[] posDouble;

    public GameObject cacheVu;

    private GameObject background_menu;

    private float spawnSpeed = 1.0f;
    private int[] tabLastPos = {1,2,3,4};
    private int[] tabLastPosDouble = {1,2};
    private int[] tabLastPosDouble2 = {1,2,3};
    
    int doubleMid;
    private int indiceTab=0;
    private int indiceTabDouble=0;
    private bool launch;
    private bool doubleLaunch;
    private bool onlyDouble;

    public bool islevel=false;

    obstacles scriptObs;
    obstacles scriptObsDouble;

    private TextMeshProUGUI text;

    public Material sky;
    public Material ground;

    public Material skyBase;
    public Material groundBase;


    int dureeNiveau=0;

    void Start()
    {

        print("SPAWNER START");
        text = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        launch = false;
        background_menu = GameObject.Find("background_menu");
        doubleLaunch = false;
        onlyDouble = false;

        scriptObs = obstacle.GetComponent<obstacles>();
        scriptObsDouble = obstacleDouble.GetComponent<obstacles>();

        
        //InvokeRepeating("checkScore",0,0.2f); //comme update en plus lent

        //startLevel(1,1); //lancer un niveau

        doubleMid = 0;

    }

    public void stopInvokeCheckScore(){
        CancelInvoke("checkScore");
    }
    public void startInvokeCheckScore(){
        InvokeRepeating("checkScore",0,0.2f); //comme update en plus lent
    }
    

   public void spawnObstacle(){
    print("SPAWNER SPAWNOBSTACLE");
    
        int rand = Random.Range(0,19);
        if(doubleLaunch && rand == 0){//spawn double proba 1/20

            tabLastPosDouble[indiceTabDouble] = Random.Range(0,posDouble.Length); //Check pas la meme ligne tout le temps
            while(tabLastPosDouble[0] == tabLastPosDouble[1] || (tabLastPosDouble[indiceTabDouble]==1 && PlayerPrefs.GetInt("event",0)==1)){
            tabLastPosDouble[indiceTabDouble] = Random.Range(0,pos.Length);
            }

            Vector3 arrPos = new Vector3(posDouble[tabLastPosDouble[indiceTabDouble]].position.x,-1,posDouble[tabLastPosDouble[indiceTabDouble]].position.z-1.3f);

            var parent = Instantiate(obstacleDouble,posDouble[tabLastPosDouble[indiceTabDouble]].position,Quaternion.identity);
            if(tabLastPosDouble[indiceTabDouble]==1){//gauche
                Instantiate(arrows,arrPos,Quaternion.Euler(0,0,90),parent.transform); //parent = obstacle

            }
            
            indiceTabDouble +=1;
            if(indiceTabDouble>1) indiceTabDouble = 0;
        }
        else{ //cube  normal + gold?


            tabLastPos[indiceTab] = Random.Range(0,pos.Length); //Check pas la meme ligne tout le temps
            while(tabLastPos[0] == tabLastPos[1] && tabLastPos[1] == tabLastPos[2] && tabLastPos[2] == tabLastPos[3]){
            tabLastPos[indiceTab] = Random.Range(0,pos.Length);
            }

            var parentCoin = Instantiate(obstacle,pos[tabLastPos[indiceTab]].position, Quaternion.identity); //spawn simple

            if(PlayerPrefs.GetInt("coin_active",1)==1){
            int randCoin = Random.Range(0,19); /////////////////////////////
            if(randCoin == 0){
                
                Vector3 posCoin = new Vector3(pos[tabLastPos[indiceTab]].position.x,-0.5f,pos[tabLastPos[indiceTab]].position.z);
                if(int.Parse(text.text)<400){//spawn silver

                    Instantiate(silver,posCoin,Quaternion.Euler(90,0,0),parentCoin.transform);
                }
                else if(int.Parse(text.text)>=400 && int.Parse(text.text)<2000){ //spawn Gold
                    Instantiate(gold,posCoin,Quaternion.Euler(90,0,0),parentCoin.transform);
                }

                else{//spawn Orange
                    Instantiate(Orange,posCoin,Quaternion.Euler(90,0,0),parentCoin.transform);
                }
            }}

            indiceTab +=1;
            if(indiceTab>3) indiceTab = 0;
        }


        if(!onlyDouble)Invoke("spawnObstacle",spawnSpeed); //REPEAT

   }

   public void spawnOnlyDouble(){
    
      

            tabLastPosDouble2[indiceTabDouble] = Random.Range(0,posDouble.Length); //Check pas la meme ligne tout le temps
            while((tabLastPosDouble2[0] == tabLastPosDouble2[1] && tabLastPosDouble2[1] == tabLastPosDouble2[2]) || (tabLastPosDouble2[indiceTabDouble] == 1 && doubleMid >=0)){
            tabLastPosDouble2[indiceTabDouble] = Random.Range(0,pos.Length);
            }

            Vector3 arrPos = new Vector3(posDouble[tabLastPosDouble2[indiceTabDouble]].position.x,-1,posDouble[tabLastPosDouble2[indiceTabDouble]].position.z-1.3f);

            var parent = Instantiate(obstacleDouble,posDouble[tabLastPosDouble2[indiceTabDouble]].position,Quaternion.identity);
            if(tabLastPosDouble2[indiceTabDouble]==1){//gauche
                Instantiate(arrows,arrPos,Quaternion.Euler(0,0,90),parent.transform); //parent = obstacle
                doubleMid = 2;

            }

            if(PlayerPrefs.GetInt("coin_active",1)==1){
            int randCoin = Random.Range(0,19); /////////////////////////////
            if(randCoin == 0){
                
                Vector3 posCoin = new Vector3(posDouble[tabLastPosDouble2[indiceTabDouble]].position.x,-0.5f,posDouble[tabLastPosDouble2[indiceTabDouble]].position.z);

                Transform newTrans = parent.transform;
                newTrans.localScale = new Vector3(1,1,1);

                if(int.Parse(text.text)<400){//spawn silver

                    Instantiate(silver,posCoin,Quaternion.Euler(90,0,0),parent.transform);
                }
                else if(int.Parse(text.text)>=400 && int.Parse(text.text)<2000){ //spawn Gold
                    Instantiate(gold,posCoin,Quaternion.Euler(90,0,0),parent.transform);
                }

                else{//spawn Orange
                    Instantiate(Orange,posCoin,Quaternion.Euler(90,0,0),parent.transform);
                }
            }}
            
            indiceTabDouble +=1;
            if(indiceTabDouble>2) indiceTabDouble = 0;


        doubleMid-=1;
        
        Invoke("spawnOnlyDouble",spawnSpeed); //REPEAT

   }

   public void spawnEvent1(){

   }

    void checkScore(){

        print("check");

        if(background_menu.transform.localScale == new Vector3(0,0,0) && !launch){

            if(PlayerPrefs.GetInt("event",0)==1){
                launch=true;
                Invoke("spawnObstacle",1);
            }
            else{

            if(!onlyDouble){
                launch = true;
                Invoke("spawnObstacle",1); 
            }
            else{
                
                launch = true;
                Invoke("spawnOnlyDouble",1);
            }
            }
            
        }

        int intscore = int.Parse(text.text);


        if(PlayerPrefs.GetInt("event",0)==1){
            doubleLaunch = true;
            onlyDouble = false;
            if(intscore<100){

            spawnSpeed = 0.3f;
            scriptObs.setSpeed(30.0f);
            scriptObsDouble.setSpeed(30.0f);
        }

        else if(intscore>=100 && intscore <200){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,17);

        }
        else if(intscore>=200 && intscore < 300){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,15);
        }
        else if(intscore>=300 && intscore < 400){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,14);
        }
        else if(intscore>=400 && intscore < 500){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,13);
        }
        else if(intscore>=500 && intscore < 600){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,12);
        }
        else if(intscore>=600 && intscore < 700){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,11);
        }
        else if(intscore>=700 && intscore < 800){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,10);
        }
        else if(intscore>=800 && intscore < 900){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,9);
        }
        else if(intscore>=900 && intscore < 1000){
            GameObject obj =GameObject.Find("event1_obs");
            obj.transform.position = new Vector3(obj.transform.position.x,obj.transform.position.y,8);
        }
        


        }
        else{

        if(intscore <6000){
            cacheVu.SetActive(false);
                GameObject.Find("Skybox").GetComponent<MeshRenderer>().material = skyBase;
                GameObject.Find("Ground").GetComponent<MeshRenderer>().material = groundBase;
        }

        if(intscore <4000){
            if(onlyDouble){
                launch = false;
                onlyDouble = false;
            }
        }

        if(intscore >= 4000 && intscore < 6000){
            if(!onlyDouble){
                launch = false;
                onlyDouble = true;
            }
        }
        if(intscore >= 6000){
            if(cacheVu.activeSelf == false){
                launch = false;
                onlyDouble = false;
                cacheVu.SetActive(true);
                GameObject.Find("Skybox").GetComponent<MeshRenderer>().material = sky;
                GameObject.Find("Ground").GetComponent<MeshRenderer>().material = ground;
                
            }
        }


        if(intscore<10){
            scriptObs.setSpeed(20.0f);
            scriptObsDouble.setSpeed(20.0f);
        }

        else if(intscore>=10 && intscore <30){
            spawnSpeed = 0.8f;
            scriptObs.setSpeed(20.0f);
            scriptObsDouble.setSpeed(20.0f);

        }
        else if(intscore>=30 && intscore <100){
            doubleLaunch = true;

            spawnSpeed = 0.6f;
            scriptObs.setSpeed(20.0f);
            scriptObsDouble.setSpeed(20.0f);
        }
        else if(intscore>=100 && intscore <200){
            scriptObs.setSpeed(23.0f);
            spawnSpeed = 0.6f;
            scriptObsDouble.setSpeed(23.0f);
            doubleLaunch = true;
            }
        else if(intscore>=200 && intscore < 400){
            spawnSpeed = 0.4f;
            scriptObs.setSpeed(23.0f);
            scriptObsDouble.setSpeed(23.0f);
            doubleLaunch = true;
        }
        else if(intscore>=400 && intscore < 600){
            
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(23.0f);
            scriptObsDouble.setSpeed(23.0f);
            doubleLaunch = true;
    
        }
        else if(intscore>=600 && intscore < 800){
            scriptObs.setSpeed(26.0f);
            spawnSpeed = 0.3f;
            scriptObsDouble.setSpeed(26.0f);
            doubleLaunch = true;
        }
        else if(intscore>=800&&intscore<1000){
            scriptObs.setSpeed(28.0f);
            spawnSpeed = 0.3f;
            scriptObsDouble.setSpeed(28.0f);
            doubleLaunch = true;
        }
        else if(intscore >=1000 && intscore < 2000){
            scriptObs.setSpeed(30.0f);
            spawnSpeed = 0.2f;
            scriptObsDouble.setSpeed(30.0f);
            doubleLaunch = true;

        }
        else if(intscore >=2000 && intscore < 2300){
            

            scriptObs.setSpeed(33.0f);
            scriptObsDouble.setSpeed(33.0f);
            spawnSpeed = 0.2f;
            doubleLaunch = true;
        }
        else if(intscore >=2300 && intscore < 2600){
            

            scriptObs.setSpeed(36.0f);
            scriptObsDouble.setSpeed(36.0f);
            spawnSpeed = 0.2f;
            doubleLaunch = true;
        }
        else if(intscore >=2600 && intscore < 2900){
            

            scriptObs.setSpeed(38.0f);
            scriptObsDouble.setSpeed(38.0f);
            spawnSpeed = 0.2f;
            doubleLaunch = true;
        }
        else if(intscore >=2900 && intscore < 3500){
            

            scriptObs.setSpeed(40.0f);
            scriptObsDouble.setSpeed(40.0f);
            spawnSpeed = 0.2f;
            doubleLaunch = true;
        }
        else if(intscore >=3500 && intscore < 4000){
            

            scriptObs.setSpeed(43.0f);
            scriptObsDouble.setSpeed(43.0f);
            spawnSpeed = 0.2f;
            doubleLaunch = true;
        }
        else if(intscore >=4000 && intscore < 4050){


            spawnSpeed = 0.8f;
            scriptObs.setSpeed(20.0f);
            scriptObsDouble.setSpeed(20.0f);
            CancelInvoke("spawnObstacle");
            


        }




        else if(intscore>=4050 && intscore <4100){
            spawnSpeed = 0.6f;
            scriptObs.setSpeed(20.0f);
            scriptObsDouble.setSpeed(20.0f);

        }
        else if(intscore>=4100 && intscore <4150){
            

            scriptObs.setSpeed(23.0f);
            spawnSpeed = 0.6f;
            scriptObsDouble.setSpeed(23.0f);
        }
        else if(intscore>=4150 && intscore <4250){
            spawnSpeed = 0.4f;
            scriptObs.setSpeed(23.0f);
            scriptObsDouble.setSpeed(23.0f);
           
            }
        else if(intscore>=4250 && intscore < 4350){
            spawnSpeed = 0.4f;
            scriptObs.setSpeed(26.0f);
            scriptObsDouble.setSpeed(26.0f);
           
        }
        else if(intscore>=4350 && intscore < 4500){
            
            scriptObs.setSpeed(28.0f);
            spawnSpeed = 0.4f;
            scriptObsDouble.setSpeed(28.0f);
            
    
        }
        else if(intscore>=4500 && intscore < 4600){
            scriptObs.setSpeed(30.0f);
            spawnSpeed = 0.4f;
            scriptObsDouble.setSpeed(30.0f);
            
        }
        else if(intscore>=4600&&intscore<4800){
            
           scriptObs.setSpeed(33.0f);
            spawnSpeed = 0.4f;
            scriptObsDouble.setSpeed(33.0f);
        }
        else if(intscore>=4800&&intscore<5000){
            
           scriptObs.setSpeed(33.0f);
            spawnSpeed = 0.3f;
            scriptObsDouble.setSpeed(33.0f);
        }
        else if(intscore>=5000&&intscore<5300){
            
            scriptObs.setSpeed(36.0f);
            spawnSpeed = 0.3f;
            scriptObsDouble.setSpeed(36.0f);
        }
        else if(intscore>=5300&&intscore<5600){
            
           scriptObs.setSpeed(39.0f);
            spawnSpeed = 0.3f;
            scriptObsDouble.setSpeed(39.0f);
        }
        else if(intscore>=5600&&intscore<6000){
            
           scriptObs.setSpeed(44.0f);
            spawnSpeed = 0.25f;
            scriptObsDouble.setSpeed(44.0f);
        }






        else if(intscore>=6000&&intscore<6150){
            
         CancelInvoke("spawnOnlyDouble");
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(30.0f);
            scriptObsDouble.setSpeed(30.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,25);
        }
        else if(intscore>=6150&&intscore<6300){
            
            
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(30.0f);
            scriptObsDouble.setSpeed(30.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,23);
        }
        else if(intscore>=6300&&intscore<6500){
            
          
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(30.0f);
            scriptObsDouble.setSpeed(30.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,20);
        }
        else if(intscore>=6500&&intscore<6700){
            
       
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(30.0f);
            scriptObsDouble.setSpeed(30.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,18);
        }
        else if(intscore>=6700&&intscore<6900){
           
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(32.0f);
            scriptObsDouble.setSpeed(32.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,17);
        }
        else if(intscore>=6900&&intscore<7100){
            
         
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(32.0f);
            scriptObsDouble.setSpeed(32.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,16);
        }
        else if(intscore>=7100&&intscore<7300){
            
            
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(32.0f);
            scriptObsDouble.setSpeed(32.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,15);
        }
        else if(intscore>=7300&&intscore<7500){
            
           
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(32.0f);
            scriptObsDouble.setSpeed(32.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,14);
        }
        else if(intscore>=7500&&intscore<7700){
            
           
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(32.0f);
            scriptObsDouble.setSpeed(32.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,13);
        }
        else if(intscore>=7700&&intscore<8000){
            
            
            spawnSpeed = 0.3f;
            scriptObs.setSpeed(32.0f);
            scriptObsDouble.setSpeed(32.0f);
            cacheVu.transform.position = new Vector3(cacheVu.transform.position.x,cacheVu.transform.position.y,12);
        }
        
        

        }

    }


    private void stopLevel(){
        CancelInvoke("spawnObstacle");
        CancelInvoke("spawnOnlyDouble");
        Invoke("winScreen",3f);
        
    }

    private void winScreen(){
        if(GameObject.FindWithTag("Player") != null){ //si joueur encore en vie
            GameObject.Find("CampagneManager").GetComponent<CampagneManager>().screenCampagne(1); //appel fonction victoire
        }
    }

    public void startLevel(int world,int level){

        print("SPAWNER START LEVEL");

        dureeNiveau = staticDureeNiveau(world,level);
        GameObject.Find("campagne_name").GetComponent<TextMeshProUGUI>().text = world.ToString() + "-" + level.ToString(); //pour recuperer le nom du niveau au moment de loose ou win

        
        switch (world){
            
            case 1: //WORLD 1

                switch(level){

                    case 1: //LEVEL 1-1

                       
                        spawnSpeed = 0.4f;
                        scriptObs.setSpeed(23.0f);
                        scriptObsDouble.setSpeed(23.0f);
                        doubleLaunch = true;

                        launch = true;
                        Invoke("spawnObstacle",1);
                        Invoke("stopLevel",dureeNiveau+1);


                        break;

                    case 2: //LEVEL 1-2

                       

                        scriptObs.setSpeed(26.0f);
                        spawnSpeed = 0.3f;
                        scriptObsDouble.setSpeed(26.0f);
                        doubleLaunch = true;

                        launch = true;
                        Invoke("spawnObstacle",1);
                        Invoke("stopLevel",dureeNiveau+1);
                        break;

                    
                    case 3: //LEVEL 1-3

                      

                        scriptObs.setSpeed(28.0f);
                        spawnSpeed = 0.3f;
                        scriptObsDouble.setSpeed(28.0f);
                        doubleLaunch = true;

                        launch = true;
                        Invoke("spawnObstacle",1);
                        Invoke("stopLevel",dureeNiveau+1);
                        break;

                        
                    case 4: //LEVEL 1-4

                      

                        scriptObs.setSpeed(30.0f);
                        spawnSpeed = 0.2f;
                        scriptObsDouble.setSpeed(30.0f);
                        doubleLaunch = true;

                        launch = true;
                        Invoke("spawnObstacle",1);
                        Invoke("stopLevel",dureeNiveau+1);
                        break;


                    case 5: //LEVEL 1-5

                        

                        scriptObs.setSpeed(33.0f);
                        scriptObsDouble.setSpeed(33.0f);
                        spawnSpeed = 0.2f;
                        doubleLaunch = true;

                        launch = true;
                        Invoke("spawnObstacle",1);
                        Invoke("stopLevel",dureeNiveau+1);
                        break;



                    default:
                        dureeNiveau=0;
                        print("error level indice");
                        break;
                }




                break;
            
            default:
                dureeNiveau=0;
                print("error world indice");
                break;

        }
        
        

        sliderMaxValueAndReset(dureeNiveau);

        Invoke("incrementProgressBar",0.1f);

    }

    private void incrementProgressBar(){
        Slider slider = GameObject.Find("campagne_progress").GetComponent<Slider>();
        if(slider.value < (dureeNiveau+3) && GameObject.FindWithTag("Player") != null){
            slider.value += 0.1f;
            Invoke("incrementProgressBar",0.1f);
        }
        else{
            //niveau fini
        }
        
    }
    private void sliderMaxValueAndReset(int maxvalue){
        Slider s = GameObject.Find("campagne_progress").GetComponent<Slider>();
        s.maxValue = maxvalue;
        s.value = 0.0f;
    }


    public static int staticDureeNiveau(int world, int level){

        int duree=0;

        switch (world){
            
            case 1: //WORLD 1

                switch(level){

                    case 1: //LEVEL 1-1

                        duree = 40;


                        break;

                    case 2: //LEVEL 1-2

                        duree = 40;

                        break;

                    
                    case 3: //LEVEL 1-3

                        duree = 40;

                        break;

                        
                    case 4: //LEVEL 1-4

                        duree = 40;

                        break;


                    case 5: //LEVEL 1-5

                        duree = 50;


                        break;

                    case 6: //LEVEL 1-2

                        duree = 50;

                        break;

                    
                    case 7: //LEVEL 1-3

                        duree = 60;

                        break;

                        
                    case 8: //LEVEL 1-4

                        duree = 70;

                        break;


                    case 9: //LEVEL 1-5

                        duree = 80;


                        break;



                    default:
                        print("error level indice");
                        break;
                }




                break;
            
            default:

                print("error world indice");
                break;

        }

        return duree;
    }
    
    public void StopAllInvoke(){
        CancelInvoke("checkScore");
        CancelInvoke("incrementProgressBar");
        CancelInvoke("stopLevel");
        CancelInvoke("spawnOnlyDouble");
        CancelInvoke("spawnObstacle");
        CancelInvoke();
        }

    
}
