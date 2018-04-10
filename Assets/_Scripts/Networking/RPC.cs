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

    public void SendQuestions(string[] questions, PhotonPlayer target)
    {
        this.photonView.RPC("ReciveQuestions", target, questions);
    }

    [PunRPC]
    public void GoToGame()
    {
        UIController.singleton.GoToScreen(ScreenType.GAMESCREEN);
    }

    [PunRPC]
    public void ReciveQuestions(string[] questions)
    {
        Client.singleton.SetPrompts(questions);
    }
}