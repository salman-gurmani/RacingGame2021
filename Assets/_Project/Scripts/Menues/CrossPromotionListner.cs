using UnityEngine;
using UnityEngine.UI;

public class CrossPromotionListner : MonoBehaviour
{
    public RawImage promotionTexture;
    private int cpIndex = 0;

    private void Start()
    {
        DontDestroyOnLoad(this);
        cpIndex = Toolbox.DB.GetMyAppGameData().cpIndex;

        if (Toolbox.DB.serverPrefs.cpData[cpIndex].appBundle == Application.identifier)
            IncreaseCpIndex();

        promotionTexture.texture = Toolbox.DB.serverPrefs.cpData[cpIndex].tex;

        if (promotionTexture.texture == null)
            OnPress_Close();
    }

    public void OnClickLink() {

        Application.OpenURL(Toolbox.DB.serverPrefs.cpData[cpIndex].appLink);

        OnPress_Close();
    }

    void IncreaseCpIndex() {

        if (Toolbox.DB.serverPrefs.cpData.Length <= 1)
            return;

        if (cpIndex + 1 >= Toolbox.DB.serverPrefs.cpData.Length)
            cpIndex = 0;

        cpIndex++;

        if (Toolbox.DB.serverPrefs.cpData[cpIndex].appBundle == Application.identifier)
            IncreaseCpIndex();

        Toolbox.DB.GetMyAppGameData().cpIndex = cpIndex;
    }

    public void OnPress_Close() {

        IncreaseCpIndex();
        Destroy(this.gameObject);
    }
}
