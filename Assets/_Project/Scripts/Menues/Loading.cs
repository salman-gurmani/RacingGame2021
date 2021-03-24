using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class Loading : MonoBehaviour {

	public Image loadingBar;
	
	[SerializeField]
	private bool isTemporaryLoading = false;

    public float delayTime = 2;

    public bool IsTemporaryLoading { get => isTemporaryLoading; set => isTemporaryLoading = value; }

    private void OnEnable()
    {
		//AdsManager.instance.RequestBannerWithSpecs(IronSourceBannerSize.RECTANGLE, IronSourceBannerPosition.TOP);
		Toolbox.Soundmanager.PlaySound(Toolbox.Soundmanager.Loading);
	}

	private void OnDestroy()
    {
		AdsManager.instance.HideBannerAd();
    }

    private void Start()
    {
		if (isTemporaryLoading) {

			DontDestroyOnLoad(this.gameObject);
		}
    }

    void Update () {

		if (loadingBar && Toolbox.GameManager.async != null) {
		
			loadingBar.fillAmount = Toolbox.GameManager.async.progress;
		}

		if (isTemporaryLoading) {

			delayTime -= Time.deltaTime;

			if (delayTime <= 0) {

				isTemporaryLoading = false;

				if (SceneManager.GetActiveScene().buildIndex == Toolbox.GameManager.nextSceneIndex) {

					Destroy(this.gameObject);

				}

			}
		}
	}

}
