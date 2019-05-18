using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DivisionMoleChanceControl : MonoBehaviour {

	[System.Serializable]
	public class ChanceList
	{	// inner class, define float array
		public float[] endValue = new float[6];
	}
	public ChanceList[] CLArray = new ChanceList[4];

	private int currentPoint;

	// Save status of type contained in currentPoint
	private bool[] containType = new bool[6];
	private int containTypeCount = 0;

	//宣告linkedlist讓containType為true的可以addFront，為false的可以addLast，儲存index
	private LinkedList<int> index = new LinkedList<int> ();

	private DivisionPointGenerator dpg;

	// Use this for initialization
	void Start () {
	
		dpg = gameObject.GetComponent<DivisionPointGenerator> ();
		if (dpg == null) {
			Debug.LogWarning ("Unable to load DivisionPointGenerator");
		}// Warning, 通常不會執行

		// 驗証各組合機率和為1
		float sum = 0f;
		foreach (ChanceList a in CLArray) {
			for (int i = 0; i < a.endValue.Length; i++) {
				sum += a.endValue [i];
			}
			if (sum.ToString () != 1f.ToString ()) {
				Debug.LogWarning (a + "機率和不為1, 請檢查設定，目前值為:" + sum);
			}
			sum = 0f;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
		currentPoint = ScoreScript.CurrentPoint;	//取得現在點數

		// 判斷currentPoint可以被哪些prime除盡
		for (int i = 0; i < dpg.Prime.Length; i++) {
			if (currentPoint % dpg.Prime [i] == 0) {
				containType [i] = true;
				containTypeCount++;
			}
		}
		// index sort by true or false
		CommitIndexList ();

		switch (containTypeCount) {
		case 1:
			ChanceControl (0);
			break;
		case 2:
			ChanceControl (1);
			break;
		case 3:
			ChanceControl (2);
			break;
		default:
			ChanceControl (3);
			break;
		}

		//歸0，清除以備下回合
		for (int j = 0; j < containType.Length; j++) {
			containType [j] = false;
		}
		containTypeCount = 0;
		index.Clear ();
	}

	void CommitIndexList(){	//依containType為true or false排序index
		for (int i = 0; i < containType.Length; i++) {
			//將index排序
			if (containType [i]) 
				index.AddFirst (i);
			else 
				index.AddLast (i);
		}
	}

	void ChanceControl(int chanceIndex){
		
		LinkedListNode<int> current = index.First;
		int ci = chanceIndex;
		// 依照index更改typeChance的值
		for (int i = 0; i < index.Count; i++) {
			dpg.typeChance [current.Value] = CLArray[ci].endValue [i];
			if (current == index.Last)
				break;
			else
				current = current.Next;
		}
		// Assist recycle system
		current = null;
	}
}
