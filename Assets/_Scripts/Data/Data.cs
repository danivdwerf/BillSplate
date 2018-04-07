using UnityEngine;

public static class Data
{
	public const string GAME_VERSION = "0.0.0";
	public const bool AUTO_JOIN_LOBBY = false;
	public const bool AUTO_SYNC_SCENE = true;

	public const byte MAX_PLAYERS = 7;
	public const byte MIN_PLAYERS = 2;

	public const byte ROOMNAME_SIZE = 5;
	public const string DEFAULT_NAME = "Randy";

	public static Object[] PLAYER_ICONS = Resources.LoadAll("Playericons", typeof(Sprite));

	public static RoundsData ROUNDS_DATA;
}
