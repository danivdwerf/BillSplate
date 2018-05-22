using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class SplashScreenManager : UIManager 
{
	public static SplashScreenManager singleton;
	public static System.Action OnSplashDone;

	[SerializeField]private VideoPlayer videoPlayer; 
	[SerializeField]private RawImage texture;
	[SerializeField]private VideoClip[] clips;
	[SerializeField]private bool skip;
	private byte index;
	
	protected override void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this);
		singleton = this;
		this.screenType = ScreenType.SPLASHSCREEN;
		this.texture.enabled = false;
	}

	protected override void OnScreenEnabled()
	{
		if(this.videoPlayer == null)
			return;
		
		if(this.skip)
		{
			this.index = (byte)this.clips.Length;
			this.UpdateVideo(null);
			return;
		}

		this.index = 0;
		this.videoPlayer.loopPointReached += this.UpdateVideo;
		this.UpdateVideo(null);
	}

	private void UpdateVideo(VideoPlayer player)
	{
		if(this.index == this.clips.Length)
		{
			if(OnSplashDone == null) 
			{
				UIController.singleton.ShowLoading(true);
				UIController.singleton.GoToScreen(ScreenType.NONE);
				return;
			}

			OnSplashDone();
			return;
		}
		StartCoroutine(PlayVideo());
	}

	private System.Collections.IEnumerator PlayVideo()
	{
		this.videoPlayer.clip = this.clips[this.index];
		this.videoPlayer.Prepare();
		while(!this.videoPlayer.isPrepared)
			yield return new WaitForEndOfFrame();
		
		this.texture.texture = this.videoPlayer.texture;
		this.texture.enabled = true;
		this.videoPlayer.Play();
		this.index++;
		yield return null;
	}

	protected override void OnScreenDisabled()
	{
		if(this.videoPlayer == null)
			return;

		this.videoPlayer.Stop();
		this.clips = null;
		Destroy(this.videoPlayer);
		Destroy(this.texture);
	}
}
