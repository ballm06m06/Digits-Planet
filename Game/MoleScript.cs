using UnityEngine;
using System.Collections;

public class MoleScript : MonoBehaviour 
{
//	public const int SIZE = 3;
	public tk2dClippedSprite[] sprite = new tk2dClippedSprite[3];

//	public tk2dTextMesh numberText;
//	public AudioClip moleUp;
//	public AudioClip moleDown;

	private float height;
	private float speed;
	private float timeLimit;
	private Rect spriteRec;
	private bool isWhacked;				// is it hit?
	private float transformY;
	private int molePoint = 0; 			// point on mole body
	private int moleType = -1;			// Select the mole
	private string spriteType;

	// To enable to hit
	private bool isActivate;

	public bool IsActivate {
		get {
			return isActivate;
		}
	}


	// To recognize the transform for hit
	private Transform colliderTransform;

	public Transform ColliderTransform
	{
		get
		{
			return colliderTransform;
		}
	}
	
	// Trigger the mole.  It is now 'active' and the sprite is set to the default mole sprite, just in case it isn't.
	
	public void Trigger(float tl,int mp,int type)
	{
		moleType = type;

		sprite[moleType].gameObject.SetActive (true);
		isWhacked = false;
		spriteType = "monster"+(type+1);
		sprite[moleType].SetSprite(spriteType);
		timeLimit = tl;
		//Set points on mole
		molePoint = mp;
		isActivate = true;

		//Start the animation
		StartCoroutine (MainLoop());
	}
		
	void Start()
	{
		timeLimit = 1.0f;
		speed = 2.0f;

		for (int i = 0; i < sprite.Length; i++) {
			
			// Get the 'size' of the mole sprite
			Bounds bounds = sprite [i].GetUntrimmedBounds ();
			height = bounds.max.y - bounds.min.y;
		
			// We want the mole to be fully clipped on the Y axis initially.
			spriteRec = sprite [i].ClipRect;
			spriteRec.y = 1.0f;
			sprite [i].ClipRect = spriteRec;
					
			//Move the mole sprite into the correct place relative to the hole
			Vector3 localPos = sprite [i].transform.localPosition;
			transformY = localPos.y;
			localPos.y = transformY - (height * sprite [i].ClipRect.y);
			sprite [i].transform.localPosition = localPos;
		
			sprite [i].gameObject.SetActive (false);
			sprite [i].GetComponentInChildren<tk2dTextMesh> ().text = "";


		}
		// Add mole to the main game script's mole container
		MainGameScript.Instance.RegisterMole(this);
	}

	// Update is called once per frame
	void Update ()
	{
		// In order to prevent array out of bound. Just reset it to 0.
		if (moleType >= sprite.Length) {
			Debug.LogWarning ("Array Out Of Bound, reset moleType to 0");
			moleType = 0;
		}

	}
	
	// Main loop for the sprite.  Move up, then wait, then move down again. Simple.
	private IEnumerator MainLoop()
	{
		yield return StartCoroutine(MoveUp());
		yield return StartCoroutine(WaitForHit());
		yield return StartCoroutine(MoveDown());
	}
	
	// As it 'moves up', we see more of the sprite and the position of it has to be adjusted so that the 'bottom' of the sprite is in line with the hole.
	private IEnumerator MoveUp()
	{	
//		AudioSource.PlayClipAtPoint(moleUp, new Vector3());

		while(spriteRec.y > 0.0f)
		{
			spriteRec = sprite[moleType].ClipRect;
			float newYPos = spriteRec.y - speed * Time.deltaTime;
			spriteRec.y = newYPos < 0.0f ? 0.0f : newYPos;
			sprite[moleType].ClipRect = spriteRec;
			
			Vector3 localPos = sprite[moleType].transform.localPosition;
			localPos.y = transformY - (height * sprite[moleType].ClipRect.y);
			sprite[moleType].transform.localPosition = localPos;
			
			yield return null;
		}

		//顯示mole身上的東西
		sprite[moleType].GetComponentInChildren<tk2dTextMesh> ().text = molePoint.ToString ();


	}
	
	// Give the player a chance to hit the mole.
	private IEnumerator WaitForHit()
	{
		// Set collider in order to detect hit
		colliderTransform = sprite [moleType].transform;

		float time = 0.0f;
		
		while(!isWhacked && time < timeLimit)
		{
			time += Time.deltaTime;
			yield return null;
		}
	}
	
	// Same as the MoveUp function but the other way around!	
	private IEnumerator MoveDown()
	{		
		//先讓mole身上的數字消失，然後再下降
		sprite[moleType].GetComponentInChildren<tk2dTextMesh> ().text = "";

		while(spriteRec.y < 1.0f)
		{ 
			spriteRec = sprite[moleType].ClipRect;
			float newYPos = spriteRec.y + speed * Time.deltaTime;
			spriteRec.y = newYPos > 1.0f ? 1.0f : newYPos;
			sprite[moleType].ClipRect = spriteRec;
			
			Vector3 localPos = sprite[moleType].transform.localPosition;
			localPos.y = transformY - (height * sprite[moleType].ClipRect.y);
			sprite[moleType].transform.localPosition = localPos;
			
			yield return null;
		}

//		AudioSource.PlayClipAtPoint(moleDown, new Vector3());

		// This will stop coroutine
		sprite [moleType].gameObject.SetActive (false);
		isActivate = false;

	}
	
	// Mole has been hit
	public void Whack()
	{
		isWhacked = true;
		spriteType +="hit";
		sprite[moleType].SetSprite(spriteType);
		//打爆之後可以在此處讓數字消失
	}
		
	public bool Whacked
	{
		get
		{
			return isWhacked;	
		}
	}

	public int MolePoint {
		get {
			return molePoint;
		}
	}
}
