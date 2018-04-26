using UnityEngine;

public class NetworkConnector : MonoBehaviour 
{
	private void Awake() 
	{
		PhotonNetwork.autoJoinLobby = Data.AUTO_JOIN_LOBBY;
		PhotonNetwork.automaticallySyncScene = Data.AUTO_SYNC_SCENE;
		UIController.singleton.GoToScreen(ScreenType.SPLASHSCREEN);
	}

	private void Start()
	{
		this.ConnectToServer();
	}

	private void ConnectToServer()
	{
		if (PhotonNetwork.connected)
		{
			SplashScreenManager.OnSplashDone = ()=>UIController.singleton.GoToScreen(ScreenType.STARTSCREEN);
			return;
		}
		PhotonNetwork.ConnectUsingSettings(Data.GAME_VERSION);
	}
}
