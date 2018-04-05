using UnityEngine;
public class NetworkPlayer : Photon.PunBehaviour
{
    public static NetworkPlayer singleton;
    public PhotonView photonView;
    private string playerName = Data.DEFAULT_NAME;
    public string Name{get{return this.playerName;} 
    set
    {
        this.playerName = value;
        Object[] icons = Data.PLAYER_ICONS;
        this.playerIcon = (Sprite)icons[PhotonNetwork.room.PlayerCount];
    }}

    private Sprite playerIcon = null;
    public Sprite Icon{get{return this.playerIcon;}}

    private void Awake ()
    {
        if (singleton != null && singleton != this)
            Destroy (this.gameObject);
        singleton = this;
    }
}