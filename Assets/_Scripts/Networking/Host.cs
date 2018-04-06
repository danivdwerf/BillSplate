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
	}

	private void AddPlayer(Player player)
	{
		this.players.Add(player);
	}
}
