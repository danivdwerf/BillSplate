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
		JoinscreenManager.singleton.SetFeedback("The room you were in was closed.");
	}

    public override void OnLeftRoom()
    {
        UIController.singleton.GoToScreen(ScreenType.JOINSCREEN);
    }

    public override void OnJoinedLobby()
    {
        UIController.singleton.GoToScreen(ScreenType.STARTSCREEN);
        UIController.singleton.ShowLoading(false);
    }

    public override void OnCreatedRoom()
    {
        this.gameObject.AddComponent<Host>();
        Utilities.JSON.singleton.loadJSON("http://freetimedev.com/gamedata.json", (string json)=>
        {
            RoundsData data = new RoundsData();
            Utilities.JSON.singleton.ToClass(json, ref data);
            Data.ROUNDS_DATA = data;
        });
    }

    public override void OnPhotonCreateRoomFailed(object[] codeAndMessage)
    {
        UIController.singleton.GoToScreen(ScreenType.CREATEROOMSCREEN);
        string error = "Failed to create room.";
        if(codeAndMessage != null)
        {
		    short errorCode = (short)codeAndMessage[0];

		    if(errorCode == 32766)
			    error = "This roomname is already taken.";
        }
		RoomscreenManager.singleton.SetFeedback(error);
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer player)
    {
        LobbyscreenManager.singleton.AddPlayer(player.NickName);
        if(PhotonNetwork.room.PlayerCount > 3)
            LobbyscreenManager.singleton.ShowStartbutton(true);
    }

    public override void OnJoinedRoom()
    {
        UIController.singleton.GoToScreen(ScreenType.LOBBYSCREEN);
        if(PhotonNetwork.isMasterClient)
        {
            LobbyscreenManager.singleton.SetRoomcode(PhotonNetwork.room.Name);
            return;
        }
        PhotonNetwork.player.NickName = JoinscreenManager.singleton.Name;
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
