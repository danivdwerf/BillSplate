using UnityEngine;
using UnityEngine.UI;

public class StartscreenManager : UIManager 
{
	public static StartscreenManager singleton;

    [Header("Computer")]
    [SerializeField]private GameObject masterView;
    [SerializeField]private Button createGame;
    [SerializeField]private Button aboutGameMaster;

    [Space(10)]
    [Header("Mobile")]
    [SerializeField]private GameObject clientView;
    [SerializeField]private Button joinGame;
    [SerializeField]private Button aboutGameClient;

    protected override void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this);
        singleton = this;
        
        this.screenType = ScreenType.STARTSCREEN;
        base.Awake();
    }   

    /// <summary>
    /// Hides and unsets the Computer UI and activates the Mobile UI
    /// </summary>
    protected override void SetScreenForMobile()
    {
        this.masterView.SetActive(false);
        this.masterView = null;

        this.createGame.gameObject.SetActive(false);
        this.createGame = null;

        this.aboutGameMaster.gameObject.SetActive(false);
        this.aboutGameMaster = null;

        this.clientView.SetActive(true);
        this.joinGame.gameObject.SetActive(true);
        this.aboutGameClient.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides and unsets the Mobile UI and activates the Computer UI
    /// </summary>
    protected override void SetScreenForComputer()
    {
        this.clientView.SetActive(false);
        this.clientView = null;

        this.joinGame.gameObject.SetActive(false);
        this.joinGame = null;

        this.aboutGameClient.gameObject.SetActive(false);
        this.aboutGameClient = null;

        this.masterView.SetActive(true);        
        this.createGame.gameObject.SetActive(true);
        this.aboutGameMaster.gameObject.SetActive(true);
    }

    /// <summary>
    /// Add eventlisteners to the buttons
    /// </summary>
    protected override void OnScreenEnabled()
    {
        if(this.masterView != null)
        {
            this.createGame.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.CREATEROOMSCREEN));
            this.aboutGameMaster.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.ABOUTSCREEN));
        }
        else
        {
            this.joinGame.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.JOINSCREEN));
            this.aboutGameClient.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.ABOUTSCREEN));
        }
    }

    /// <summary>
    /// Remove eventlisteners from the buttons
    /// </summary>
    protected override void OnScreenDisabled()
    {
        if(this.masterView != null)
        {
            this.createGame.onClick.RemoveAllListeners();
            this.aboutGameMaster.onClick.RemoveAllListeners();
        }
        else
        {
            this.joinGame.onClick.RemoveAllListeners();
            this.aboutGameClient.onClick.RemoveAllListeners();
        }
    }
}
