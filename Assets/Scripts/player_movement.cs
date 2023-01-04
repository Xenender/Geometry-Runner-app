using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class player_movement : MonoBehaviour
{
    private Touch touch;
    public float speed;
    public float posXmin;
    public float posXmax;

    public Vector3 jump;
    public float jumpForce = 2.0f;
    public bool isGrounded;

    public float fallMultip = 2.5f;
    public float lowFallMultip = 2.0f;

    GameObject background_menu;

    public GameObject score;
    private GameObject shop;
    Rigidbody rb;

   



  
    int screenHeight;




    void Start(){
        Physics.gravity = new Vector3 (0,-15,0);



        background_menu = GameObject.Find("background_menu");
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);

        shop = GameObject.Find("shop");

        PlayerPrefs.SetInt("click_shop",0);

        

        screenHeight = Screen.height;
        
    }

    // Update is called once per frame
    void Update()
    {

                

       
        //Au moins un doigt sur l'écran
        if(Input.touchCount > 0){

//premier doigt posé
            touch = Input.GetTouch(0);

            if(touch.phase==TouchPhase.Moved  && background_menu.transform.localScale == new Vector3(0,0,0) && PlayerPrefs.GetInt("isPaused",0)==0 /*&& PlayerPrefs.GetInt("shop_open",0) != 1 && PlayerPrefs.GetInt("tools_open",0) != 1 && touch.position.y < (screenHeight/4)*3*/){
            
                
                
                transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speed * float.Parse(PlayerPrefs.GetString("sensi_multi","1,00")) ,transform.position.y,transform.position.z);
        
            }
            if(background_menu.transform.localScale == new Vector3(0,0,0)){
            if(transform.position.x > posXmax){
                transform.position = new Vector3(posXmax ,transform.position.y,transform.position.z);
            }
            else if(transform.position.x < posXmin){
                transform.position = new Vector3(posXmin,transform.position.y,transform.position.z);
            }
            }

        }
        else{ //0 doigts on saute
            if((background_menu.transform.localScale == new Vector3(0,0,0))&&isGrounded){
     

               
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                
                
                
                 
                isGrounded = false;
             }

        }
        if(rb.velocity.y < 0){
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMultip -1)*Time.deltaTime;

        }
        else if(rb.velocity.y > 0){
            rb.velocity += Vector3.up * Physics.gravity.y * (lowFallMultip -1)*Time.deltaTime;
        }

    
    }


    void OnCollisionStay()
         {
             isGrounded = true;
         }
    
}
