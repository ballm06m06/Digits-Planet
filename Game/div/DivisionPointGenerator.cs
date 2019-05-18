using UnityEngine;
using System.Collections;

//控制生成的數字
public class DivisionPointGenerator : PointGenerator
{
	// Attention!!
	// 這邊的變數要和moleScript的陣列大小設一樣，若設定不一樣，遊戲會運作錯誤，且unity不會提出警告
	public int moleKinds;

	public float[] typeChance = new float[6];
	private float[] sumArray = new float[6];

	//宣告質數陣列
	private int[] prime = new int[6];

	public int[] Prime {
		get {
			return prime;
		}
	}

	// Use this for initialization
	void Start () {
		
		//定義質數陣列
		prime [0] = 2;
		prime [1] = 3;
		prime [2] = 5;
		prime [3] = 7;
		prime [4] = 11;
		prime [5] = 13;

		// Check typeChance valid
		float check = 0;
		foreach(float i in typeChance){ check += i; }
		if (check != 1) {
			Debug.LogWarning ("Generator chance settings invalid, using default chance");
			typeChance [0] = .2f;
			typeChance [1] = .2f;
			typeChance [2] = .2f;
			typeChance [3] = .2f;
			typeChance [4] = .1f;
			typeChance [5] = .1f;
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

		pointAndSeq.y = typeChooser ();						//sequence
		pointAndSeq.x = (int)prime [(int)pointAndSeq.y];	//point

		//儲存生成數字
		generatedPoint.Add (pointAndSeq);

		return pointAndSeq;
	}

	private int typeChooser(){
		float range = Random.value;
//		print (range);

		if (0f <= range && range <= sumArray [0]) {//0~.2
			return 0;
		} else if (sumArray [0] < range && range <= sumArray [1]) {//.21~.4
			return 1;	
		} else if (sumArray [1] < range && range <= sumArray [2]) {//.41~6
			return 2;
		} else if (sumArray [2] < range && range <= sumArray [3]) {//.61~.8
			return 3;
		} else if (sumArray [3] < range && range <= sumArray [4]) {//.81~.9
			return 4;
		} else if (sumArray [4] < range && range <= sumArray [5]) {//.91~1
			return 5;
		} else {
			Debug.LogWarning ("Invalid type choosed");
			return 0;
		}
	}
}
