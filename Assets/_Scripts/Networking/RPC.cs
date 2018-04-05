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

    public void CallTest()
    {
        this.photonView.RPC("test", PhotonTargets.MasterClient, null);
    }

    [PunRPC]
    public void test()
    {
        Debug.Log("RPC TEST!!");
    }
}