using UnityEngine;
using System.Collections;

public class AdditionPointGenerator : PointGenerator  {

	public Vector2[] moleKind = new Vector2[3];

	// 機率控制地鼠出現機率，三者加起來為1
	public float[] typeChance = new float[3];
	private float[] sumArray = new float[3];


	// Use this for initialization
	void Start () {

		//Check the settings
		for (int i = 0; i < moleKind.Length; i++) 
		{
			//通常不會執行，如果執行則需查看參數
			if (moleKind [i].x == 0 || moleKind [i].y == 0) {
				Debug.LogWarning ("Using default number area, Please check Settings in AdditionPointGenerator");
				// Reset to default
				moleKind [i].x = 1;
				moleKind [i].y = 9;
			}
		}

		// Check typeChance valid
		float check = 0;
		foreach(float i in typeChance){ check += i; }
		if (check != 1) {
			Debug.LogWarning ("Generator chance settings invalid, using default chance");
			typeChance [0] = .5f;
			typeChance [1] = .3f;
			typeChance [2] = .2f;
		}

		// Sum the chance and save them into an array
		for (int i = 0; i < typeChance.Length; i++) {
			for (int j = 0; j <= i; j++) {
				sumArray [i] += typeChance [j];
			}
		}
	}

	void Update(){
		// 歸0
		for (int i = 0; i < sumArray.Length; i++) {
			sumArray[i] = 0f;
		}

		// Sum the chance and save them into an array
		for (int i = 0; i < typeChance.Length; i++) {
			for (int j = 0; j <= i; j++) {
				sumArray [i] += typeChance [j];
			}
		}

	}

	public override Vector2 numberGenerator(){
		Vector2 pointAndSeq;

		//控制生成種類
		pointAndSeq.y = typeChooser();
//		Debug.Log ("chooseType:"+pointAndSeq.y);

		float min, max;
		min = moleKind [(int)pointAndSeq.y].x;
		max = moleKind [(int)pointAndSeq.y].y;

		//依種類生成數字
		pointAndSeq.x = (int)Random.Range (min, max);

		//儲存生成數字
		generatedPoint.Add (pointAndSeq);

		return pointAndSeq;
	}

	private int typeChooser(){
		float range = Random.value;
//		Debug.Log (range);

		if (0f <= range && range <= sumArray [0]) {//0~.5
			return 0;
		} else if (sumArray [0] < range && range <= sumArray [1]) {//.51~..7
			return 1;	
		} else if (sumArray [1] < range && range <= 1f) {//.7~1
			return 2;
		} else {
			Debug.LogWarning ("Invalid type choosed");
			return 0;
		}
	}
}
