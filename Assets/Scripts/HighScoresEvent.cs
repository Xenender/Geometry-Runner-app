using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HighScoresEvent : MonoBehaviour
{
//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité
    //URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité
    //URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité
    //URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité//URL DELETE par mesure de sécurité
    const string privateCode = "";  //Key to Upload New Info
    const string publicCode = "";   //Key to download
    const string webURL = ""; //  Website the keys are for

    public PlayerScoreEvent[] scoreList;
    DisplayHighscoresEvent myDisplay;

    static HighScoresEvent instance; //Required for STATIC usability
    void Awake()
    {
        instance = this; //Sets Static Instance
        myDisplay = GetComponent<DisplayHighscoresEvent>();
    }
    
    public static void UploadScore(string username, int score)  //CALLED when Uploading new Score to WEBSITE
    {//STATIC to call from other scripts easily
        instance.StartCoroutine(instance.DatabaseUpload(username,score)); //Calls Instance
    }

    IEnumerator DatabaseUpload(string userame, int score) //Called when sending new score to Website
    {
        UnityWebRequest www = new UnityWebRequest(webURL + privateCode + "/add/" + UnityWebRequest.EscapeURL(userame) + "/" + score);
        yield return www.SendWebRequest();

        if (string.IsNullOrEmpty(www.error))
        {
            print("Upload Successful");
            DownloadScores();
        }
        else print("Error uploading" + www.error);
    }

    public void DownloadScores()
    {
        StartCoroutine("DatabaseDownload");
    }
    IEnumerator DatabaseDownload()
    {
        //WWW www = new WWW(webURL + publicCode + "/pipe/"); //Gets the whole list
        UnityWebRequest www = new UnityWebRequest(webURL + publicCode + "/pipe/0/10"); //Gets top 10
        www.downloadHandler = new DownloadHandlerBuffer();
        yield return www.SendWebRequest();
        string info = www.downloadHandler.text;

        if (string.IsNullOrEmpty(www.error))
        {
            OrganizeInfo(info);
            myDisplay.SetScoresToMenu(scoreList);
        }
        else print("Error uploading" + www.error);
    }

    void OrganizeInfo(string rawData) //Divides Scoreboard info by new lines
    {
        string[] entries = rawData.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
        scoreList = new PlayerScoreEvent[entries.Length];
        for (int i = 0; i < entries.Length; i ++) //For each entry in the string array
        {
            string[] entryInfo = entries[i].Split(new char[] {'|'});
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            scoreList[i] = new PlayerScoreEvent(username,score);
            print(scoreList[i].username + ": " + scoreList[i].score);
        }
    }
}

public struct PlayerScoreEvent //Creates place to store the variables for the name and score of each player
{
    public string username;
    public int score;

    public PlayerScoreEvent(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}