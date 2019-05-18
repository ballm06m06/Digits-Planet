using UnityEngine;
using System.Collections;

public class LevelControl : MonoBehaviour {

	public Vector2 unlockPosition = new Vector2(35f, 50f); //解鎖Medium & Hard的時機
	public GameObject maps;

	//經驗值
	public static float expAdd = 0f;
	public static float expSub = 0f;
	public static float expDiv = 0f;

	static bool isExpModify = false;
	bool[] isUnlock = new bool[3]; //Easy, Medium, Hard lock

	public static void AddExperience(float score, string type){	//增加經驗值

		float e = score / 1000f;

		switch (type) 
		{
		case "Addition":
			expAdd += e;
			break;
		case "Subtraction":
			expSub += e;
			break;
		case"Division":
			expDiv += e;
			break;
		}
		isExpModify = true;
		print ("Current exp: " + expAdd);
		print ("Current exp: " + expSub);
		print ("Current exp: " + expDiv);
   	}

	// Use this for initialization
	void Start () {

		// 遊戲進度歸0
		isUnlock [0] = false;
		isUnlock [1] = false;
		isUnlock [2] = false;

		// Lock all level
		for(int j=0;j<maps.transform.childCount;j++){
			for (int k = 0; k < 3; k++) {
				maps.transform.GetChild (j).GetChild (k).gameObject.SetActive (false);
			}
		}

		//讀取玩家記錄
		if (!PlayerPrefs.HasKey("isFirstTime") || chooseMode.isGameLoaded)// First time to excute the game
			Save();			
		else
			Load ();
		
		ExpValidation (expAdd, "Add");	//解鎖玩家目前關卡
		ExpValidation (expSub, "Sub");	//解鎖玩家目前關卡
		ExpValidation (expDiv, "Div");	//解鎖玩家目前關卡

	}

	void Update () {
		if (isExpModify) {
			ExpValidation (expAdd, "Add");	//解鎖玩家目前關卡
			ExpValidation (expSub, "Sub");	//解鎖玩家目前關卡
			ExpValidation (expDiv, "Div");	//解鎖玩家目前關卡	
			Save ();
			isExpModify = false;
		}
	}
	void OnDestroy (){
//		Save ();
	}
				
	public void ExpValidation(float exp, string type){	//只有在增加經驗值的時候執行
		if (exp >= 100f) {

            isUnlock [0] = true;    //Easy
            isUnlock [1] = true;    //Medium
            isUnlock [2] = true;
				
		} else if (exp > unlockPosition.y) {
			//Unlock Hard level
			isUnlock [0] = true;	//Easy
			isUnlock [1] = true;	//Medium
			isUnlock [2] = true;	//Hard

		} else if (exp > unlockPosition.x) {
			//Unlock Medium level
			isUnlock [0] = true;	//Easy
			isUnlock [1] = true;	//Medium
			isUnlock [2] = false;	//Hard, locked

		} else
			isUnlock [0] = true; 	//Easy 預設解鎖

		UnlockLevel (type);
		// 遊戲進度歸0
		isUnlock [0] = false;
		isUnlock [1] = false;
		isUnlock [2] = false;
	}

	void UnlockLevel(string type){

		switch (type) 
		{
		case "Add":
			if (isUnlock [0]) 	//Easy is unlocked
				maps.transform.GetChild (0).GetChild (2).gameObject.SetActive (true);
			if (isUnlock [1])   //Medium is unlocked
				maps.transform.GetChild (0).GetChild (1).gameObject.SetActive (true);
			if (isUnlock [2])   //Hard is unlocked
				maps.transform.GetChild (0).GetChild (0).gameObject.SetActive (true);			
			break;
		case "Sub":
			if (isUnlock [0]) 	//Easy is unlocked
				maps.transform.GetChild (1).GetChild (2).gameObject.SetActive (true);
			if (isUnlock [1])   //Medium is unlocked
				maps.transform.GetChild (1).GetChild (1).gameObject.SetActive (true);
			if (isUnlock [2])   //Hard is unlocked
				maps.transform.GetChild (1).GetChild (0).gameObject.SetActive (true);			
			break;
		case "Div":
			if (isUnlock [0]) 	//Easy is unlocked
				maps.transform.GetChild (2).GetChild (2).gameObject.SetActive (true);
			if (isUnlock [1])   //Medium is unlocked
				maps.transform.GetChild (2).GetChild (1).gameObject.SetActive (true);
			if (isUnlock [2])   //Hard is unlocked
				maps.transform.GetChild (2).GetChild (0).gameObject.SetActive (true);			
			break;
		default:
			Debug.LogError ("Unable to unlock level");
			break;
		}

	}

	//
	// Save & Load
	//
	void Save(){
		PlayerPrefs.SetFloat ("PlayerExperienceAdd", expAdd);
		PlayerPrefs.SetFloat ("PlayerExperienceSub", expSub);
		PlayerPrefs.SetFloat ("PlayerExperienceDiv", expDiv);
		print ("玩家經驗值已存檔");

		//Debug
		Load();

	}
	void Load(){
		if (PlayerPrefs.HasKey ("PlayerExperienceAdd")) {
			expAdd = PlayerPrefs.GetFloat ("PlayerExperienceAdd");
		} else
			Debug.LogError ("Unable to load PlayerExperienceAdd");
		if (PlayerPrefs.HasKey ("PlayerExperienceSub")) {
			expSub = PlayerPrefs.GetFloat ("PlayerExperienceSub");
		} else
			Debug.LogError ("Unable to load PlayerExperienceSub");
		if (PlayerPrefs.HasKey ("PlayerExperienceDiv")) {
			expDiv = PlayerPrefs.GetFloat ("PlayerExperienceDiv");
		} else
			Debug.LogError ("Unable to load PlayerExperienceDiv");

		// Debug
		print ("Current exp: " + expAdd);
		print ("Current exp: " + expSub);
		print ("Current exp: " + expDiv);
	}


}
