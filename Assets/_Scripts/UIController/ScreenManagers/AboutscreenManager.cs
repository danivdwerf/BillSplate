using UnityEngine;

public class AboutscreenManager : UIManager 
{
	public static AboutscreenManager singleton;
	
	protected override void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this);
		singleton = this;
		
		this.screenType = ScreenType.ABOUTSCREEN;
		base.Awake();
	}

	protected override void OnScreenEnabled()
	{

	}

	protected override void OnScreenDisabled()
	{
		
	}
}
