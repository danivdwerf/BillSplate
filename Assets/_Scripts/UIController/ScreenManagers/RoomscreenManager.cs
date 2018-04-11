using UnityEngine;
using UnityEngine.UI;

using System.Text.RegularExpressions;
public class RoomscreenManager : UIManager 
{
    public static RoomscreenManager singleton;

    [SerializeField]private InputField roomNameField;
    [SerializeField]private Button createRoomButton;
    [SerializeField]private Text feedback;
    
    protected override void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this);
        singleton = this;

        this.screenType = ScreenType.CREATEROOMSCREEN;
        base.Awake();
    }

    protected override void OnScreenEnabled()
    {
        this.feedback.text = string.Empty;
        this.roomNameField.Clear();
        this.createRoomButton.onClick.AddListener(()=>this.CreateRoom());
    }

    private void CreateRoom()
    {
        if(!this.ValidateValues())
            return;
        
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = Data.MAX_PLAYERS;

        string roomName = this.roomNameField.text;
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    private bool ValidateValues()
    {
        string roomName = this.roomNameField.text;
        if(roomName.Length != Data.ROOMNAME_SIZE)
        {
            this.SetFeedback("Roomname must be " + Data.ROOMNAME_SIZE +  " characters long");
            return false;
        }

        if(!roomName.Validate(new Regex(@"^[A-Za-z0-9]+$")))
        {
            this.SetFeedback("Roomname may only contain lowercase letters, capital letters and numbers");
            return false;
        }
        return true;
    }

    public void SetFeedback(string feedback)
    {
        this.feedback.text = feedback;
    }

    protected override void OnScreenDisabled()
    {
        this.createRoomButton.onClick.RemoveAllListeners();
    }
}
