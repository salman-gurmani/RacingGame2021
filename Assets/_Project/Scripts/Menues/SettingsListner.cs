using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class SettingsListner: MonoBehaviour {

    public GameObject soundBtn;
    public GameObject musicBtn;
    public GameObject tiltBtn;
    public Slider TiltSensitivity;

    private void Start()
    {
        soundBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameAudio;
        musicBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameMusic;
        tiltBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.TiltControl;

        TiltSensitivity.value = Toolbox.DB.prefs.GyroSensitivity;
        RCC_Settings.Instance.gyroSensitivity = TiltSensitivity.value;
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

    public void onTiltToggle()
    {
        Toolbox.DB.prefs.TiltControl = !Toolbox.DB.prefs.TiltControl;
        RCC_Settings.Instance.useAccelerometerForSteering = Toolbox.DB.prefs.TiltControl;
        tiltBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.TiltControl;
    }

    public void Adjust_TiltSensitivity()
    {
        Toolbox.DB.prefs.GyroSensitivity = TiltSensitivity.value;
        RCC_Settings.Instance.gyroSensitivity = TiltSensitivity.value;
    }
}



