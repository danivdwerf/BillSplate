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

    public void SendQuestions(byte[] ids, string[] questions, PhotonPlayer target)
    {
        this.photonView.RPC("ReceiveQuestions", target, ids, questions);
    }

    public void SendAnswers(byte[] ids, string[] answers)
    {
        this.photonView.RPC("ReceiveAnswers", PhotonTargets.MasterClient, ids, answers);
    }

    public void CallTimeIsOver()
    {
        this.photonView.RPC("TimeIsOver", PhotonTargets.Others, null);
    }

    public void CallVote(string prompt, string answer1, string answer2)
    {
        this.photonView.RPC("ClientVote", PhotonTargets.Others, prompt, answer1, answer2);
    }

    [PunRPC]
    public void GoToGame()
    {
        UIController.singleton.GoToScreen(ScreenType.GAMESCREEN);
    }

    [PunRPC]
    public void ReceiveQuestions(byte[] ids, string[] questions)
    {
        Client.singleton.SetPrompts(ids, questions);
    }

    [PunRPC]
    public void ReceiveAnswers(byte[] ids, string[] answers)
    {
        Host.singleton.ReceiveAnswers(ids, answers);
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
}