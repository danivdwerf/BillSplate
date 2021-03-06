﻿using UnityEngine;

public abstract class UIManager : MonoBehaviour 
{
	protected ScreenType screenType;

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
		if(screenType == screen)
			this.OnScreenEnabled();
		else
			this.OnScreenDisabled();
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
