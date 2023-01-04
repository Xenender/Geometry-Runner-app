using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using UnityEngine.SceneManagement;

public class obstacles : MonoBehaviour
{
    public float speed = 10.0f;
    private GameObject text;
    public GameObject particleDead;
    public GameObject particleWin;
    private GameObject campagne_score = null;

    private Transform PosParticleWin;

    
    private GameObject player;

    

    void Awake(){
        
        }

    // Start is called before the first frame update
    void Start()
    {

        player = GameObject.FindWithTag("Player");
        text = GameObject.Find("score");
        PosParticleWin = GameObject.Find("PosParticleWin").transform;
       

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x,transform.position.y,transform.position.z - 0.008f);
        transform.Translate(Vector3.back * Time.deltaTime * speed); // * Time.deltaTime = vitesse pareille sur tout appareils
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if(collisionInfo.gameObject.tag == "Player")
        {
    
            Instantiate(particleDead,new Vector3(collisionInfo.gameObject.transform.position.x,collisionInfo.gameObject.transform.position.y+0.5f,collisionInfo.gameObject.transform.position.z),Quaternion.Euler(new Vector3(90, 0,0)));
            Destroy(collisionInfo.gameObject);
            Destroy(gameObject.GetComponent<Collider>());
            Destroy(gameObject.GetComponent<MeshRenderer>());
            gameObject.transform.localScale = new Vector3(0,0,0);
            CancelInvoke();

            //apres destruction : game manager
            
        
        
        }
        if(collisionInfo.gameObject.name == "Out")
        {
            if(GameObject.FindWithTag("Player")!=null){

                //jeux normal
                if(PlayerPrefs.GetInt("campagneOpen",0)==0){
                    int newScore = int.Parse(text.GetComponent<TextMeshProUGUI>().text) +1;
                    if(newScore%10 == 0){
                        Transform joueurPos = GameObject.FindWithTag("Player").transform;
                        iTween.PunchScale(text,new Vector3(2,2,2),1);
                        if(newScore%50 == 0) Instantiate(particleWin,new Vector3(joueurPos.position.x,joueurPos.position.y+1.5f,joueurPos.position.z),Quaternion.identity);
                    }
                    text.GetComponent<TextMeshProUGUI>().text = newScore.ToString();
                }
                //camapagne
                else if(PlayerPrefs.GetInt("campagneOpen",0)==1){
                    //si le score n'a pas été initialisé
                    if(campagne_score == null) campagne_score = GameObject.Find("campagne_score");
                    int newScore = int.Parse(campagne_score.GetComponent<TextMeshProUGUI>().text)+1;
                    if(newScore%50 == 0){
                        Transform joueurPos = GameObject.FindWithTag("Player").transform;
                        Instantiate(particleWin,new Vector3(joueurPos.position.x,joueurPos.position.y+1.5f,joueurPos.position.z),Quaternion.identity);
                    }
                    campagne_score.GetComponent<TextMeshProUGUI>().text = newScore.ToString();

                }

                

            }
            Destroy(gameObject);
        }   
        
        
         }

    public void setSpeed(float newSpeed){
        speed = newSpeed;
    }

}
