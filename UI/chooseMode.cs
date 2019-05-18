using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using Facebook.Unity;
using UnityEngine.UI;
            //選星球的
public class chooseMode : MonoBehaviour {

    public static InputField inputfield;
   
	public Transform maps;
	public Transform planet;
	public Transform arrow;
	public Transform startMenu;
    public Transform UserNameInput;
    public Transform Ranking;
    public Transform FacebookRanking;
	public Transform nameSprite;
	public Transform inputField;
	public Transform rank;
    public Transform ItemList;
    public Transform SettingPad;
    public Transform InformationPad;
    public Transform ChestPad;  
    public Transform ChestInfoPad;
    public Transform ScrollView;
    public Transform heart1, heart2, heart3;

    public tk2dSprite AddRankingsprite;
    public tk2dSprite SubRankingsprite;
    public tk2dSprite DivRankingsprite;
    public Text NameText;
    public Text ExperienceText;


	//為方便直接用scene執行設3, 要記得改回0!!!
	public static int setDifficulty = 3;	// 1=Hard, 2=Mid, 3=Easy
	public static string setGameMode =  null;		// default=null

	public static bool isGameLoaded = false;
  
    private string PlayerSaveFileName = "inputText";

    public string getPlayerSaveFileName
    {
        get
        {
            return PlayerSaveFileName;
        }
    }
		
	float time;
	int count;
	Transform chp;
	bool inRanking = false;

	public float exitTime = 1;
	private static int currentActivePortrait = -1;
	private int currentIndex;
		 
    void Start () {
        // DO NOT REMOVE THE CODE BELOW
        Screen.orientation = ScreenOrientation.Portrait;
        //
		currentIndex = planet.GetComponent<SwipeControl> ().CurrentChoice;

		chp = nameSprite.FindChild ("ChangeHeadPad");
		if (chp == null)
			Debug.LogError ("Unable to load ChangeHeadPad");
		//先把地圖每一個物件關掉
		for (int i = 0; i < maps.childCount; i++) {
			maps.GetChild (i).gameObject.SetActive (false);
		}

        if (!isGameLoaded)//遊戲開啟，進入tapToStart畫面
        {   //唯一一個被開啟的
            startMenu.gameObject.SetActive(true);
            //其他都是false
            planet.gameObject.SetActive(false);
            maps.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
			nameSprite.gameObject.SetActive (false);
			Ranking.gameObject.SetActive (false);
            UserNameInput.gameObject.SetActive (false);
            ItemList.gameObject.SetActive(false);
            SettingPad.gameObject.SetActive(false);

			//Canvus
			rank.gameObject.SetActive (false);
			inputField.gameObject.SetActive (false);
            ScrollView.gameObject.SetActive(false);
            NameText.gameObject.SetActive(false);
            getCurrentPortrait();

            if (PlayerPrefs.HasKey("isFirstTime"))// Not first time to excute the game
                //這裡會讓名字顯現出來(在startMenu)
                GetPlayerName();
            print("遊戲開啟，進入tapToStart畫面");
        }
        else//玩完遊戲回到Lobby
        {
			TapToStartPressed ();
			LastPlanetSelected ();
            GetExperience();
            setPortrait();
            print("玩完遊戲回到Lobby");
        }
    }

	void Update(){

		// Double tab to escape
		time += Time.deltaTime;
		if (Input.GetKeyDown (KeyCode.Escape)) //Android 的返回鍵
		{
			count++;
			if (time <= exitTime && count == 2) {
				Application.Quit ();
				#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying=false;
				#endif
			}
			time = 0f;
			if (!inRanking) {
				if (!startMenu.gameObject.activeSelf) {
                    if (planet.gameObject.activeSelf)
                        ReturnToTaptoStartScreen();
                    else if (Ranking.gameObject.activeSelf)
                        OKButtonPressed();
                    else if (FacebookRanking.gameObject.activeSelf)
                        OKButtonPressed();
                    else if (ChestPad.gameObject.activeSelf)
                        ChestPadReturn();
                    else if (SettingPad.gameObject.activeSelf)
                        SettingPadCancelled();
                    else if (InformationPad.gameObject.activeSelf)
                        InformationPadCancelled();
                    else
						ReturnButtonPressed ();
				}
			}
		}
		if (time > exitTime)
			count = 0;

    }

	void AdditionMode(tk2dUIItem called){
		switch (called.name) {

        case "map3-1":
//			print ("map3-1 clicked, Hard");
            setDifficulty = 1;
			break;
		case "map3-2":
//			print ("map3-2 clicked, Medium");
			setDifficulty = 2;
			break;
		case "map3-3":
//			print ("map3-3 clicked, Easy");
			setDifficulty = 3;
			break;
		default:
			Debug.LogError ("Cannot find object");
			setDifficulty = 0;
			break;
		}
//      GameObject.DontDestroyOnLoad (gameObject);
		SceneManager.LoadScene ("Addition", LoadSceneMode.Single);
	}

