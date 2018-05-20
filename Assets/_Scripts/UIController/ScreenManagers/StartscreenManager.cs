using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class StartscreenManager : UIManager 
{
	public static StartscreenManager singleton;

    [Header("Computer")]
    [SerializeField]private GameObject masterView;
    [SerializeField]private Button createGame;
    [SerializeField]private Button settings;
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

        this.settings.gameObject.SetActive(false);
        this.settings = null;

        this.clientView.SetActive(true);
        this.joinGame.gameObject.SetActive(true);
        this.aboutGameClient.gameObject.SetActive(true);

        LavaManager.singleton.SetScreen(LavaManager.LavaScreen.Mobile);
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
        this.settings.gameObject.SetActive(true);

        LavaManager.singleton.SetScreen(LavaManager.LavaScreen.Desktop);
    }

    /// <summary>
    /// Add eventlisteners to the buttons
    /// </summary>
    protected override void OnScreenEnabled()
    {
        if(this.masterView != null)
        {
            this.createGame.onClick.AddListener(this.CreateGame);
            this.aboutGameMaster.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.ABOUTSCREEN));
            this.settings.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.SETTINGSSCREEN));
            LavaManager.singleton.StartPlaying(this.masterView.transform);
        }
        else
        {
            this.joinGame.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.JOINSCREEN));
            this.aboutGameClient.onClick.AddListener(()=>UIController.singleton.GoToScreen(ScreenType.ABOUTSCREEN));
            LavaManager.singleton.StartPlaying(this.clientView.transform);
        }
    }

    public void CreateGame()
    {
        UIController.singleton.ShowLoading(true);
        string roomCode = StringExtensions.Random(5, CharacterTypes.LOWERCASE|CharacterTypes.UPPERCASE|CharacterTypes.NUMBERS);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.PublishUserId = true;
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = Data.MAX_PLAYERS;
        PhotonNetwork.CreateRoom(roomCode, roomOptions, TypedLobby.Default);
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
