using System.Collections;
using UnityEngine;
using UnityEngine.Events;
public class CommonMethods : MonoBehaviour
{
    public int startDelay = 1;

    public UnityEvent onStart;

    private void Start()
    {
        StartCoroutine(CR_Start());
    }

    IEnumerator CR_Start() {

        yield return new WaitForSeconds(startDelay);
        onStart.Invoke();
    }

    public void PlaySound(AudioClip _clip) {

        Toolbox.Soundmanager.PlaySound(_clip);
    }

    public void LoadSceneWithoutLoading(int _index) {

        Toolbox.GameManager.LoadScene(_index,  false, 0);
    }

    public void LoadSceneWithLoading(int _index)
    {
        Toolbox.GameManager.LoadScene(_index, true, 0);
    }

    public void DestroyAfterDelay(float _time)
    {
        Destroy(this.gameObject, _time);
    }

    public void EnableAnimator(Animator _anim) {

        _anim.enabled = true;
    }

    public void StartRace() {

        //Toolbox.GameplayScript.StartRaceHandling();
    }
}
