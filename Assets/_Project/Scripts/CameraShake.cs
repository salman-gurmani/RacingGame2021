using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.PostProcessing;
public class CameraShake : MonoBehaviour
{
	// Transform of the camera to shake. Grabs the gameObject's transform
	// if null.
	public Transform camTransform;

	// How long the object should shake for.
	public float shakeDuration = 0f;
	public bool continousShake = false;
	// Amplitude of the shake. A larger value shakes the camera harder.
	public float shakeAmount = 0.7f;
	public float continousShakeAmount = 0.5f;
	public float decreaseFactor = 1.0f;
	public RCC_Camera mainCamera;
	public GameObject motionBlur;

	Vector3 originalPos;

	void Awake()
	{
		mainCamera = this.gameObject.GetComponent<RCC_Camera>();
		

		if (camTransform == null)
		{
			camTransform = GetComponent(typeof(Transform)) as Transform;
		}
	}

	void OnEnable()
	{
		originalPos = camTransform.localPosition;
	}

	void Update()
	{
		if (shakeDuration > 0)
		{
			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}
		else
		{
			shakeDuration = 0f;
			camTransform.localPosition = originalPos;
		}
		if (continousShake&&mainCamera.playerCar&& mainCamera.playerCar.speed>200)
        {
			camTransform.localPosition = originalPos + Random.insideUnitSphere * (continousShakeAmount+ mainCamera.playerCar.speed/20000);
			//motionBlur.SetActive(true);
		}
		//else if(mainCamera.playerCar.speed < 200)
  //      {
		//	motionBlur.SetActive(false);
		//}
	}

	IEnumerator Shake() {

		yield return new WaitForSeconds(0);

		while (shakeDuration > 0) {

			camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

			shakeDuration -= Time.deltaTime * decreaseFactor;
		}

		shakeDuration = 0f;
		camTransform.localPosition = originalPos;
	}
}