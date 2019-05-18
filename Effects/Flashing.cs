using UnityEngine;
using System.Collections;

public class Flashing : MonoBehaviour {

	public float interval;

	float time;
	GameObject child;
	bool isMouseOver;

	void Start(){
		child = gameObject.transform.GetChild(0).gameObject;
		child.SetActive (true);
	}
	// Update is called once per frame
	void Update () {

		time += Time.deltaTime;
		if (time >= interval) {
			time = 0;
			if (!isMouseOver) {
				if (child.activeSelf)
					child.SetActive (false);
				else
					child.SetActive (true);
			}
		}
	}

	//因為UNITY_ANDROID觸屏會永遠感應到滑鼠
	#if UNITY_EDITOR
	void OnMouseEnter(){
		isMouseOver = true;		
		child.SetActive (true);

	}
	void OnMouseExit(){
		isMouseOver = false;
	}
	#endif
		
}