	void substractionMode(tk2dUIItem called){
		
		switch (called.name) {

		case "map4-1":
//			print ("map4-1 clicked, Hard");
			setDifficulty = 1;
			break;
		case "map4-2":
//			print ("map4-2 clicked, Medium");
			setDifficulty = 2;
			break;
		case "map4-3":
//			print ("map4-3 clicked, Easy");
			setDifficulty = 3;
			break;
		default:
			Debug.LogError ("Cannot find object");
			setDifficulty = 0;
			break;
		}
//		GameObject.DontDestroyOnLoad (gameObject);
		SceneManager.LoadScene ("Subtraction", LoadSceneMode.Single);
	}

	void divisionMode(tk2dUIItem called){
		switch (called.name) {

		case "map6-1":
//			print ("map6-1 clicked, Hard");
			setDifficulty = 1;
			break;
		case "map6-2":
//			print ("map6-2 clicked, Medium");
			setDifficulty = 2;
			break;
		case "map6-3":
//			print ("map6-3 clicked, Easy");
			setDifficulty = 3;
			break;
		default:
			Debug.LogError ("Cannot find object");
			setDifficulty = 0;
			break;
		}
//		GameObject.DontDestroyOnLoad (gameObject);
        SceneManager.LoadScene ("Division", LoadSceneMode.Single);
	}


	void TapToStartPressed(){
		startMenu.gameObject.SetActive (false);

		// To check if it's our first time to load game
		if (!PlayerPrefs.HasKey ("isFirstTime") || PlayerPrefs.GetInt ("isFirstTime") != 1) { //第一次開啟遊戲//換成apk時要測試
			UserNameInput.gameObject.SetActive (true);
			inputField.gameObject.SetActive (true);
			PlayerPrefs.SetInt ("isFirstTime", 1);
		}
        else
        {	//直接進入planet選單
            planet.gameObject.SetActive(true);
            arrow.gameObject.SetActive(true);
			nameSprite.gameObject.SetActive (true);
            ItemList.gameObject.SetActive(true);
            GetPlayerName();
            GetExperience();
            ExperienceText.gameObject.SetActive(true);
        }
	}

	void LastPlanetSelected(){

		SwipeControl sc = GameObject.Find ("PlanetChoice").GetComponent<SwipeControl> ();
		if (sc == null) {
			Debug.LogError ("Unable to get script.");
			return;
		}

		
        switch (setGameMode) {
		case "Addition":
			//預設就會到，不做事
			break;
        case "Division":
			//上划兩個
            sc.swipeAfter();
            sc.swipeAfter();
			break;
		case "Subtraction":
			//上划一個
            sc.swipeAfter ();
            break;
		default:
			Debug.Log ("Unable to get last planet set.");
			break;
		} 
		setGameMode = null; 

	}

	//
	// Name Input Field
	//
    void YesButtonPressed()//輸入名字的inputField
    {  
        
        inputfield = GameObject.Find("InputField").GetComponent<InputField>();
       
		if (inputfield.text != "" && inputfield != null) 
		{ 
			NameText.text = inputfield.text;	//設定頭像名字
            PlayerPrefs.SetString (PlayerSaveFileName, inputfield.text);				//名字存檔
//			print (inputfield.text);	//Debug
           
		} else   //未輸入名字
		{
//			print("End of function");	//Debug
			return; //結束函數
		}			

        UserNameInput.gameObject.SetActive(false);
        planet.gameObject.SetActive(true);
        arrow.gameObject.SetActive(true);
        ItemList.gameObject.SetActive(true);

        inputfield.enabled = false;
		//載入影片
		SceneManager.LoadScene ("StartAnimation", LoadSceneMode.Single);
    }

    void GetPlayerName()
    {
        if (PlayerPrefs.HasKey(PlayerSaveFileName))
        {
			NameText.text = PlayerPrefs.GetString(PlayerSaveFileName);	//設定頭像名字
            NameText.gameObject.SetActive(true);
        }
        else
            Debug.LogError("Unable to load saved data!");
    }

