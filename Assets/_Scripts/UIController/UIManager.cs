using UnityEngine;

public abstract class UIManager : MonoBehaviour 
{
	protected ScreenType screenType;

	private void OnEnable() 
	{
		UIController.onScreenChanged += this.OnScreenChanged;	
	}

	private void OnScreenChanged(ScreenType screen)
	{
		if(screenType == screen)
			this.OnScreenEnabled();
		else
			this.OnScreenDisabled();
	}
	protected virtual void OnScreenEnabled(){}
	protected virtual void OnScreenDisabled(){}

	private void OnDisable() 
	{
		UIController.onScreenChanged -= this.OnScreenChanged;	
	}
}
