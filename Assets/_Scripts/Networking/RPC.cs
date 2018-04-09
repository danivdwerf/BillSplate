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

    public void CallAddPlayer(string name)
    {
        this.photonView.RPC("AddPlayer", PhotonTargets.MasterClient, name);
    }

    public void CallGoToGame()
    {
        this.photonView.RPC("GoToGame", PhotonTargets.All, null);
    }

    public void SendQuestions(string[] quesitons, PhotonPlayer target)
    {
        this.photonView.RPC("ReciveQuestions", target, quesitons);
    }

    [PunRPC]
    public void AddPlayer(string name)
    {
        Host.singleton.AddPlayer(new Player(name));
        LobbyscreenManager.singleton.AddPlayer(name);
    }

    [PunRPC]
    public void GoToGame()
    {
        UIController.singleton.GoToScreen(ScreenType.GAMESCREEN);
    }

    [PunRPC]
    public void ReciveQuestions(string[] quesitons)
    {
        GamescreenManager.singleton.SetQuestion(quesitons[0], false);
    }
}