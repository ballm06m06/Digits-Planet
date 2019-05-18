using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainGameScript : MonoBehaviour
{	
	private List<MoleScript> moles = new List<MoleScript>();
	private bool gameEnd;

	public bool GameEnd {
		get {
			return gameEnd;
		}
		set {
			gameEnd = value;
		}
	}


	public float hitTimeLimit = 5f;
	public Vector2 TimeWaitBeforeInstaniate = new Vector2 (5f, 1000f);
	public int moleLimit = 5;
	public Camera gameCam;
	public tk2dSpriteAnimator dustAnimator;
	public AudioClip moleHit;
	public tk2dTextMesh timeDisplay;
	public int EasyModeTime = 30;
    public int MediumModeTime = 60;
    public int HardModeTime = 120;
	public static int gameTime;

	private PointGenerator pointGenerator;

	// Treat this class as a singleton.  This will hold the instance of the class.
	private static MainGameScript instance;
	
	public static MainGameScript Instance
	{
		get
		{
			// This should NEVER happen, so we want to know about it if it does 
			if(instance == null)
			{
				Debug.LogError("MainGameScript instance does not exist");	
			}
			return instance;	
		}
	}

	void Awake()
	{
		instance = this; 
		pointGenerator = gameObject.GetComponent<PointGenerator> ();
		if (pointGenerator == null) {
			Debug.LogWarning ("Did not fount Point Generator");
		}
		
        if (chooseMode.setDifficulty == 3)
        {
            gameTime = EasyModeTime;
        }
        if (chooseMode.setDifficulty == 2)
        {
            gameTime = MediumModeTime;
        }
        if (chooseMode.setDifficulty == 1)
        {
            gameTime = HardModeTime;
        }
	}
	
	IEnumerator Start () 
	{

		yield return 0;  // Let other settings load first, wait for next frame

		gameEnd = false;

		if (moleLimit > moles.Count) {
			Debug.LogWarning ("Change moleLimit to maximum holes");
			moleLimit = moles.Count;
		}
		
		if (hitTimeLimit < 1f) {
			Debug.LogWarning ("Change hit time to 1s");
			hitTimeLimit = 1f;
		}
		// Yield here to give everything else a chance to be set up before we start our main game loop
		
		yield return null;  // wait for the next frame!

		dustAnimator.gameObject.SetActive(false);
		StartCoroutine(MainGameLoop());
	}
	
	void Update()
	{
		// Check to see if mouse has been clicked, and if so check to see if it has 'hit' any of the moles, and check which mole.
		if(Input.GetButtonDown ("Fire1") && !gameEnd && !GameStatusControl.IsPaused)
		{
			Ray ray = gameCam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();
			
			if(Physics.Raycast(ray, out hit))
			{
				foreach(MoleScript mole in moles)
				{
					if(mole.IsActivate && !mole.Whacked && mole.ColliderTransform == hit.transform)
					{
						if (!SettingsScript.IsSFXMute)
							AudioSource.PlayClipAtPoint (moleHit, new Vector3 (), 1f);
						ScoreScript.HitPoint = mole.MolePoint;
						mole.Whack();
						StartCoroutine(CallAnim(mole));
					}
				}
			}
		}

		//Control game time
		float timeLeft = DownCounter();
		if (timeLeft <= 0) {
//			print ("Gameover");
			gameEnd = true;
		} else {
			timeDisplay.text = ((int)timeLeft).ToString();
		}


	}
	
	private IEnumerator MainGameLoop()
	{
		int randomMole;
		
		while(!gameEnd)
		{
			yield return StartCoroutine(OkToTrigger());
			yield return new WaitForSeconds((float)Random.Range((int)TimeWaitBeforeInstaniate.x, (int)TimeWaitBeforeInstaniate.y) / 1000.0f);
			
			// Check if there are any free moles to choose from
			int availableMoles = 0;
			for (int i = 0; i < moles.Count; ++i) {
				if (!moles[i].IsActivate) {
					availableMoles++;
				}
			}

			if (availableMoles > 0) {
				
				randomMole = (int)Random.Range(0, moles.Count);			
				while(moles[randomMole].IsActivate)
				{
					randomMole = (int)Random.Range(0, moles.Count);
				}
				//return point-x and sequence-y
				Vector2 pAnds = pointGenerator.numberGenerator ();

				int point = (int)pAnds.x;
				int type = (int)pAnds.y;
//				print ("point:" + point + ", type" + type);

				// Wait until it is Inactivate
				while(moles[randomMole].IsActivate)
				{
					yield return null;
				}
				// Trigger the mole
				moles [randomMole].Trigger (hitTimeLimit, point, type);
//				hitTimeLimit -= hitTimeLimit <= 0.0f ? 0.0f : 0.01f;	// Less time to hit the next mole
			}
						
			yield return null;
		}
	}
	
	public void RegisterMole(MoleScript who)
	{
		moles.Add(who);
	}
	
	// Currently only 3 moles at a time can be active.  So if there are that many, then we can't trigger another one...
	private IEnumerator OkToTrigger()
	{
		int molesActive;

		do
		{
			yield return null;
			molesActive = 0;
			
			foreach(MoleScript mole in moles)
			{
				molesActive += mole.IsActivate ? 1 : 0;
			}
		}
		while(molesActive >= moleLimit);

		yield break;
	}
	
	private IEnumerator CallAnim(MoleScript mole)
	{
		yield return new WaitForSeconds(0.25f);
		
		tk2dSpriteAnimator newAnimator;
		newAnimator = Instantiate(dustAnimator, new Vector3(mole.transform.position.x, mole.transform.position.y, dustAnimator.transform.position.z), dustAnimator.transform.rotation) as tk2dSpriteAnimator; 
		newAnimator.gameObject.SetActive(true);
		newAnimator.Play("DustCloud");
		
		while(newAnimator.IsPlaying("DustCloud"))
		{
			yield return null;	
		}
		
		Destroy(newAnimator.gameObject);
	}


	private float DownCounter(){

		return gameTime - Time.timeSinceLevelLoad;
	}

	public static void GameTimeChange(int amount)
	{
		gameTime += amount;
	}

}
