using UnityEngine;
using System.Collections;

public class GameProgressControl : MonoBehaviour {
	
	public Transform maps;
	Transform mapAdd;
	Transform mapSub;
	Transform mapDiv;


	bool[] addProgress = new bool[3];	// Hard, Medium, Easy
	bool[] subProgress = new bool[3];
	bool[] divProgress = new bool[3];

	public bool[] addCurrentProgress = new bool[3];	// Hard, Medium, Easy
	public bool[] subCurrentProgress = new bool[3];
	public bool[] divCurrentProgress = new bool[3];

	// Use this for initialization
	void Awake () {
		// Determin shortcuts to maps
		mapAdd = maps.GetChild (0);
		mapSub = maps.GetChild (1);
		mapDiv = maps.GetChild (2);

		// Default Progress, Easy is all unlock but medium and hard is locked
		for (int i = 0; i <= 2; i++) {
			addProgress [i] = false;
			subProgress [i] = false;
			divProgress [i] = false;
		}

		SetProgress ();
	}

	void SetProgress(){
		for (int i = 0; i < addProgress.Length; i++) 
			mapAdd.GetChild (i).gameObject.SetActive (addProgress [i]);

		for (int j = 0; j < subProgress.Length; j++) 
			mapSub.GetChild (j).gameObject.SetActive (subProgress [j]);

		for (int k = 0; k < divProgress.Length; k++)
			mapDiv.GetChild (k).gameObject.SetActive (divProgress [k]);
	}

	void LoadCurrentProgress(){
		for (int i = 0; i < addCurrentProgress.Length; i++) 
			addProgress [i] = addCurrentProgress [i];

		for (int j = 0; j < subCurrentProgress.Length; j++)
			subProgress [j] = subCurrentProgress [j];

		for (int k = 0; k < divCurrentProgress.Length; k++)
			divProgress [k] = divCurrentProgress [k];
	}
}
