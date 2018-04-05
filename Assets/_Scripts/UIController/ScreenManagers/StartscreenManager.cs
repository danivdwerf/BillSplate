using UnityEngine;
using UnityEngine.UI;

public class StartscreenManager : UIManager 
{
	public static StartscreenManager singleton;
    
    [Header("Buttons")]
    [SerializeField]private Button startButton;
    [SerializeField]private Button joinButton;
    [SerializeField]private Button aboutButton;

    protected override void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this.gameObject);
        singleton = this;
        this.screenType = ScreenType.STARTSCREEN;

        base.Awake();
    }

    protected override void SetScreenForMobile()
    {
        this.startButton.gameObject.SetActive(false);
    }

    protected override void SetScreenForComputer()
    {
        this.joinButton.gameObject.SetActive(false);
    }

    protected override void OnScreenEnabled()
    {
        this.startButton.onClick.AddListener(()=> UIController.singleton.GoToScreen(ScreenType.CREATEROOMSCREEN));
        this.joinButton.onClick.AddListener(()=> UIController.singleton.GoToScreen(ScreenType.JOINSCREEN));
        this.aboutButton.onClick.AddListener(()=> UIController.singleton.GoToScreen(ScreenType.ABOUTSCREEN));
    }

    protected override void OnScreenDisabled()
    {
        this.startButton.onClick.RemoveAllListeners();
        this.joinButton.onClick.RemoveAllListeners();
        this.aboutButton.onClick.RemoveAllListeners();
    }
}
