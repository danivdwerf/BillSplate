using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

public class GamescreenManager : UIManager 
{
    public static GamescreenManager singleton;

	[Header("Master version")]
    [SerializeField]private GameObject masterView;
    [SerializeField]private Text questionText;
    [SerializeField]private Text[] answers;

    [Space(10)]
    [Header("Client version")]
    public static Action<string> OnSubmitAnswer;
    
    [SerializeField]private GameObject clientView;
    [SerializeField]private Text nameField;
    [SerializeField]private Text question;
    [SerializeField]private InputField answerField;
    [SerializeField]private Button submitButton;
    
    protected override void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this);
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
            Host.singleton.UpdateRound(0);
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
        this.submitButton.gameObject.SetActive(!removeInput);
    }

    public void StartVoting(Dictionary<string, string[]> data)
    {
        int len = data.Keys.Count;
        string[] keys = new string[len];
        data.Keys.CopyTo(keys, 0);

        questionText.text = keys[0];
        answers[0].text = data[keys[0]][0];
        answers[1].text = data[keys[0]][1];
    }

    private void OnSubmit()
    {
        string answer = this.answerField.text;
        if(!this.ValidateAnswer(answer))
            return;

        if(OnSubmitAnswer != null)
            OnSubmitAnswer(answer);
    }

    private bool ValidateAnswer(string text)
    {
        return true;
    }

    protected override void OnScreenDisabled()
    {

    }
}
