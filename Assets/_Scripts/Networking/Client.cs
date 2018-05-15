using UnityEngine;
public class Client : MonoBehaviour
{
    public static Client singleton;

    private string[] currentPrompts;
    private string[] myAnswers;
    private int promptIndex;
    
    private void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this);
        singleton = this;
    }

    private void OnEnable() 
    {
        GamescreenManager.OnSubmitAnswer += this.SetAnswer;
    }

    public void SetPrompts(string[] prompts)
    {
        this.myAnswers = new string[2];
        this.currentPrompts = prompts;
        this.promptIndex = 0;
        GamescreenManager.singleton.SetQuestion(prompts[0], false);
    }

    public void UpdatePrompt()
    {
        if(this.promptIndex == 0)
        {
            this.promptIndex++;
            GamescreenManager.singleton.SetQuestion(this.currentPrompts[this.promptIndex], false);
            return;
        }
        else
        {
            GamescreenManager.singleton.SetQuestion("Wait for the other players", true);
            RPC.singleton.SendAnswers(this.myAnswers, PhotonNetwork.player.ID);
        }
    }

    public void SetAnswer(string answer)
    {
        this.myAnswers[this.promptIndex] = answer;
        this.UpdatePrompt();
    }

    private void OnDisable() 
    {
        GamescreenManager.OnSubmitAnswer -= this.SetAnswer;    
    }
}