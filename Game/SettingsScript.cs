using UnityEngine;
using System.Collections;

public class SettingsScript : MonoBehaviour {

	private static bool isBGMMute;

	public static bool IsBGMMute {
		get {
			return isBGMMute;
		}
	}

	private static bool isSFXMute;

	public static bool IsSFXMute {
		get {
			return isSFXMute;
		}
	}

	public GameObject pauseMenuInMenu;
    //大廳中的setting
    public Transform SFXon, BGMon, SFXoff, BGMoff;
    //遊戲中的setting
    public tk2dSprite SFXButton;
	public tk2dSprite BGMButton;
	// Mute=3, Unmute=0

	void Start(){
		if (isBGMMute) { 	// Mute
			GameObject.Find ("GameBGM").GetComponent<AudioSource> ().volume = 0;
			BGMButton.spriteId = 3;

        } else {			// Unmute
			BGMButton.spriteId = 0;
   
		}
		if (isSFXMute) {	// Mute
			GameObject.Find ("tk2dUIAudioManager").GetComponent<AudioSource> ().volume = 0;
			SFXButton.spriteId = 3;



		} else {			// Unmute
			SFXButton.spriteId = 0;

          
		}

	}
	void OnMuteSFX(tk2dUIItem who){
		if (isSFXMute) {
//			print ("SFX Unmute");
			isSFXMute = false;
			who.GetComponent<tk2dSprite> ().spriteId = 0;
			GameObject.Find ("tk2dUIAudioManager").GetComponent<AudioSource> ().volume = 1;

		} else {
//			print("SFX mute");
			isSFXMute = true;
			who.GetComponent<tk2dSprite> ().spriteId = 3;
			GameObject.Find ("tk2dUIAudioManager").GetComponent<AudioSource> ().volume = 0;
		}
	}
	void OnMuteBGM(tk2dUIItem who){
		if (isBGMMute) {
//			print ("BGM Unmute");
			isBGMMute = false;
			who.GetComponent<tk2dSprite> ().spriteId = 0;
			GameObject.Find ("GameBGM").GetComponent<AudioSource> ().volume = 1;

		} else {
//			print("BGM mute");
			isBGMMute = true;
			who.GetComponent<tk2dSprite> ().spriteId = 3;
			GameObject.Find ("GameBGM").GetComponent<AudioSource> ().volume = 0;
		}

	}

    void MuteSFX()
    {
        if (isSFXMute)
        {
            isSFXMute = false;
            GameObject.Find("tk2dUIAudioManager").GetComponent<AudioSource>().volume = 1;
            SFXon.gameObject.SetActive(false);
            SFXoff.gameObject.SetActive(true);
        }
        else
        {
            isSFXMute = true;
            GameObject.Find("tk2dUIAudioManager").GetComponent<AudioSource>().volume = 0;
            SFXon.gameObject.SetActive(true);
            SFXoff.gameObject.SetActive(false);
        }
    }
    void MuteBGM()
    {
        if (isBGMMute) 
        {
            isBGMMute = false;
            GameObject.Find ("GameBGM").GetComponent<AudioSource> ().volume = 1;
            BGMon.gameObject.SetActive(false);
            BGMoff.gameObject.SetActive(true);

        } else {
            isBGMMute = true;
            GameObject.Find ("GameBGM").GetComponent<AudioSource> ().volume = 0;
            BGMon.gameObject.SetActive(true);
            BGMoff.gameObject.SetActive(false);
        }

    }

	// Settings in Menu
	void OnOKPressedInMenu(){
		pauseMenuInMenu.SetActive (false);
	}
	void OnSettingsInMenu(){
		pauseMenuInMenu.SetActive (true);
	}
}
