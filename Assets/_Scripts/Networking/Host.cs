using UnityEngine;

using System.Collections.Generic;

public class Host : Photon.PunBehaviour 
{
	public static Host singleton;
	
	[SerializeField]private List<Player> players;

	private void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this.gameObject);
		singleton = this;

		this.players = new List<Player>();
	}

	public void AddPlayer(Player player)
	{
		this.players.Add(player);
		// if(this.players.Count >= 3)
			LobbyscreenManager.singleton.ShowStartbutton(true);
	}
}
