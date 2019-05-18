using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook.Unity;

public class RankRefreshControl : MonoBehaviour {


    public float swipeDistanceMin;
    public float swipeTimeMax;

    private Vector3 startPosition;
    private Vector3 endPosition;

    private float startTime;
    private float endTime;

    private float swipeDistance;
    private float swipeTime;
   
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        //For touch screen
        if (Input.touchCount > 0)                                           /*如果觸碰螢幕超過0次*/
        {
            Touch touch = Input.GetTouch(0);                                /*把觸碰螢幕的次數存在touch裡*/

            if (touch.phase == TouchPhase.Began)                            //觸碰剛開始,表示剛把手指放到螢幕上
            {
                startTime = Time.time;                                      //剛開始觸碰螢幕的時間,把那個time傳入startTime  
                startPosition = touch.position;                             //剛開始手指觸碰的位置
            }
            else if (touch.phase == TouchPhase.Ended)                       //觸碰結束
            {
                endTime = Time.time;                                        //確認剛離開螢幕的時間,把那個time傳入endTime  
                endPosition = touch.position;                               //結束手指離開觸碰的位置

                swipeDistance = (endPosition - startPosition).magnitude;    //.magnitude是一個計算量值的語法,可以偵測距離
                swipeTime = endTime - startTime;
                if (swipeDistance > swipeDistanceMin && swipeTime < swipeTimeMax) //trigger
                {
                    if ((endPosition.y - startPosition.y) < 0f)//往下划,to refresh
                    {
                      //  fb.SetScore();

                    }
                }
                // reset value
                startTime=0;
                endTime = 0;
                startPosition = Vector3.zero;
                endPosition = Vector3.zero;
                swipeDistance = 0;
                swipeTime = 0;
            }

        }
        #if UNITY_EDITOR
        //For mouse control
        if (Input.mousePresent) {
            if (Input.GetMouseButtonDown (0)) {                             //按下滑鼠左鍵
                //              print("OnMousePress");
                startTime = Time.time;                                      //確認按下去的時間，把那個time傳入startTime  
                startPosition = Input.mousePosition;                        //剛開始滑鼠按下去的位置
            } else if (Input.GetMouseButtonUp (0)) {                        //放開滑鼠左鍵
                //              print("OnMouseRelease");
                endTime = Time.time;                                        //確認放開的時間,把那個time傳入endTime  
                endPosition = Input.mousePosition;                          //結束滑鼠放開的位置

                swipeDistance = (endPosition - startPosition).magnitude;    //.magnitude是一個計算量值的語法,可以偵測距離
                //              print("swipeDistance"+swipeDistance);
                swipeTime = endTime - startTime;
                if (swipeDistance > swipeDistanceMin && swipeTime < swipeTimeMax) { //trigger
                    if ((endPosition.y - startPosition.y) < 0f)
                    {
                       // SetScore();.
                        FacebookLogin fb = GameObject.Find("GameManager").GetComponent<FacebookLogin>();
                        fb.info(FB.IsLoggedIn);
                    }
                   
                    // reset value
                    startTime=0;
                    endTime = 0;
                    startPosition = Vector3.zero;
                    endPosition = Vector3.zero;
                    swipeDistance = 0;
                    swipeTime = 0;
                }
            }
        }
        #endif

    }
    private void SetScore()
    {
       
        var scoreData = new Dictionary<string,string>();
        scoreData["score"] = ScoreBoard.addScoreList[0].ToString();

        FB.API("me/scores", HttpMethod.POST, delegate(IGraphResult graphResult){}
            , scoreData);

        Debug.Log(scoreData["score"].ToString());
        Debug.Log("swipe down babe");
       
      
    }
}
