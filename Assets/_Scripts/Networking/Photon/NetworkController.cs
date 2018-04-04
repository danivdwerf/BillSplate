using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;

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

    public override void OnPhotonCreateRoomFailed(object[] codeAndMsg)
    {
        UIController.singleton.GoToScreen(ScreenType.CREATEROOMSCREEN);
        RoomscreenManager.singleton.SetFeedback("Failed to create room.");
    }

    public override void OnJoinedRoom()
    {
        UIController.singleton.GoToScreen(ScreenType.LOBBYSCREEN);
    }
}
