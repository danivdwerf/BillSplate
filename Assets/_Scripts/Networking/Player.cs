using UnityEngine;
public class Player
{
    private string playerName = Data.DEFAULT_NAME;
    public string Name{get{return this.playerName;}}

    private int score = 0;
    public int Score{get{return this.score;}}

    public Player(string name)
    {
        this.playerName = name;
    }

    public void SetData(string name)
    {
        this.playerName = name;
    }
}