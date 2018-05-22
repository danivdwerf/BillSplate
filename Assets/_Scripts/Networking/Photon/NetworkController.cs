using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class NetworkController : Photon.PunBehaviour
{
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnMasterClientSwitched(PhotonPlayer newMaster)
	{
		PhotonNetwork.LeaveRoom();
        Destroy(this.GetComponent<Client>());
		JoinscreenManager.singleton.SetFeedback("The room you were in was closed.");
	}

    public override void OnLeftRoom()
    {
        if(Host.singleton != null)
        {
            UIController.singleton.GoToScreen(ScreenType.STARTSCREEN);
            Host.Destroy(Host.singleton);
            return;
        }
        UIController.singleton.GoToScreen(ScreenType.JOINSCREEN);
    }

    public override void OnJoinedLobby()
    {
        if(SplashScreenManager.singleton.Enabled)
            SplashScreenManager.OnSplashDone = ()=> UIController.singleton.GoToScreen(ScreenType.STARTSCREEN);
        else
        {
            UIController.singleton.GoToScreen(ScreenType.STARTSCREEN);
            UIController.singleton.ShowLoading(false);
        }
    }

    public override void OnCreatedRoom()
    {
        this.gameObject.AddComponent<Host>();
        Utilities.JSON.singleton.loadJSON("http://freetimedev.com/gamedata.json", (string json)=>
        {
            RoundsData data = new RoundsData();
            AIanswers aidata = new AIanswers();
            Utilities.JSON.singleton.ToClass(json, ref data);
            Utilities.JSON.singleton.ToClass(json, ref aidata);
            Data.ROUNDS_DATA = data;
            Data.AI_DATA = aidata;
        });
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        if(codeAndMessage != null)
        {
		    short errorCode = (short)codeAndMessage[0];
		    if(errorCode == 32766)
                StartscreenManager.singleton.CreateGame();
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        if(!PhotonNetwork.isMasterClient)
            return;

        LobbyscreenManager.singleton.AddPlayer(player.NickName);
        Host.singleton.AddPlayerToScore(player.ID);
    }

    public override void OnJoinedRoom()
    {
        UIController.singleton.GoToScreen(ScreenType.LOBBYSCREEN);
        if(PhotonNetwork.isMasterClient)
        {
            LobbyscreenManager.singleton.SetRoomcode(PhotonNetwork.room.Name);
            return;
        }
        this.gameObject.AddComponent<Client>();
    }

    public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
    {
        string error = "Failed to join room.";
        if(codeAndMsg != null)
        {
            short code = (short)codeAndMsg[0];
            if(code == 32758)
                error = "This room does not exist.";
        }
        JoinscreenManager.singleton.SetFeedback(error);
    }
}
