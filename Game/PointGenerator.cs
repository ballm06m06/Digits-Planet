using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public abstract class PointGenerator : MonoBehaviour 
{
	//儲存出現過的點數
	protected List<Vector2> generatedPoint = new List<Vector2> ();

	public abstract Vector2 numberGenerator();


//	//Debug，統計各出現機率
//	void OnDestroy(){
//		int[] count = new int[3];
//		float sum = 0;
//		float[] chance = new float[3];
//
//		foreach(Vector2 i in generatedPoint){
//			print("(point, sequence) = "+i);
//			if (i.y == 0) {
//				count [0]++;
//			} else if (i.y == 1) {
//				count [1]++;
//			} else {
//				count [2]++;
//			}
//		}
//		for (int i = 0; i < count.Length; i++) {
//			sum += count [i];
//		}
//		if (sum == 0) {
//			return;
//		}
//		for (int j = 0; j < count.Length; j++) {
//			chance [j] = count [j] / sum;
//		}
//		for (int k = 0; k < chance.Length; k++) {
//			print((k)+"地鼠的出現機率統計"+chance[k]);
//
//		}
//		generatedPoint = null;
//	}


}
