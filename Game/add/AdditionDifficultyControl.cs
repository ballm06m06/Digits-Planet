using UnityEngine;
using System.Collections;

/******************************************
加法模式
怪獸0
	數字範圍0-9
怪獸1
	數字範圍10-99
怪獸2
	數字範圍100-299

難度控制要控制的東西：
1.Target的大小
2.是否要Ban
*******************************************/
public class AdditionDifficultyControl : MonoBehaviour {

	public enum difficulty
	{
		easy,normal,hard
	}

	public difficulty CurrentDifficulty;

	AdditionScoreControl asc;

	public tk2dSprite startInstruction;


	// Use this for initialization
	void Awake () {
		
		asc = gameObject.GetComponent<AdditionScoreControl> ();

		switch (chooseMode.setDifficulty) {

		case 1:
			CurrentDifficulty = difficulty.hard;
			break;
		case 2:
			CurrentDifficulty = difficulty.normal;
			break;
		case 3:
			CurrentDifficulty = difficulty.easy;
			break;
		default:
			Debug.LogError ("Unable to set difficulty in addition mode");
			break;
		}

		switch (CurrentDifficulty) {

		case difficulty.easy:
			asc.targetMinMax = new Vector2 (50f, 100f);
			asc.BanSwitch = false;
			break;

		case difficulty.normal:
			asc.targetMinMax = new Vector2 (50f, 100f);
			asc.bannedMinMax = new Vector2 (1f, 9f);
			asc.BanSwitch = true;
			asc.bannedMode = false;							//尾數禁止
			break;

		case difficulty.hard:
			asc.targetMinMax = new Vector2 (100f, 200f);
			asc.bannedMinMax = new Vector2 (12f, 98f);
			asc.BanSwitch = true;
			asc.bannedMode = true;							//所有數禁止
			break;
		}
	}

	void Start(){

		//Set sprite
		switch(CurrentDifficulty){// H=0, M=1, E=2
		case difficulty.easy:
			startInstruction.spriteId = 2;
			break;
		case difficulty.normal:
			startInstruction.spriteId = 1;
			break;
		case difficulty.hard:
			startInstruction.spriteId = 0;
			break;
		default:
			Debug.LogWarning ("Unable to set startInstruction");
			break;
		}
        chooseMode.isGameLoaded = true;
	}
}
