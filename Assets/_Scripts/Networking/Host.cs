using UnityEngine;

using System.Collections.Generic;

public class Host : Photon.PunBehaviour 
{
	public static Host singleton;
	private byte currentRound;

    private Dictionary<byte, int> scores;

	private void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this.gameObject);
		singleton = this;
	}

    private void Start() 
    {
        this.scores = new Dictionary<byte, int>();    
    }

    public void AddPlayerToScore(byte id)
    {
        this.scores.Add(id, 0);
    }

    public void UpdateRound(byte? roundNumber)
    {
        if(Data.ROUNDS_DATA == null)
            return;

        this.currentRound = (roundNumber == null) ? (byte)(this.currentRound+1) : (byte)roundNumber;

		PhotonPlayer[] players = PhotonNetwork.playerList;
        byte amountOfPlayers = (byte)players.Length;

		List<Prompt> promptsData = Data.ROUNDS_DATA.rounds[this.currentRound].prompts;
        byte promptsLen = (byte)promptsData.Count;

		byte promptsNeeded = (byte)(amountOfPlayers-1);
		List<string> currentPrompts = new List<string>();
        for(byte i = 0; i < promptsNeeded; i++)
        {
            int randomIndex = Random.Range(0, promptsLen);
            currentPrompts.Add(promptsData[randomIndex].prompt);
            promptsData.RemoveAt(randomIndex);
            promptsLen--;
        }

		int index = 0;
        List<string[]> prompts = new List<string[]>();
        for(byte i = 0; i < amountOfPlayers; i++)
        {   
            if(players[i].IsMasterClient)
            {
                prompts.Add(new string[2]);
                continue;
            }

            string[] tmp = new string[2];
			tmp[0] = currentPrompts[index];
			tmp[1] = currentPrompts[(index==promptsNeeded-1) ? 0 : index+1];
            prompts.Add(tmp);
			index++;
        }

        for(byte i = 0; i < amountOfPlayers; i++)
        {
            if(players[i].IsMasterClient)
                continue;

            RPC.singleton.SendQuestions(prompts[i], players[i]);
        }
    }
}
