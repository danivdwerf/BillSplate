using UnityEngine;

public class RPC : Photon.PunBehaviour
{
    public static RPC singleton;
    
    private void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this);
        singleton = this;
    }

    public void CallGoToGame()
    {
        this.photonView.RPC("GoToGame", PhotonTargets.All, null);
    }

    public void SendQuestions(string[] prompts, PhotonPlayer target)
    {
        this.photonView.RPC("ReceiveQuestions", target, prompts);
    }

    public void SendAnswers(string[] answers, int playerID)
    {
        this.photonView.RPC("ReceiveAnswers", PhotonTargets.MasterClient, answers, playerID);
    }

    public void CallTimeIsOver()
    {
        this.photonView.RPC("TimeIsOver", PhotonTargets.Others, null);
    }

    public void CallVote(string prompt, string answer1, string answer2)
    {
        this.photonView.RPC("ClientVote", PhotonTargets.Others, prompt, answer1, answer2);
    }

    public void SendVote(int index)
    {
        this.photonView.RPC("Vote", PhotonTargets.MasterClient, index);
    }

    [PunRPC]
    public void GoToGame()
    {
        UIController.singleton.GoToScreen(ScreenType.GAMESCREEN);
    }

    [PunRPC]
    public void ReceiveQuestions(string[] prompts)
    {
        Client.singleton.SetPrompts(prompts);
    }

    [PunRPC]
    public void ReceiveAnswers(string[] answers, int playerID)
    {
        Host.singleton.ReceiveAnswers(answers, playerID);
    }

    [PunRPC]
    public void TimeIsOver()
    {

    }

    [PunRPC]
    public void ClientVote(string prompt, string answer1, string answer2)
    {
        GamescreenManager.singleton.ShowClientVote(prompt, answer1, answer2);
    }

    [PunRPC]
    public void Vote(int index)
    {
        Host.singleton.Vote(index);
    }
}