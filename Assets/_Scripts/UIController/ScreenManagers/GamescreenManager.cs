using UnityEngine;
using UnityEngine.UI;

using System.Collections.Generic;
public class GamescreenManager : UIManager 
{
    public static GamescreenManager singleton;

	[Header("Master version")]
    [SerializeField]private GameObject masterView;
    
    private byte currentRound;

    [Space(10)]
    [Header("Client version")]
    [SerializeField]private GameObject clientView;
    [SerializeField]private Text nameField;
    [SerializeField]private Text question;
    [SerializeField]private InputField answerField;
    [SerializeField]private Button submitButton;
    
    protected override void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this.gameObject);
        singleton = this;
        this.screenType = ScreenType.GAMESCREEN;
        base.Awake();
    }

    protected override void OnScreenEnabled()
    {
        if(this.nameField != null)
        {
            this.nameField.text = JoinscreenManager.singleton.Name;
            submitButton.onClick.AddListener(()=>this.OnSubmit());
        }
        if(Data.ROUNDS_DATA != null)
            this.UpdateRound(0);
    }

    public void UpdateRound(byte? roundNumber)
    {
        this.currentRound = (roundNumber == null) ? (byte)(this.currentRound+1) : (byte)roundNumber;

        if(Data.ROUNDS_DATA != null)
        {
            byte questionsNeeded = (byte)(PhotonNetwork.room.PlayerCount-1);
            List<string> questions = new List<string>();

            List<Prompt> allPrompts = Data.ROUNDS_DATA.rounds[this.currentRound].prompts;
            byte len = (byte)allPrompts.Count;
            
            for(byte i = 0; i < questionsNeeded; i++)
            {
                int randomIndex = Random.Range(0, len);
                Debug.Log("select number " + randomIndex + " from the " + len);
                questions.Add(allPrompts[randomIndex].prompt);
                allPrompts.RemoveAt(randomIndex);
                len--;
            }

            PhotonPlayer[] players = PhotonNetwork.playerList;
            for(byte i = 1; i < questionsNeeded+1; i++)
            {   
                Debug.Log(players[i].NickName);
                PhotonPlayer first = players[i];
                PhotonPlayer second = (i == questionsNeeded) ? players[0] : players[i+1];
                string[] currQuestions = {questions[0], questions[1]};
                questions.RemoveAt(0);
                RPC.singleton.SendQuestions(currQuestions, first);
                RPC.singleton.SendQuestions(currQuestions, second);
            }
        }
    }

    protected override void SetScreenForComputer()
    {
        this.masterView.SetActive(true);
        this.clientView.SetActive(false);
        
        this.clientView = null;
        this.nameField = null;
        this.question = null;
        this.answerField = null;
        this.submitButton = null;
    }

    protected override void SetScreenForMobile()
    {
        this.masterView.SetActive(false);
        this.clientView.SetActive(true);

        this.SetQuestion("Waiting for a pun", false);
    }

    public void SetQuestion(string question, bool removeInput)
    {
        this.question.text = question;
        this.answerField.gameObject.SetActive(!removeInput);
    }

    private void OnSubmit()
    {
        if(!this.ValidateAnswer())
            return;
    }

    private bool ValidateAnswer()
    {
        return true;
    }

    protected override void OnScreenDisabled()
    {

    }
}
