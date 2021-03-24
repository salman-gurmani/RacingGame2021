using UnityEngine;

public class ReviewListner : MonoBehaviour
{

    public void onPressReview()
    {
        Toolbox.GameManager.Analytics_Design("Review-Pressed");

        Application.OpenURL(Constants.appLink);
        Toolbox.DB.prefs.AppRated = true;
        OnPress_Close();
    }

    public void OnPress_Close()
    {
        Destroy(this.gameObject);
    }
}
