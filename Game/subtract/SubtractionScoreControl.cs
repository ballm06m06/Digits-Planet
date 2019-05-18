using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

public class SubtractionScoreControl : ScoreControlAbstract {

	//文字顯示繼承ScoreControlAbstract

	public bool bannedMode;	//禁止數模式選擇 true:每一位禁止 false:尾數禁止

	public Vector2 targetMinMax = new Vector2 (50f, 100f);
	public Vector2 distanceMinMax = new Vector2 (50f, 100f);
	public Vector2 bannedMinMax = new Vector2 (50f, 100f);

	//禁止數模式開關 true=開啟 false=關閉
	public bool BanSwitch = false;

	// targetPoint 繼承自ScoreControlAbstract
//	private int currentPoint;
	private char[] bannedArray;

    private SubtractionDifficultyControl sdc;

	//Debug
//	List<int> countList = new List<int>();
//	List<int> countTarList = new List<int>();
//	List<int> countBanList = new List<int>();
//	List<int> countDistList = new List<int>();




	// Use this for initialization
	void Start () {

		bannedDisplay.text = "";
        sdc = gameObject.GetComponent<SubtractionDifficultyControl>();

		//重設點數目標及禁數
		do {
			resetTarget (BanSwitch);
		} while(BanSwitch && isBanned (ScoreScript.CurrentPoint, bannedMode));


		//讓遊戲分數歸零
		ScoreScript.Score = 0;

		//選擇ScoreSprite
		changeScoreSprite();

	}

	// Update is called once per frame
	void Update () {
		
		//遊戲進行中不斷Update目前數字
		currentDisplay.text = string.Format("{0}" , ScoreScript.CurrentPoint);
		scoreDisplay.text = string.Format("{0}", ScoreScript.Score);
		scoreDisplay.Commit();

		//驗證得分
		if (targetPoint == ScoreScript.CurrentPoint) {	//目標點數=現在點數，得分！

			//得分
			ScoreScript.Score += 10;
			ScoreScript.CurrentPoint = 0;
			resetTarget (BanSwitch);
			MainGameScript.GameTimeChange (gameTimeBonus);

		}
		//驗證是否超過目標數
		else if (targetPoint > ScoreScript.CurrentPoint ) {	//不為0 且 爆掉了
			resetTarget (BanSwitch);
			fade ();
			MainGameScript.GameTimeChange(gameTimeDeduct);
		}
		//驗證是否採到禁止數
		if (BanSwitch) {//禁數模式開啟
			if (isBanned(ScoreScript.CurrentPoint,bannedMode) ) { //踩到禁數 
				resetTarget (BanSwitch);
				fade ();
				//扣遊戲時間
				MainGameScript.GameTimeChange(gameTimeDeduct);
			}
		}

		//依各關不同控制
		//取得hitPoint然後依各關規則修改currentPoint
		//以本關為例，currentPoint要減hitPoint
		if (ScoreScript.HitPoint != 0) {
			ScoreScript.CurrentPoint -= ScoreScript.HitPoint;
			ScoreScript.HitPoint = 0;
		}
	}

