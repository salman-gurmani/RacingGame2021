using UnityEngine;
using UnityEngine.UI;
using Michsky.UI.ModernUIPack;

public class SettingsListner: MonoBehaviour {

    public GameObject soundBtn;
    public GameObject musicBtn;
    public GameObject tiltBtn;
    public Slider tiltSlider;

    private void Start()
    {
        soundBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameAudio;
        musicBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.GameMusic;
        tiltBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.TiltControl;
        tiltSlider.value = Toolbox.DB.prefs.TiltSensitivity;
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

    public void GyroToggel()
    {
        Toolbox.DB.prefs.TiltControl = !Toolbox.DB.prefs.TiltControl;
        if (Toolbox.DB.prefs.TiltControl)  RCC.SetMobileController(RCC_Settings.MobileController.Gyro);
        else RCC.SetMobileController(RCC_Settings.MobileController.TouchScreen);

        tiltBtn.GetComponentInChildren<SwitchManager>().isOn = Toolbox.DB.prefs.TiltControl;
    }

    public void Set_GyroSensitivity() 
    {
        Toolbox.DB.prefs.TiltSensitivity = tiltSlider.value;
        RCC_Settings.Instance.gyroSensitivity = tiltSlider.value;
    }
}
