using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;

public class LobbyscreenManager : UIManager 
{
    public static LobbyscreenManager singleton;

    [Header("Computer View")]
    [SerializeField]private GameObject masterView;
    [SerializeField]private GameObject iconPrefab;
    [SerializeField]private Transform wheel;
    [SerializeField]private Text roomCode;

    private List<GameObject> iconHolders;
    private Button playButton;

    [Space(10)]
    [Header("Mobile View")]
    [SerializeField]private GameObject clientView;

    protected override void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this);
        singleton = this;

        this.playButton = this.wheel.GetComponentInChildren<Button>();
        this.screenType = ScreenType.LOBBYSCREEN;
        base.Awake();
    }

    protected override void SetScreenForComputer()
    {
        this.clientView.SetActive(false);
        this.clientView = null;

        this.masterView.SetActive(true);
        this.wheel.gameObject.SetActive(true);
        this.roomCode.gameObject.SetActive(true);

        this.iconHolders = new List<GameObject>(Data.MAX_PLAYERS);
        this.CreateIconHolders();
    }

    protected override void SetScreenForMobile()
    {
        this.masterView.SetActive(false);
        this.masterView = null;

        this.wheel.gameObject.SetActive(false);
        this.wheel = null;

        this.roomCode.gameObject.SetActive(false);
        this.roomCode = null;
        
        this.iconPrefab = null;
        this.iconHolders = null;
        this.playButton = null;
    }

    protected override void OnScreenEnabled()
    {
        if(playButton != null)
        {
            this.playButton.gameObject.SetActive(false);
            this.playButton.onClick.AddListener(()=>this.OnStartGame());
        }

        UIController.singleton.ShowLoading(false);
    } 

    private void OnStartGame()
    {
        PhotonNetwork.room.IsOpen = false;
        RPC.singleton.CallGoToGame();
    }

    private void CreateIconHolders()
    {
        byte len = Data.MAX_PLAYERS;
        float maxItemSize = MathHelper.GetDegreesPerSegment(len);

        for(byte i = 0; i < len; i++)
        {
            GameObject iconHolder = GameObject.Instantiate(this.iconPrefab);
            iconHolder.transform.SetParent(wheel);

            GameObject icon = iconHolder.transform.GetChild(1).gameObject;
            icon.SetActive(false);

            Vector3 pos = MathHelper.PlaceOnCircle(wheel.position, 220, maxItemSize*i);
            iconHolder.transform.localPosition = pos;
            iconHolder.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
            this.iconHolders.Add(iconHolder);
        }
    }

    public void AddPlayer(string name)
    {
        if(this.iconHolders.Count <= 0)
            return;
        
        GameObject holder = this.iconHolders[0];    
        this.iconHolders.RemoveAt(0);

        GameObject textObject = holder.transform.GetChild(0).gameObject;
        Text text = textObject.GetComponent<Text>();
        text.text = name;
        text.color = Color.white;
        textObject.GetComponent<Transitions.MoveTo>().Move(null);

        GameObject iconObject = holder.transform.GetChild(1).gameObject;
        iconObject.GetComponent<Image>().sprite = (Sprite)Data.PLAYER_ICONS[PhotonNetwork.room.PlayerCount-1];
        iconObject.SetActive(true);
    }

    public void ShowStartbutton(bool value)
    {
        if(this.playButton == null)
            return;
        this.playButton.gameObject.SetActive(value);
    }

    public void SetRoomcode(string code)
    {
        if(this.roomCode == null)
            return;
        this.roomCode.text = "Roomcode: " + code;
    }

    protected override void OnScreenDisabled()
    {
        if(playButton != null)
            this.playButton.onClick.RemoveAllListeners();
    }
}