	void resetTarget(bool isBannedOn)
	{

		int bannedNum;
		int numDistance;		//currentPoint與targetPoint間的數字差異

		//Debug
//		int count=0;
//		int countTar=0;
//		int countBan=0;
//		int countDist=0;

		//現在數歸0
		ScoreScript.CurrentPoint = 0;

		//判斷禁止模式是否開啟，產生禁止數及目標數
		if (isBannedOn) {//-->禁止數模式開啟

			//隨機產生一個目標數
			targetPoint = (int)Random.Range (targetMinMax.x, targetMinMax.y);
//			count++;
//			countTar++;

			//隨機產生一距離數
			numDistance = (int)Random.Range (distanceMinMax.x, distanceMinMax.y);
			//產生目前數
			ScoreScript.CurrentPoint = targetPoint + numDistance;

			//隨機產生一個禁止數
			do {
				bannedNum = (int)Random.Range (bannedMinMax.x, bannedMinMax.y);
				bannedArray = bannedNum.ToString ().ToCharArray ();
//				count++;
//				countBan++;
			} while(isLegalBannedNumber (bannedArray) || isBanned (targetPoint, bannedMode));	//驗證禁止數是否符合規則，不符合則重新產生

			//排序禁止數資料
			System.Array.Sort(bannedArray);

//			countList.Add (count);
//			countTarList.Add (countTar);
//			countBanList.Add (countBan);

			//讓禁止數顯示在UI上
			if (!bannedMode) {
				bannedDisplay.text = ""; //Reset
				for (int i = 0; i < bannedArray.Length - 1; i++) {
					//ex: "尾數禁止: 0,1,2"
					bannedDisplay.text += bannedArray [i];
					bannedDisplay.text += ", ";
				}
				bannedDisplay.text += bannedArray [bannedArray.Length - 1];

			} else {
				bannedDisplay.text = ""; //Reset
				for (int i = 0; i < bannedArray.Length - 1; i++) {
					//ex: "任意數禁含: 0,1,2"
					bannedDisplay.text += bannedArray [i];
					bannedDisplay.text += ", ";
				}
				bannedDisplay.text += bannedArray [bannedArray.Length - 1];	
			}


		} else {
			//產生目標數
			targetPoint = (int)Random.Range (targetMinMax.x, targetMinMax.y);
			//產生距離數
			numDistance = (int)Random.Range (distanceMinMax.x, distanceMinMax.y);
			//產生目前數
			ScoreScript.CurrentPoint = targetPoint + numDistance;
			//顯示在UI上
			bannedDisplay.text = "";

		}

		//讓目標數顯示在UI上
		targetDisplay.text = targetPoint.ToString();

		//註：目前數不用額外顯示，目前數定義後會自動顯示

	}

	bool isLegalBannedNumber(char[] inputArr){
		//判斷bannedNumber不可以有相同數字

		//true為ban中有相同的數字, false為ban中沒相同的數字
		for (int i = 0; i < (inputArr.Length - 1); i++) {
			for (int j = (i + 1); j < inputArr.Length; j++) {
				if (inputArr [i] == inputArr [j])
					return true;
			}
		}

        if ((int)sdc.CurrentDifficulty == 2) //Hard mode
        { //判斷bannedNumber中不能有1
            for (int i = 0; i < (inputArr.Length - 1); i++) {
                if(inputArr[i] == 1)
                    return true;
            }
        }

		return false;
	}

	bool isBanned(int nowPoint,bool mode){	
		//判斷是否吻合禁止數
		//吻合return true, 不吻合return false

		//mode=false 檢查末位 , true檢查全部數字
		char[] inputArray = nowPoint.ToString ().ToCharArray();
		char[] cpArray = bannedArray;

		if (mode == false) 		//檢查末位數字
		{
			foreach (char j in cpArray) {
				if (inputArray [inputArray.Length - 1] == j) //末位數字
					return true;
			}
		} 
		else 					//檢查全部
		{
			foreach (char i in inputArray) {
				foreach (char j in cpArray) {
					if (i == j) 
						return true;	
				}		
			}
		}
		return false;
	}

	private void changeScoreSprite(){// None=3, Every=0, Tail=4
		if (BanSwitch) {
			if (bannedMode)
				scoreSprite.spriteId = 0;// Every
			else
				scoreSprite.spriteId = 4;// Tail
		}else
			scoreSprite.spriteId = 3;// None
	}

//	void OnDestroy(){
//		int sumCount = 0;
//		int sumCountTar = 0;
//		int sumCountBan = 0;
//
//		foreach (int i in countList)
//			sumCount += i;
//		foreach (int j in countTarList)
//			sumCountTar += j;
//		foreach (int k in countBanList)
//			sumCountBan += k;
//
//		int avgTar = sumCountTar / countTarList.Count;
//		int avg = sumCount / countList.Count;
//		int avgBan = sumCountBan / countBanList.Count;
//
//		print ("avg:" + avg);
//		print ("avgTar:" + avgTar);
//		print ("avgBan:" + avgBan);
//			
//	}
}
