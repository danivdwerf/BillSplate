using UnityEngine;
public class Client : MonoBehaviour
{
    public static Client singleton;

    private string[] currentPrompts;
    private string[] answers;
    private byte promptIndex;
    
    private void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this.gameObject);
        singleton = this;
    }

    private void OnEnable() 
    {
        GamescreenManager.OnSubmitAnswer += this.SetAnswer;
    }

    public void SetPrompts(string[] prompts)
    {
        this.currentPrompts = new string[2];
        this.answers = new string[2];

        this.currentPrompts[0] = prompts[0];
        this.currentPrompts[1] = prompts[1];

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
        }
    }

    public void SetAnswer(string answer)
    {
        this.answers[this.promptIndex] = answer;
        this.UpdatePrompt();
    }

    private void OnDisable() 
    {
        GamescreenManager.OnSubmitAnswer -= this.SetAnswer;    
    }
}