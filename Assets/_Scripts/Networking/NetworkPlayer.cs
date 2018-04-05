using UnityEngine;
public class NetworkPlayer : Photon.PunBehaviour
{
    public static NetworkPlayer singleton;

    private string playerName = Data.DEFAULT_NAME;
    public string Name{get{return this.playerName;} set{this.playerName = value;}}

    private void Awake ()
    {
        if (singleton != null && singleton != this)
            Destroy (this.gameObject);
        singleton = this;
    }
}