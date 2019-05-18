using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RankControl : MonoBehaviour {

	public GameObject PlayerScoreListPrefab;
	private GameObject instGameObj;	//分數物件生成的

	private chooseMode cm;

	// Awake會在這個GameObject一被activate時執行
	void Awake(){
		cm = GameObject.Find("GameManager").GetComponent<chooseMode>();
		if (cm == null) {
			Debug.LogError ("Unable to load chooseMode script");
		}
	}


	public void LoadScore(string type) {

		// 清空排行榜
		ClearLayout();

		switch(type)
		{
		case "Add":	//add
			// 生成排行榜
			for (int i = 0; i < 8; i++)
			{
				instGameObj = (GameObject)Instantiate (PlayerScoreListPrefab);
				//SetParent(transform,false) the false can moderate the prefab scale instantiate on UI
				instGameObj.transform.SetParent (transform, false);
				instGameObj.transform.Find ("RankText").GetComponent<Text> ().text = "No." + (i+1);
				instGameObj.transform.Find ("NameText").GetComponent<Text> ().text = PlayerPrefs.GetString (cm.getPlayerSaveFileName);
				instGameObj.transform.Find ("ScoreText").GetComponent<Text> ().text = ScoreBoard.addScoreList [i].ToString ();
			}
			break;
		case "Sub":	//sub
			// 生成排行榜
			for (int i = 0; i < 8; i++)
			{
				instGameObj = (GameObject)Instantiate (PlayerScoreListPrefab);
				//SetParent(transform,false) the false can moderate the prefab scale instantiate on UI
				instGameObj.transform.SetParent (transform, false);
				instGameObj.transform.Find ("RankText").GetComponent<Text> ().text = "No." + (i+1);
				instGameObj.transform.Find ("NameText").GetComponent<Text> ().text = PlayerPrefs.GetString (cm.getPlayerSaveFileName);
				instGameObj.transform.Find ("ScoreText").GetComponent<Text> ().text = ScoreBoard.subScoreList [i].ToString ();
			}
			break;
		case "Div":	//div
			// 生成排行榜
			for (int i = 0; i < 8; i++)
			{
				instGameObj = (GameObject)Instantiate (PlayerScoreListPrefab);
				//SetParent(transform,false) the false can moderate the prefab scale instantiate on UI
				instGameObj.transform.SetParent (transform, false);
				instGameObj.transform.Find ("RankText").GetComponent<Text> ().text = "No." + (i+1);
				instGameObj.transform.Find ("NameText").GetComponent<Text> ().text = PlayerPrefs.GetString (cm.getPlayerSaveFileName);
//				print ("current Score value in div" + ScoreBoard.divScoreList [i]+", i="+i);
				instGameObj.transform.Find ("ScoreText").GetComponent<Text> ().text = ScoreBoard.divScoreList [i].ToString ();
			}
			break;
		default:
			Debug.LogError ("Unable to load score");
			break;
		}
	}

	void ClearLayout(){
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			Destroy (gameObject.transform.GetChild (i).gameObject);
		}
	}

}
