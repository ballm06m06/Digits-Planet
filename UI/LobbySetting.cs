using UnityEngine;
using System.Collections;

public class LobbySetting : MonoBehaviour {

    public Transform SFXon, BGMon, SFXoff, BGMoff;

    private  bool isBGMMute = false;
    private  bool isSFXMute = false;



	void Start () {
	
	}
	
    void MuteSFX()
    {
        if (isSFXMute)
        {   
            isSFXMute = false;
            GameObject.Find("tk2dUIAudioManager").GetComponent<AudioSource>().volume = 1;
            SFXon.gameObject.SetActive(true);
            SFXoff.gameObject.SetActive(false);
        }
        else
        {
            isSFXMute = true;
            GameObject.Find("tk2dUIAudioManager").GetComponent<AudioSource>().volume = 0;
            SFXon.gameObject.SetActive(false);
            SFXoff.gameObject.SetActive(true);
        }
    }
    void MuteBGM()
    {
        if (isBGMMute) 
        {
            isBGMMute = false;
            GameObject.Find ("GameBGM").GetComponent<AudioSource> ().volume = 1;
            BGMon.gameObject.SetActive(true);
            BGMoff.gameObject.SetActive(false);

        } else {
            isBGMMute = true;
            GameObject.Find ("GameBGM").GetComponent<AudioSource> ().volume = 0;
            BGMon.gameObject.SetActive(false);
            BGMoff.gameObject.SetActive(true);
        }

    }

}
