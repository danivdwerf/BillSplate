using UnityEngine;
public class Player : Photon.PunBehaviour
{
    private string playerName = Data.DEFAULT_NAME;
    public string Name{get{return this.playerName;}}

    private int score = 0;
    public int Score{get{return this.score;}}

    private Sprite playerIcon;
    public Sprite Icon{get{return this.playerIcon;}}

    private void Start()
    {
        this.playerIcon = (Sprite)Data.PLAYER_ICONS[0];
    }

    public void SetData(string name)
    {
        this.playerName = name;
        PhotonNetwork.playerName = name;

        int index = PhotonNetwork.room.PlayerCount;
        if(index > Data.MAX_PLAYERS)
            index = 0;
        this.playerIcon = (Sprite)Data.PLAYER_ICONS[index];

       RPC.singleton.CallTest();
    }
}