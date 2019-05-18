using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public abstract class ScoreControlAbstract : MonoBehaviour {

	public tk2dTextMesh bannedDisplay;
	public tk2dTextMesh targetDisplay;
	public tk2dTextMesh currentDisplay;
	public tk2dTextMesh scoreDisplay;
	public tk2dSprite scoreSprite;

	public AudioClip getScore;

	public int gameTimeBonus = 10;
	public int gameTimeDeduct = -5;

	public Image fadeImage;
	public float fadeTime = 10f;

	protected int fadeCount = 0;

	public int FadeCount {
		get {
			return fadeCount;
		}
	}

	protected int targetPoint;

	public int TargetPoint {
		get {
			return targetPoint;
		}
	}
		
//	// Vibrate function
//	#if UNITY_ANDROID
//	AndroidJavaObject v;
//
//	void Start () {
//		using(AndroidJavaClass p = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) {
//			using(AndroidJavaObject a = p.GetStatic<AndroidJavaObject>("currentActivity")) {
//				v = a.Call<AndroidJavaObject>("getSystemService", "vibrator");
//			}
//		}
//	}
//
//	void Vibrate(long time){ // 此部分要單獨寫成方法(函式)，不可單獨呼叫
//		v.Call("vibrate", time);
//	}
//	#endif

	protected void fade()
	{
		fadeImage.color = Color.red;
		StartCoroutine (recoverLoop ());
		fadeCount++;

//		#if UNITY_ANDROID
//		Vibrate(150);	// 控制震動時長1/1000s
//		#endif
	}

	private IEnumerator recoverLoop()
	{
		// Turn back to clear slowly after amount of time
		while (fadeImage.color != Color.clear)
		{
			fadeImage.color = Color.Lerp (fadeImage.color, Color.clear, fadeTime * Time.deltaTime);
			yield return null;
		}
		StopCoroutine (recoverLoop());
	}
}
