using UnityEngine;

public abstract class UIManager : MonoBehaviour 
{
	protected ScreenType screenType;
	protected bool isEnabled;
	public bool Enabled{get{return this.isEnabled;}}

	protected virtual void Awake()
	{

		#if UNITY_EDITOR
        this.SetScreenForComputer();
		#elif UNITY_IOS || UNITY_ANDROID
        this.SetScreenForMobile();
        #else
        this.SetScreenForComputer();
        #endif
	}
	private void OnEnable() 
	{
		UIController.onScreenChanged += this.OnScreenChanged;
	}

	private void OnScreenChanged(ScreenType screen)
	{
		if(this.screenType == screen)
		{
			this.isEnabled = true;
			this.OnScreenEnabled();
		}
		else
		{
			this.isEnabled = false;
			this.OnScreenDisabled();
		}
	}
	protected virtual void OnScreenEnabled(){}
	protected virtual void OnScreenDisabled(){}
	protected virtual void SetScreenForMobile(){}
	protected virtual void SetScreenForComputer(){}

	private void OnDisable() 
	{
		UIController.onScreenChanged -= this.OnScreenChanged;	
	}
}
