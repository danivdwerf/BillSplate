using UnityEngine;

public enum ScreenType
{
	NONE = -1,
	SPLASHSCREEN = 0,
	STARTSCREEN = 1,
	LOBBYSCREEN = 2,
	JOINSCREEN = 3,
	GAMESCREEN = 4,
	ABOUTSCREEN = 5,
	SETTINGSSCREEN = 6
};

public class UIController : MonoBehaviour 
{
	public static UIController singleton;
	public static System.Action<ScreenType> onScreenChanged;

	[SerializeField]private GameObject[] screens;
	[SerializeField]private GameObject loadingScreen;

	private void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this);
		singleton = this;
	}

	public void GoToScreen(ScreenType screen)
	{
		byte newIndex = (byte)screen;
		int len = this.screens.Length;
		for(byte i = 0; i < len; i++)
			this.screens[i].SetActive((i == newIndex));

		if(onScreenChanged != null)
			onScreenChanged(screen);
	}

	public void ShowLoading(bool value)
	{
		loadingScreen.SetActive(value);
	}
}
