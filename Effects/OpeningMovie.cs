using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]//必須要有AudioSource，任何加上此腳本的物件將自動加入AudioSource
public class OpeningMovie : MonoBehaviour {

	#if UNITY_EDITOR
	public MovieTexture movieTexture;   //影片
	private AudioSource movieAudio;     //影片音軌
	#endif

	private int skipPressCount = 0;
	private float skipPressInterval = 0f;
	private float skipPressIntervalWait = 1f;
	private float movieDuration = 128.4f;	//行動裝置無法使用movieTexture

	void Start()
	{

		#if UNITY_EDITOR
		//Get source
		movieDuration = movieTexture.duration;
		GetComponent<RawImage>().texture = movieTexture;
		movieAudio = GetComponent<AudioSource>();
		movieAudio.clip = movieTexture.audioClip;//這個MovieTexture的音軌

		//Play
		movieTexture.Play();
		movieAudio.Play();

		#elif UNITY_ANDROID
		gameObject.GetComponent<RawImage>().color = Color.black;
		StartCoroutine(PlayOnMobile());

		#endif

		//When load menu will enter PlanetChoice directly
		chooseMode.isGameLoaded = true;
	}

	#if UNITY_EDITOR
	void Update()
	{
		// Skip for UNITY_EDITOR, debug only
		if (skipPressCount != 0) {
			skipPressInterval += Time.deltaTime;
			if (skipPressInterval > skipPressIntervalWait) {
				skipPressCount = 0;
				skipPressInterval = 0;
				print ("Reset skipPressCount");
			}
		}
	}

	public void SkipMovieButton(){
		print ("Skip movie pressed");

		skipPressCount++;
		if (skipPressCount == 2) {
			SkipMovie ();
			skipPressCount = 0;
		} else if (skipPressCount > 2) 
			Debug.LogError ("Skip movie count overbound!");
	}

	void SkipMovie(){
		UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");//載入場景
	}
	#endif

	IEnumerator LoadScene()
	{
		// Another program will load while playing the movie, thus the game will pause until the movie play was done.
		// Game will replay when the movie was finished, and we shall wait until then.
		yield return new WaitForEndOfFrame();	

		UnityEngine.SceneManagement.SceneManager.LoadScene("Menu");	//載入場景

		yield return null;
	}

	IEnumerator PlayOnMobile(){

		/* 
		 * 因為使用 Handheld.PlayFullScreenMovie，Android在播放影片是會載入原生播放器並暫停遊戲，
		 * 原生播放器會使用預設的ScreenOrientation。
		 * 
		 * 解決方法：
		 * 將預設的ScreenOrientation改為LandscapeLeft，
		 * 並在所有需要Portrait的Scene加入控制ScreenOrientation的程式碼強制為LandscapeLeft
		*/

		// DO NOT REMOVE THE CODE BELOW
		Screen.orientation = ScreenOrientation.LandscapeLeft;
		//
       
            //		print("開始播放片頭");
            Handheld.PlayFullScreenMovie("s2.mp4", Color.black, 
                FullScreenMovieControlMode.CancelOnInput, FullScreenMovieScalingMode.AspectFit);
       
		yield return null;

		StartCoroutine (LoadScene ());

		yield return null;
	}
}
