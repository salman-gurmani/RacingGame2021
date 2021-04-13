using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	[Header("Audio Sources")]
	public AudioSource audioo;
	public AudioSource bgMusicSource;

	[Header("BG Clips")]
	public AudioClip menuBG;
	public AudioClip gameBG;

    [Header("Sound Clips")]
    public AudioClip Select;
    public AudioClip buttonPressYes;
	public AudioClip buttonPressNo;
    public AudioClip back;
	public AudioClip fail;
	public AudioClip complete;
	public AudioClip Loading;
	public AudioClip [] Female;
    public AudioClip[] Male;
	public AudioClip [] Male_Shouting;
	public AudioClip [] Female_Shouting;
	public AudioClip DoorOpen, DoorClose;
	public AudioClip horn, WaterExplosion;
	public AudioClip M_TaxiCall, F_TaxiCall;
	public AudioClip Repair;
	public AudioClip ThankYou_F, ThankYou_M;

	void Start () {

		PlayBGSound(menuBG);
	
		UpdateSoundStatus();
		UpdateMusicStatus();

	}

	public void UpdateSoundStatus()
	{
		audioo.mute = !Toolbox.DB.prefs.GameAudio;
	}

	public void UpdateMusicStatus() {

		bgMusicSource.mute = !Toolbox.DB.prefs.GameMusic;
	}

	public void Pause_All(){

		this.audioo.Pause ();
		this.bgMusicSource.Pause ();
	}

	public void UnPause_All(){

		this.audioo.UnPause ();
		this.bgMusicSource.UnPause ();

	}
    public void Pause_Sound()
    {
        this.audioo.Pause();
    }

    public void PlayBGSound(AudioClip _clip) {

        this.bgMusicSource.clip = _clip;

        this.bgMusicSource.Play();

        this.bgMusicSource.loop = true;
    }

    public void PlaySound(AudioClip _clip){
		
		if (_clip != null)
			audioo.PlayOneShot (_clip);
	}


	public void Stop_PlayingSound(){
		audioo.Stop ();
	}

	public void Stop_PlayingBGSound()
	{
		bgMusicSource.Stop();
	}
}
