using UnityEngine;
     using System.Collections;
     
     public class saveMusic4 : MonoBehaviour {
     
         private GameObject[] music;
        
         void Start(){
            music = GameObject.FindGameObjectsWithTag ("gameMusic3");
            for(int i = 1; i<music.Length;i++){
                Destroy (music[i]);
            }
            
         }
         
         // Update is called once per frame
         void Awake () {
             DontDestroyOnLoad (transform.gameObject);
         }
     }