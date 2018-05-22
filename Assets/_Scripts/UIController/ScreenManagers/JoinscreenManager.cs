using UnityEngine;
using UnityEngine.UI;

using System.Text.RegularExpressions;

public class JoinscreenManager : UIManager 
{
	public static JoinscreenManager singleton;
    
    [SerializeField]private InputField usernameField;
    [SerializeField]private InputField roomcodeField;
    [SerializeField]private Button joinButton;
    [SerializeField]private Text feedback;

    public string Name{get{return this.usernameField.text;}}

    protected override void Awake()
    {
        if (singleton != null && singleton != this)
            Destroy (this);
        singleton = this;
        
        this.screenType = ScreenType.JOINSCREEN;
    }

    protected override void OnScreenEnabled()
    {
        joinButton.onClick.AddListener(()=>this.OnJoinRoom());
        feedback.text = string.Empty;
    }

    private void OnJoinRoom()
    {
        if(!this.ValidateValues())
            return;

        PhotonNetwork.playerName = usernameField.text;
        PhotonNetwork.JoinRoom(roomcodeField.text);
    }

    private bool ValidateValues()
    {
        string username = usernameField.text;
        string roomcode = roomcodeField.text;

        if(string.IsNullOrEmpty(username))
        {
            this.SetFeedback("Your username cannot be empty.");
            return false;
        }

        if(username.Length < 3 || username.Length > 32)
        {
            this.SetFeedback("Your username must be at least 3 charaters and max 32 characters long.");
            return false;
        }

        if(username.IsAllWhiteSpace())
        {
            this.SetFeedback("Your username cannot exist of spaces only.");
            return false;
        }

        if(!username.Validate(new Regex(@"^[A-Za-z0-9 ]+$")))
        {
            this.SetFeedback("Your username may only contain lowercase letters, uppercase letters and numbers.");
            return false;
        }

        if(string.IsNullOrEmpty(roomcode))
        {
            this.SetFeedback("The room cannot be empty.");
            return false;
        }

        if(roomcode.Length != Data.ROOMNAME_SIZE)
        {
            this.SetFeedback("Roomcodes are always " + Data.ROOMNAME_SIZE + " characters long.");
            return false;
        }

        if(!roomcode.Validate(new Regex(@"^[A-Za-z0-9]+$")))
        {
            this.SetFeedback("Roomcodes only contain lowercase letters, uppercase letters and numbers.");
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
        joinButton.onClick.RemoveAllListeners();
    }
}
