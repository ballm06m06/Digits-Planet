using UnityEngine;
using System.Collections;

public class ScoreScript : MonoBehaviour
{

	static private int score;					//遊戲分數
	static private int currentPoint = 0;		//目前點數
	static private int hitPoint;				//打到的怪物點數

	static public int Score
	{
		get
		{
			return score;	
		}

		set
		{
			score = value;	
		}
	}

	static public int CurrentPoint {
		get {
			return currentPoint;
		}
		set {
			currentPoint = value;
		}
	}

	public static int HitPoint {
		get {
			return hitPoint;
		}
		set {
			hitPoint = value;
		}
	}

	// Use this for initialization
	void Start ()
	{
		score = 0;
		hitPoint = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
//		Debug.Log ("Score:" + score + ", now:" + currentPoint);
	}
}
