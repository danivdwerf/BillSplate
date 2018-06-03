using UnityEngine;
using UnityEngine.UI;

public class CanvasSetup : MonoBehaviour 
{
	private void Awake()
	{
		#if UNITY_EDITOR
		return;
		#endif

		Vector2 ratio = new Vector2(1280, 720);
		#if UNITY_IOS || UNITY_ANDROID
		ratio = new Vector2(720, 1280);
		#endif

		this.gameObject.GetComponent<CanvasScaler>().referenceResolution = ratio;
	}
}
