using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class ScoreBoard : MonoBehaviour {

    public static List<int> addScoreList = new List<int> ();   //暫存分數的清單
	public static List<int> subScoreList = new List<int> ();
	public static List<int> divScoreList = new List<int> ();

    private chooseMode cm;

    private string PlayerScoreSavedFileName = "PlayerScore";

	// Use this for initialization
	void Awake () {

		cm = gameObject.GetComponent<chooseMode>();
		if (cm == null) {
			Debug.LogError ("Unable to load chooseMode script");
		}

		//如果讀取前不存，會讀不到8筆
		//如果讀取前存檔，當有資料會覆寫過去
		if (!PlayerPrefs.HasKey("isFirstTime") || chooseMode.isGameLoaded){// First time to excute the game
			SaveScoreToSave();
			LoadScoreFromSave();
		}
		else
	        LoadScoreFromSave();

		if (addScoreList.Count != 8)
			Debug.LogError ("Load Error");
		if (subScoreList.Count != 8)
			Debug.LogError ("Load Error");
		if (divScoreList.Count != 8)
			Debug.LogError ("Load Error");
        
    }
 
    void OnDestroy(){
        SaveScoreToSave();
    }

	public static List<int> SortList(List<int> l){ //大到小排序
		l.Sort();
		l.Reverse();
		return l;
    }
	public static List<int> SortList(string type)
	{
		switch (type) 
		{
		case "Addition":
			addScoreList = SortList (addScoreList);
			return addScoreList;
		case "Subtraction":
			subScoreList = SortList (subScoreList);
			return subScoreList;
		case "Division":
			divScoreList = SortList (divScoreList);
			return divScoreList;
		default:
			Debug.LogError ("Unable to sort List");
			return null;
		}
	}

    void SaveScoreToSave(){
		
		SortList(addScoreList);
		for (int i = 0; i < 8; i++) {
			try {
				PlayerPrefs.SetInt (PlayerScoreSavedFileName + "Add" + i, addScoreList [i]);
			} catch (System.ArgumentOutOfRangeException) {
				PlayerPrefs.SetInt (PlayerScoreSavedFileName + "Add" + i, 0);
				print ("Add exception, i=" + i);
			}
		}

		SortList(subScoreList);

		for (int i = 0; i < 8; i++) {
			try {
				PlayerPrefs.SetInt (PlayerScoreSavedFileName + "Sub" + i, subScoreList [i]);
			} catch (System.ArgumentOutOfRangeException) {
				PlayerPrefs.SetInt (PlayerScoreSavedFileName + "Sub" + i, 0);
				print ("Sub exception, i=" + i);
			}
		}

		SortList(divScoreList);
		for (int i = 0; i < 8; i++) {
			try {
				PlayerPrefs.SetInt (PlayerScoreSavedFileName + "Div" + i, divScoreList [i]);
			} catch (System.ArgumentOutOfRangeException) {
				PlayerPrefs.SetInt (PlayerScoreSavedFileName + "Div" + i, 0);
				print ("Div exception, i=" + i);
			}
		}
		print ("玩家分數記錄已存檔");
    }

    void LoadScoreFromSave(){

        addScoreList.Clear();
        for (int i = 0; i < 8; i++){
			if (!PlayerPrefs.HasKey (PlayerScoreSavedFileName + "Add" + i))
				Debug.LogError ("Unable Load Player Data");
            addScoreList.Add(PlayerPrefs.GetInt(PlayerScoreSavedFileName +"Add"+ i));
        }
		subScoreList.Clear();
		for (int i = 0; i < 8; i++){
			if (!PlayerPrefs.HasKey (PlayerScoreSavedFileName + "Sub" + i))
				Debug.LogError ("Unable Load Player Data");
			subScoreList.Add(PlayerPrefs.GetInt(PlayerScoreSavedFileName +"Sub"+ i));
		}
		divScoreList.Clear();
		for (int i = 0; i < 8; i++){
			if (!PlayerPrefs.HasKey (PlayerScoreSavedFileName + "Div" + i))
				Debug.LogError ("Unable Load Player Data");
			divScoreList.Add(PlayerPrefs.GetInt(PlayerScoreSavedFileName +"Div"+ i));
		}
		print ("玩家分數記錄讀取");
    }
	public static void Add(int score, string type)
	{
		switch (type) 
		{
		case "Addition":
			ScoreBoard.addScoreList.Add (score);
			print (score + " added to Addition");
			break;
		case "Subtraction":
			ScoreBoard.subScoreList.Add (score);
			print (score + " added to Subtraction");
			break;
		case "Division":
			ScoreBoard.divScoreList.Add (score);
			print (score + " added to Division");
			break;
		default:
			Debug.LogWarning ("Unable to save score");
			break;
		}
		SortList (type);
		for (int i = 0; i < divScoreList.Count; i++)
			print ("current Score value" + divScoreList [i] + " in" + type + ", i=" + i);
	}
}
