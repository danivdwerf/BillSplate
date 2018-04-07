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
}