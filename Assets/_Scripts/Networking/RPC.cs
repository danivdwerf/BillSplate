using UnityEngine;

public class RPC : Photon.PunBehaviour
{
    public static RPC singleton;
    
    private void Awake()
    {
        if(singleton != null && singleton != this)
            Destroy(this.gameObject);
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
}