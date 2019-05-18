using UnityEngine;
using System.Collections;

public class AdditionMoldChanceControl : MonoBehaviour {

	[System.Serializable]
	public struct ChanceStruct
	{
		public Vector3 endValue;
		public int dist;
	}
		
	public ChanceStruct[] CSArray = new ChanceStruct[3];

	private int currentDist;
	private AdditionPointGenerator apg;
	private ScoreControlAbstract ScoreControl;

	private Vector3 startValue;

	// Use this for initialization
	void Start () {
	
		apg = gameObject.GetComponent<AdditionPointGenerator> ();
		//透過繼承應用在各個mode
		ScoreControl = gameObject.GetComponent<ScoreControlAbstract> ();


		// 驗証各組合機率和為1
		foreach(ChanceStruct a in CSArray){
			if (a.dist == 0) {
				Debug.LogWarning ("AI距離設定不正確");
			}
			//必須轉成字串才能比較，因為float有浮點數誤差
			if ((a.endValue.x + a.endValue.y + a.endValue.z).ToString() != 1f.ToString()) {
				Debug.LogWarning ("和為"+(a.endValue.x + a.endValue.y + a.endValue.z) + "機率設定不正確");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		currentDist = Mathf.Abs(ScoreControl.TargetPoint - ScoreScript.CurrentPoint);

		startValue.x = apg.typeChance [0];
		startValue.y = apg.typeChance [1];
		startValue.z = apg.typeChance [2];

		//Debug
//		print("dist:"+currentDist);

		//依距離判斷要執行的機率腳本
		if (currentDist <= CSArray[0].dist) { //<10
			//只有在不是這個設定值的時候才套用此設定值
			if (startValue != CSArray [0].endValue) {
				apg.typeChance [0] = CSArray [0].endValue.x;
				apg.typeChance [1] = CSArray [0].endValue.y;
				apg.typeChance [2] = CSArray [0].endValue.z;
			}
		}else if (CSArray [0].dist < currentDist && currentDist < CSArray[2].dist) { // 10~50
			//只有在不是這個設定值的時候才套用此設定值
			if (startValue != CSArray [1].endValue) {
				apg.typeChance [0] = CSArray [1].endValue.x;
				apg.typeChance [1] = CSArray [1].endValue.y;
				apg.typeChance [2] = CSArray [1].endValue.z;
			}
		}else if (currentDist >= CSArray[2].dist) {
			//只有在不是這個設定值的時候才套用此設定值
			if (startValue != CSArray [2].endValue) {
				apg.typeChance [0] = CSArray [2].endValue.x;
				apg.typeChance [1] = CSArray [2].endValue.y;
				apg.typeChance [2] = CSArray [2].endValue.z;
			}
		}
	}
}
