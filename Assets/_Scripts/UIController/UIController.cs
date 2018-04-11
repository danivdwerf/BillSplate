using UnityEngine;

public enum ScreenType
{
	NONE = -1,
	STARTSCREEN = 0,
	CREATEROOMSCREEN = 1,
	LOBBYSCREEN = 2,
	JOINSCREEN = 3,
	GAMESCREEN = 4,
	ABOUTSCREEN = 5
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
		byte len = (byte)this.screens.Length;
		for(byte i = 0; i < len; i++)
		{
			if(i == newIndex)
			{
				this.screens[i].SetActive(true);
				continue;
			}
			this.screens[i].SetActive(false);
		}

		if(onScreenChanged != null)
			onScreenChanged(screen);
	}

	public void ShowLoading(bool value)
	{
		loadingScreen.SetActive(value);
	}
}
