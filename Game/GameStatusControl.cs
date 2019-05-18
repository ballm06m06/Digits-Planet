using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameStatusControl : MonoBehaviour {

	private MainGameScript mainGame;
	private static bool isPaused;

	bool playOnce = true;

	public static bool IsPaused {
		get {
			return isPaused;
		}
	}

//	public Button returnButton;
//	public Image gameOverImg;
	public GameObject pauseButton;
	public GameObject pauseMenu;
	public GameObject StartInstruction;
	public tk2dTextMesh FinalScore;
    public Transform NewHighScore;
	public tk2dSprite gameOverScreen;
	public AudioClip endSound;

	float time;
	public float exitTime = 1f;
	int count;

	private int weightedScore = 0;
	private string GameType;

	// Use this for initialization
	void Start () {
        // DO NOT REMOVE THE CODE BELOW
        Screen.orientation = ScreenOrientation.Portrait;
        //

		mainGame = gameObject.GetComponent<MainGameScript> ();
		if (mainGame == null) {
			Debug.LogError ("Unable to load MainGameScript");
		}
			
		pauseMenu.SetActive (false);
		pauseButton.SetActive (false);
		gameOverScreen.gameObject.SetActive (false);
		StartInstruction.SetActive (true);

		// Show Start Instruction and pause game on default.
		Time.timeScale = 0f;
		isPaused = true;

        // Determin GameType
        GameType = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;      


	}
	
	// Update is called once per frame
	void Update () {
		if (mainGame.GameEnd) {
			if(playOnce)
				GameOverFunc ();
			playOnce = false;
		}

        if (Input.GetKeyDown (KeyCode.Escape) && pauseButton.activeSelf) {
//			print ("Escape pressed");
			OnPause ();
		}
			
			// Double tab to escape
			time += Time.deltaTime;
			if (Input.GetKeyDown (KeyCode.Escape)) 
			{
				count++;
				if (time <= exitTime && count == 2) { // End game
					Application.Quit ();
					#if UNITY_EDITOR
					UnityEditor.EditorApplication.isPlaying=false;
					#endif
				}
				time = 0f;
			}
			if (time > exitTime)
				count = 0;

	}


	void GameOverFunc(){
		weightedScore = ScoreWeight ();
		FinalScore.text = "Score: " + weightedScore;

        switch (GameType)
        {
            case "Addition":
                //show you got a new high score in add
                if (ScoreBoard.addScoreList[0] < weightedScore)
                {
                    NewHighScore.gameObject.SetActive(true);
                }
                else
                {
                    NewHighScore.gameObject.SetActive(false);
                }
                break;
            case "Subtraction":
                //show you got a new high score in sub
                if (ScoreBoard.subScoreList[0] < weightedScore)
                {
                    NewHighScore.gameObject.SetActive(true);
                }
                else
                {
                    NewHighScore.gameObject.SetActive(false);
                }
                break;
            case "Division":
                //show you got a new high score in div
                if (ScoreBoard.divScoreList[0] < weightedScore)
                {
                    NewHighScore.gameObject.SetActive(true);
                }
                else
                {
                    NewHighScore.gameObject.SetActive(false);
                }
                break;
            default:
                Debug.LogError("NewHighScore Error");
                break;
        }


        if (SettingsScript.IsBGMMute)
			AudioSource.PlayClipAtPoint (endSound, new Vector3 (), 0f);
		else
			AudioSource.PlayClipAtPoint (endSound, new Vector3 (), 1f);
		gameOverScreen.gameObject.SetActive (true);
		GameObject.Find ("GameBGM").GetComponent<AudioSource> ().Pause ();
//		returnButton.gameObject.SetActive (true);
		chooseMode.setDifficulty=0;

		// Show fade times
		print("Fade Times:"+gameObject.GetComponent<ScoreControlAbstract>().FadeCount);
	}

	int ScoreWeight(){
		switch (chooseMode.setDifficulty) {
		case 1:	//Hard
			return ScoreScript.Score * 10;
		case 2: //Medium
			return ScoreScript.Score * 5;
		case 3: //Easy
			return ScoreScript.Score * 1;
		default:
			Debug.LogError ("無法加權分數");
			return 0;
		}
	}

	//
	// Start Instruction Function
	//
	void OnGameStart(){
		StartInstruction.SetActive (false);
		pauseButton.SetActive (true);
		OnResume ();
	}

	//
	// End Game: Return to menu
	//
	void ReturnButtonPress(){
//		Debug.Log ("ReturnButton Pressed. Return to menu");

		// Add score to database
		chooseMode.setGameMode = GameType;
		ScoreBoard.Add(weightedScore, GameType);
		LevelControl.AddExperience (weightedScore, GameType);
		GameType = null;


//		print("WeightedScore: " + weightedScore);

		// Load menu
		UnityEngine.SceneManagement.SceneManager.LoadScene ("Menu", UnityEngine.SceneManagement.LoadSceneMode.Single);
	}

	void ChangePictureWhenPress(tk2dUIItem who){
		string spriteName = who.gameObject.GetComponentInChildren<tk2dSprite> ().GetCurrentSpriteDef().name;
		who.gameObject.GetComponentInChildren<tk2dSprite> ().SetSprite (spriteName + "hit");
		who.gameObject.GetComponentInChildren<tk2dTextMesh> ().text = "";
	}

	//
	// Pause Menu Function
	//
	void OnPause(){
//		print ("Game Paused");
		Time.timeScale = 0;
		isPaused = true;
		pauseMenu.SetActive (true);
	}
	void OnResume(){
//		print ("Game Resume");
		Time.timeScale = 1;
		isPaused = false;
		pauseMenu.SetActive (false);
	}
	void OnSettings(){
//		print ("Enter settings");
		for (int i = 0; i < 4; i++) {
			pauseMenu.transform.GetChild (i).gameObject.SetActive (false);
		}
		for (int i = 4; i < pauseMenu.transform.childCount; i++) {
			pauseMenu.transform.GetChild (i).gameObject.SetActive (true);
		}
	
	}
	// Settings Option Function
	void OnGoPressed(){
//		print ("Return to pause menu");
		for (int i = 0; i < 4; i++) {
			pauseMenu.transform.GetChild (i).gameObject.SetActive (true);
		}		
		for (int i = 4; i < pauseMenu.transform.childCount; i++) {
			pauseMenu.transform.GetChild (i).gameObject.SetActive (false);
		}
	}
	void OnRestart(){
//		print ("Reload Level");
		UnityEngine.SceneManagement.SceneManager.LoadScene (UnityEngine.SceneManagement.SceneManager.GetActiveScene ().name);
		isPaused = false;
	}
	void OnQuit(){
//		print ("Quit Game");
		mainGame.GameEnd = true;
	}

   
}
	
