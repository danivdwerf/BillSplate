using UnityEngine;
using UnityEngine.UI;

public class GamescreenManager : UIManager 
{
    public static GamescreenManager singleton;

	[Header("Master version")]
    [SerializeField]private GameObject masterView;
    [SerializeField]private Text feedbackMaster;
    [SerializeField]private Text promptMaster;
    [SerializeField]private Text[] answersMaster;
    [SerializeField]private Text timer;

    [Space(10)]
    [Header("Client version")]
    [SerializeField]private GameObject clientView;
    [SerializeField]private Text clientName;
    [SerializeField]private Text promptClient;
    [SerializeField]private InputField answerfieldClient;
    [SerializeField]private Button submitClient;

    [SerializeField]private Button[] answersClient;
    [SerializeField]private Text[] answersClientLabel;
    public static System.Action<string> OnSubmitAnswer;
    
    protected override void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this);
        singleton = this;
        
        this.screenType = ScreenType.GAMESCREEN;
        base.Awake();
    }

    protected override void SetScreenForComputer()
    {
        this.clientView.SetActive(false);
        this.clientView = null;

        this.clientName.gameObject.SetActive(false);
        this.clientName = null;

        this.promptClient.gameObject.SetActive(false);
        this.promptClient = null;

        this.answerfieldClient.gameObject.SetActive(false);
        this.answerfieldClient = null;

        this.submitClient.gameObject.SetActive(false);
        this.submitClient = null;

        this.masterView.SetActive(true);

        this.feedbackMaster.gameObject.SetActive(true);
        this.feedbackMaster.text = "Answer the questions on your phone";

        this.promptMaster.gameObject.SetActive(true);
        this.promptMaster.text = string.Empty;

        this.timer.gameObject.SetActive(false);
        this.timer.text = string.Empty;

        this.answersMaster[0].gameObject.SetActive(false);
        this.answersMaster[1].gameObject.SetActive(false);
    }

    protected override void SetScreenForMobile()
    {
        this.masterView.SetActive(false);
        this.masterView = null;

        this.feedbackMaster.text = string.Empty;
        this.feedbackMaster.gameObject.SetActive(false);
        this.feedbackMaster = null;

        this.promptMaster.text = string.Empty;
        this.promptMaster.gameObject.SetActive(false);
        this.promptMaster = null;

        this.timer.text = string.Empty;
        this.timer.gameObject.SetActive(false);
        this.timer = null;

        this.answersMaster = null;

        this.clientView.SetActive(true);
        
        this.clientName.gameObject.SetActive(true);

        this.promptClient.gameObject.SetActive(true);

        this.answerfieldClient.Clear();
        this.answerfieldClient.gameObject.SetActive(true);
        
        this.submitClient.gameObject.SetActive(true);


        this.SetQuestion("Waiting for a prompt", false);
    }

    protected override void OnScreenEnabled()
    {
        if(this.clientView != null)
        {
            this.clientName.text = JoinscreenManager.singleton.Name;
            submitClient.onClick.AddListener(()=> this.OnSubmit());
        }

        if(masterView!= null)
        {
            Host.singleton.UpdateRound(0);
        }
    }

    public void SetQuestion(string promptClient, bool removeInput)
    {
        this.promptClient.text = promptClient;
        this.answerfieldClient.gameObject.SetActive(!removeInput);
        this.submitClient.gameObject.SetActive(!removeInput);
    }

    public void StartWaitForAnswers()
    {
        StartCoroutine("WaitForAnswers");
    }

    private System.Collections.IEnumerator WaitForAnswers()
    {
        float waitTime = 60.0f;
        float timer = waitTime;
        this.timer.gameObject.SetActive(true);
        this.feedbackMaster.text = "Answer the prompts on you phone.";
        while(timer > 0.0f)
        {
            this.timer.text = Mathf.Round(timer).ToString("00");
            yield return new WaitForSeconds(1.0f);
            timer--;
        }
        this.timer.gameObject.SetActive(false);
    }

    public void StartVoting(System.Collections.Generic.Dictionary<string, string[]> data)
    {
        StopCoroutine("WaitForAnswers");
        this.feedbackMaster.text = "Vote for your favourite answer on your phone:";
        this.answersMaster[0].gameObject.SetActive(true);
        this.answersMaster[1].gameObject.SetActive(true);
        StartCoroutine(Vote(data, null));
    }

    private System.Collections.IEnumerator Vote(System.Collections.Generic.Dictionary<string, string[]> data, System.Action callback)
    {
        float waitTime = 20.0f;
        float timer = waitTime;

        byte len = (byte)data.Keys.Count;
        string[] keys = new string[len];
        data.Keys.CopyTo(keys, 0);

        this.timer.gameObject.SetActive(true);

        for(byte i = 0; i < len; i++)
        {
            string currentPrompt = keys[i];
            string answer1 = data[currentPrompt][0];
            string answer2 = data[currentPrompt][1];
        
            this.promptMaster.text = currentPrompt;
            this.answersMaster[0].text = answer1;
            this.answersMaster[1].text = answer2;

            RPC.singleton.CallVote(currentPrompt, answer1, answer2);

            timer = waitTime;
            while(timer > 0.0f)
            {
                this.timer.text = Mathf.Round(timer).ToString("00");
                yield return new WaitForSeconds(1.0f);
                timer--;
            }

            yield return StartCoroutine(ShowScores());
        }
        
        this.timer.gameObject.SetActive(false);
        this.feedbackMaster.text = string.Empty;

        if(callback != null)
            callback();
        yield return null;
    }

    public void ShowClientVote(string prompt, string answer1, string answer2)
    {
        this.promptClient.text = prompt;
        this.answersClientLabel[0].text = answer1;
        this.answersClientLabel[1].text = answer2;
    }

    private System.Collections.IEnumerator ShowScores()
    {
        Debug.Log("Set the scores and animate them in.");
        yield return null;
    }

    private void OnSubmit()
    {
        string answer = this.answerfieldClient.text;
        if(!this.ValidateAnswer(answer))
            return;

        this.answerfieldClient.Clear();

        if(OnSubmitAnswer != null)
            OnSubmitAnswer(answer);
    }

    private bool ValidateAnswer(string text)
    {
        return true;
    }

    protected override void OnScreenDisabled()
    {
        if(this.submitClient != null)
            submitClient.onClick.RemoveAllListeners();
    }
}
