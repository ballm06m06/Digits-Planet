using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;
using Facebook.MiniJSON;
     
public class FacebookLogin : MonoBehaviour {
    //disconnection sprite <-> connection sprite
    public Transform ConnectStatus;
   
    //to list fb things
    public GameObject ScoreEntryPanelPrefab;
    public GameObject ScrollScoreList;
    private GameObject scorePanel;
  //  private DatabaseReference reference;  
  
    //the list of facebook permissions
    List<string> Perms = new List<string>(){"public_profile","user_friends","email"}; 
    List<string> Publish = new List<string>(){"public_actions"}; 


    void Awake()
    {
        if (!FB.IsInitialized)
        {  
            // Initialize the Facebook SDK
            FB.Init(InitCompleteCallback, UnityCallback);    
        }
        else
        {
            FB.ActivateApp();   
        }



        if (FB.IsLoggedIn)
        {
            info(FB.IsLoggedIn);
        }
    }

   /* void Start () {
        // Set this before calling into the realtime database.
        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://digits-planet.firebaseio.com/");

        // Get the root reference location of the database.
        reference = FirebaseDatabase.DefaultInstance.RootReference;
    }*/

    public void LoginButton()
    {
        if(!FB.IsLoggedIn)
        {   //如果沒登入就讓它登入 
            //還有要做功能的話,把她寫在LoginCallback裡
            FB.LogInWithReadPermissions(Perms,LoginCallback);
        
        }
        else
        {
            // If in login status then logout
            FB.LogOut();

            PlayerPrefs.DeleteKey("Twice");
            StartCoroutine("CheckForSuccessfulLogout");
        }
    }
    IEnumerator CheckForSuccessfulLogout(){
        if (FB.IsLoggedIn)
        {
            yield return new WaitForSeconds(0.1f);
            StartCoroutine("CheckForSuccessfulLogout");
        }
        else
        {
            getCurrentFBConnectStatus();
        }
    }

   /* public void ShareLink()
    {
        // Post article on Facebook
        FB.ShareLink(new Uri("https://www.youtube.com"),"your title","is cool",null,ShareCallback);
    }*/
            
    public void info(bool IsLoggedIn)
    {
        if (IsLoggedIn)
        {
           // FB.API("/me?fields=name", HttpMethod.GET, DisplayUsername);
           // FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
                       
            FB.API("/app/scores?fields=score,user.limit(30)", HttpMethod.GET, getScoreCallback);
           // setScore();

            getCurrentFBConnectStatus();
        }

    }

    private void getCurrentFBConnectStatus(){  //connect=12, disconnect=7
        tk2dSprite icon = ConnectStatus.gameObject.GetComponent<tk2dSprite>();
       
        if (FB.IsLoggedIn)
            icon.spriteId = 12;
        else
            icon.spriteId = 7;           
    }


    #region callback

    private void LoginCallback(IResult result){
        if (result.Cancelled)
        {
            Debug.LogError("User Cancelled");
        }
        else
        {
           
            info(FB.IsLoggedIn);
            Debug.Log("Login successfully");
        }
    }

    private void ShareCallback(IShareResult shareResult){
        if (shareResult.Cancelled)
        {
            Debug.LogError("User Cancelled");
        }
        else
        {
            Debug.Log("Share successfully");
        }
    }
    private void InitCompleteCallback(){
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
            Debug.Log("Successfully init the fb sdk");
        }
        else
        {
            Debug.Log("Failed init");
        }
    }
    private void UnityCallback(bool isUnity){
        if (isUnity)
        {   //mean the fb canvas ppopup is not active
            Time.timeScale = 1;
        }
        else
        {   //FB POP PAUSED THE GAME
            // Time.timeScale = 0 EQUAL TO pause the game
            Time.timeScale = 0;
        }
    }
    public void getScoreCallback(IResult result)
    {
        //List<string> Publish = new List<string>(){"publish_actions"};


        IDictionary<string, object> data = result.ResultDictionary;
        List<object> scoreList = (List<object>)data["data"];
        int i = 1;

        //to prevent instatiate a prefab again,so clear first
        for (int j = 0; j < ScrollScoreList.transform.childCount; j++)
        {
            Destroy(ScrollScoreList.transform.GetChild(j).gameObject);
        }



        //Debug.Log("score submit result: " + result.RawResult);

        foreach (object obj in scoreList)
        {

            var entry = (Dictionary<string, object>)obj;
            var user = (Dictionary<string, object>)entry["user"];

            var scoreData = new Dictionary<string, string>();

            // Debug, List user permittion
          /*  List<string> permt = AccessToken.CurrentAccessToken.Permissions.ToList();
            foreach(object o in permt){
                Debug.Log(o);
            }  */
                                                                                      
            if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
            {   //Twice 等於預判別是否為第一次執行這個函數
                if (!PlayerPrefs.HasKey("Twice") || PlayerPrefs.GetInt("Twice") != 1)
                {
                    scoreData["score"] = entry["score"].ToString();
                    //write score
                    FB.API(user["id"].ToString() + "/scores", HttpMethod.POST, delegate(IGraphResult graphResult){}
                    , scoreData);
                }
             
            }
            if (AccessToken.CurrentAccessToken.Permissions.Contains("publish_actions"))
            {
                if (PlayerPrefs.HasKey("Twice") || PlayerPrefs.GetInt("Twice") == 1)
                { 
                    Debug.Log(AccessToken.CurrentAccessToken.UserId);
                
                    if (AccessToken.CurrentAccessToken.UserId == user["id"].ToString())
                    {
                        scoreData["score"] = (ScoreBoard.addScoreList[0] + ScoreBoard.subScoreList[0] + ScoreBoard.divScoreList[0]).ToString();
                        FB.API("me/scores", HttpMethod.POST, delegate(IGraphResult graphResult)
                            {
                                Debug.Log(scoreData["score"]);
                            }
                    , scoreData);
                    }
                    else
                    {
                        Debug.Log("T-ARA");
                        scoreData["score"] = entry["score"].ToString();
                    }
                }
            }
            scorePanel = (GameObject)Instantiate(ScoreEntryPanelPrefab);
            scorePanel.transform.SetParent(ScrollScoreList.transform, false);

            scorePanel.transform.Find("NumberText").GetComponent<Text>().text = i + ".";
            scorePanel.transform.Find("fbNameText").GetComponent<Text>().text = user["name"].ToString();
            scorePanel.transform.Find("ScoreText").GetComponent<Text>().text  = scoreData["score"].ToString();


            Transform friendpicture = scorePanel.transform.Find("fbImage");
            Image friendImage = friendpicture.GetComponent<Image>();

            FB.API(user["id"].ToString() + "/picture?width=120&height=120", HttpMethod.GET, delegate(IGraphResult PicResult)
                {
                    if (PicResult.Error != null)
                    {
                        Debug.Log(PicResult.RawResult);
                    }
                    else
                    {
                        friendImage.sprite = Sprite.Create(PicResult.Texture, new Rect(0, 0, 120, 120), new Vector2(0, 0));
                    }

                }); 

            Debug.Log("UserID="+user["id"].ToString());
            i++;
            Debug.Log("RawResult"+result.RawResult.ToString());   
                                                
        }
        PlayerPrefs.SetInt("Twice",1);

    }

      #endregion

}