    public void GetExperience()// 可以改成回傳int做更多應用
    {
        SwipeControl sc = GameObject.Find ("PlanetChoice").GetComponent<SwipeControl> ();
        if (sc == null) {
            Debug.LogError ("Unable to get script.");
            return;
        }
        switch (sc.CurrentChoice) {
            case 0:
                ExperienceText.text = LevelControl.expAdd.ToString();

                break;
            case 1:
                ExperienceText.text = LevelControl.expSub.ToString();

                break;
            case 2:
                ExperienceText.text = LevelControl.expDiv.ToString();
               
                break;
            default:
                Debug.Log ("Unable to get last planet set.");
                break;
        } 
        setGameMode = null; 
    }
    void NoButtonPressed()
    {
        startMenu.gameObject.SetActive (true);
        UserNameInput.gameObject.SetActive(false);
		inputField.gameObject.SetActive(false);
		PlayerPrefs.DeleteKey("isFirstTime");
    }
	// Return to planet choose
	void ReturnButtonPressed(){
		maps.GetChild (currentIndex).gameObject.SetActive (false);
		planet.gameObject.SetActive (true);
//		print ("return button pressed, currentIndex=" + currentIndex);
		arrow.gameObject.SetActive(true);
		nameSprite.gameObject.SetActive (true);
        NameText.gameObject.SetActive(true);
        ItemList.gameObject.SetActive(true);
        ExperienceText.gameObject.SetActive(true);
    }
	// Return to Tap to start screen
	void ReturnToTaptoStartScreen(){
		startMenu.gameObject.SetActive (true);
		planet.gameObject.SetActive (false);
		arrow.gameObject.SetActive (false);
		nameSprite.gameObject.SetActive (false);
		for (int i = 0; i < maps.childCount; i++) {
			maps.GetChild (i).gameObject.SetActive (false);
		}
        NameText.gameObject.SetActive(false);
        ExperienceText.gameObject.SetActive(false);

        ItemList.gameObject.SetActive(false);
	}
	void ChooseDifficulty(){
		currentIndex = planet.GetComponent<SwipeControl> ().CurrentChoice;
//		print ("chooseDifficulty button pressed, currentIndex=" + currentIndex);
		planet.gameObject.SetActive (false);
		maps.GetChild (currentIndex).gameObject.SetActive (true);
		maps.gameObject.SetActive (true);
		arrow.gameObject.SetActive (false);
		nameSprite.gameObject.SetActive (false);
        NameText.gameObject.SetActive(false);
        ExperienceText.gameObject.SetActive(false);
        ItemList.gameObject.SetActive(false);
	}

	//
	// Ranking
	//
    void RankPressed(tk2dUIItem pressed)
    {
            
            planet.gameObject.SetActive(false);
            arrow.gameObject.SetActive(false);
            nameSprite.gameObject.SetActive(false);
            NameText.gameObject.SetActive(false);
            ExperienceText.gameObject.SetActive(false);
            ItemList.gameObject.SetActive(false);
           
        if (!FB.IsLoggedIn)
        {
            Ranking.gameObject.SetActive(true);
            rank.gameObject.SetActive(true);
            ScrollView.gameObject.SetActive(false);
            FacebookRanking.gameObject.SetActive(false);

            switch (pressed.name)
            {
                case "AddSprite": 
                // Call Add Ranking
                    rank.gameObject.GetComponent<RankControl>().LoadScore("Add");
                    AddRankingsprite.gameObject.SetActive(false);
                    SubRankingsprite.gameObject.SetActive(true);
                    DivRankingsprite.gameObject.SetActive(true);
                    break;
                case "SubSprite":
                // Call Sub Ranking
                    rank.gameObject.GetComponent<RankControl>().LoadScore("Sub");
                    AddRankingsprite.gameObject.SetActive(true);
                    SubRankingsprite.gameObject.SetActive(false);
                    DivRankingsprite.gameObject.SetActive(true);
                    break;
                case "DivSprite":
                // Call Div Ranking
                    rank.gameObject.GetComponent<RankControl>().LoadScore("Div");
                    AddRankingsprite.gameObject.SetActive(true);
                    SubRankingsprite.gameObject.SetActive(true);
                    DivRankingsprite.gameObject.SetActive(false);
                    break;
                default:
                //Debug, suppose no excute
                    print(pressed.name);
                    Debug.Log("Unable locate Ranking pressed");
                    //第一次進rank才不會沒load到東西
                    rank.gameObject.GetComponent<RankControl>().LoadScore("Add");
                    break;
            }
        }
        else
        {   
            // When FB is logged in.
            rank.gameObject.SetActive(false);
            ScrollView.gameObject.SetActive(true);
            Ranking.gameObject.SetActive(false);
            FacebookRanking.gameObject.SetActive(true);

            FacebookLogin fb = GameObject.Find("GameManager").GetComponent<FacebookLogin>();
            fb.info(FB.IsLoggedIn);

           
        }
    }
    void OKButtonPressed()
    {
		planet.gameObject.SetActive (true);
		arrow.gameObject.SetActive (true);
		nameSprite.gameObject.SetActive (true);
        rank.gameObject.SetActive (false);
        NameText.gameObject.SetActive(true);
        ExperienceText.gameObject.SetActive(true);
        ItemList.gameObject.SetActive(true);
        ScrollView.gameObject.SetActive(false);
        Ranking.gameObject.SetActive(false);
        FacebookRanking.gameObject.SetActive(false);
       
    }

