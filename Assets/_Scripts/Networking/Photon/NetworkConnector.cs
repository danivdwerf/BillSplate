using UnityEngine;

public class NetworkConnector : MonoBehaviour 
{
	private void Awake() 
	{
		PhotonNetwork.autoJoinLobby = Data.AUTO_JOIN_LOBBY;
		PhotonNetwork.automaticallySyncScene = Data.AUTO_SYNC_SCENE;
	}

	private void Start()
	{
		UIController.singleton.ShowLoading(true);
		Utilities.JSON.singleton.loadJSON("http://freetimedev.com/gamedata.json", (string json)=>
        {
            RoundsData data = new RoundsData();
            Utilities.JSON.singleton.ToClass(json, ref data);
            Data.ROUNDS_DATA = data;
        });
		
		this.ConnectToServer();
	}

	private void ConnectToServer()
	{
		if (PhotonNetwork.connected)
		{
			UIController.singleton.GoToScreen(ScreenType.STARTSCREEN);
			UIController.singleton.ShowLoading(false);
			return;
		}
		PhotonNetwork.ConnectUsingSettings(Data.GAME_VERSION);
	}
}
