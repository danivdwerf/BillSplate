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

    [PunRPC]
    public void AddPlayer(string name)
    {
        LobbyscreenManager.singleton.AddPlayer(name);
    }
}