	//
	// Portrait
	//
    void HeadPressed()
    {
		nameSprite.FindChild ("ChangeHeadPad").gameObject.SetActive (true);
    }
    void ChangeHead(tk2dUIItem selected)
    { 
		//Deselect All Portrait
		for (int i = 0; i < chp.childCount; i++) 
		{
			chp.GetChild (i).GetChild(0).gameObject.SetActive (false);
		}

		selected.gameObject.transform.GetChild (0).gameObject.SetActive (true);
		getNewPortraitID ();
		setPortrait ();
    }

	void setPortrait()
	{
		nameSprite.FindChild ("Portrait").gameObject.GetComponent<tk2dSprite> ().spriteId = currentActivePortrait;

		// Close ChangeHeadPad
		chp.gameObject.SetActive(false);
	}

	void getNewPortraitID()
	{
		for (int i = 0; i < chp.childCount; i++) 
		{
			if(chp.GetChild(i).FindChild("isChecked").gameObject.activeSelf) //check is activated
			{
				currentActivePortrait = chp.GetChild (i).gameObject.GetComponent<tk2dSprite> ().spriteId;
			}
		}
	}
    void getCurrentPortrait(){
        currentActivePortrait = nameSprite.FindChild("Portrait").gameObject.GetComponent<tk2dSprite>().spriteId;
    }

    void SettingPadPressed()
    {
        SettingPad.gameObject.SetActive(true);
        planet.gameObject.SetActive (false);
        arrow.gameObject.SetActive (false);
        nameSprite.gameObject.SetActive (false);
        NameText.gameObject.SetActive(false);
        ExperienceText.gameObject.SetActive(false);
        ItemList.gameObject.SetActive(false);

    }
    void InformationPressed()
    {
        InformationPad.gameObject.SetActive(true);
        SettingPad.gameObject.SetActive(false);
    }
    void SettingPadCancelled()
    {
        SettingPad.gameObject.SetActive(false);
        planet.gameObject.SetActive (true);
        arrow.gameObject.SetActive (true);
        nameSprite.gameObject.SetActive (true);
        NameText.gameObject.SetActive(true);
        ExperienceText.gameObject.SetActive(true);
        ItemList.gameObject.SetActive(true);
    }
    void InformationPadCancelled()
    {
        InformationPad.gameObject.SetActive(false);
        SettingPad.gameObject.SetActive(true);
    }
    void AnimePressed()
    {
        SceneManager.LoadScene("StartAnimation", LoadSceneMode.Single);
    }
    void ChestPadPressed()
    {
        ChestPad.gameObject.SetActive(true);
        planet.gameObject.SetActive (false);
        arrow.gameObject.SetActive (false);
        nameSprite.gameObject.SetActive (false);
        NameText.gameObject.SetActive(false);
        ExperienceText.gameObject.SetActive(false);
        ItemList.gameObject.SetActive(false);

        //達成指定條件,心之碎片獲得
        if (LevelControl.expAdd >= 100)
        {
            heart1.gameObject.SetActive(true);
        }
        if (LevelControl.expSub >= 100)
        {
            heart2.gameObject.SetActive(true);
        }
        if (LevelControl.expDiv >= 100)
        {
            heart3.gameObject.SetActive(true);
        }

    }
    void ChestPadReturn()
    {
        ChestPad.gameObject.SetActive(false);
        planet.gameObject.SetActive (true);
        arrow.gameObject.SetActive (true);
        nameSprite.gameObject.SetActive (true);
        NameText.gameObject.SetActive(true);
        ExperienceText.gameObject.SetActive(true);
        ItemList.gameObject.SetActive(true);
    }
    void ChestInfo()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                ChestInfoPad.gameObject.SetActive(true);
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                ChestInfoPad.gameObject.SetActive(false);
            }
        }

        #if UNITY_EDITOR
        if (Input.mousePresent)
        {
            if (Input.GetMouseButtonDown(0))
            {
                ChestInfoPad.gameObject.SetActive(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                ChestInfoPad.gameObject.SetActive(false);
            }
        }
        #endif

    }
    public void EndingTrigger()
    {
        if (LevelControl.expAdd >= 100 && LevelControl.expSub >= 100 && LevelControl.expDiv >= 100)
        {
            SceneManager.LoadScene("EndingAnimations", LoadSceneMode.Single);
        }
    }
}
