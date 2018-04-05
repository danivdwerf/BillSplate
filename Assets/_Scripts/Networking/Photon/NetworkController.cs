using UnityEngine;

using System.Collections;
using System.Collections.Generic;

public class NetworkController : Photon.PunBehaviour
{
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby(TypedLobby.Default);
    }

    public override void OnJoinedLobby()
    {
        UIController.singleton.GoToScreen(ScreenType.STARTSCREEN);
        UIController.singleton.ShowLoading(false);
    }

    public override void OnCreatedRoom()
    {

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

    public override void OnJoinedRoom()
    {
        LobbyscreenManager.singleton.SetRoomcode(PhotonNetwork.room.Name);
        UIController.singleton.GoToScreen(ScreenType.LOBBYSCREEN);
        if(PhotonNetwork.isMasterClient)
            return;

        NetworkPlayer.singleton.Name = JoinscreenManager.singleton.Name;
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
