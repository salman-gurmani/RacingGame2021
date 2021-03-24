using UnityEngine;
using UnityEngine.UI;

public class RewardListner : MonoBehaviour
{
    public Sprite [] rewardImages;
    public Image img;
    private void Start()
    {
        img.SetNativeSize();
        img.sprite = rewardImages[Mathf.RoundToInt(Random.Range(0, rewardImages.Length - 1))];
    }
}
