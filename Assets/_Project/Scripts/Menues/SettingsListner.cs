using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class SettingsListner: MonoBehaviour {

    public GameObject soundBtn;
    public GameObject musicBtn;

    private void Start()
    {
        soundBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameAudio;
        musicBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameMusic;
    }

    public void OnPress_Close()
    {
        Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.buttonPressNo);
        Destroy(this.gameObject);
    }
    public void OnMusicToggle()
    {
        Toolbox.DB.prefs.GameMusic = !Toolbox.DB.prefs.GameMusic;
        Toolbox.Soundmanager.UpdateMusicStatus();

        musicBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameMusic;
    }

    public void OnSoundToggle()
    {
        Toolbox.DB.prefs.GameAudio = !Toolbox.DB.prefs.GameAudio;
        Toolbox.Soundmanager.UpdateSoundStatus();

        soundBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameAudio;
    }
}
