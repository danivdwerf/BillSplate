using UnityEngine;

using System.Collections.Generic;

public class Host : Photon.PunBehaviour 
{
	public static Host singleton;

	private byte currentRound;
    private string[] currentPrompts;
    private Dictionary<int, int> scores;
    private Dictionary<byte, string[]> currentAnswers;
    private byte amountOfAnswers;

	private void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this);
		singleton = this;
	}

    private void Start() 
    {
        this.scores = new Dictionary<int, int>();
    }

    public void AddPlayerToScore(int id)
    {
        this.scores.Add(id, 0);
    }

    public void ReceiveAnswers(byte[] questionIDs, string[] answers)
    {
        byte key1 = questionIDs[0];
        string value1 = answers[0];

        if(!currentAnswers.ContainsKey(key1))
        {
            string[] tmp = new string[2];
            tmp[0] = value1;
            this.currentAnswers.Add(key1, tmp);
        }
        else this.currentAnswers[key1][1] = value1;
        this.amountOfAnswers++;

        byte key2 = questionIDs[1];
        string value2 = answers[1];
        if(!currentAnswers.ContainsKey(key2))
        {
            string[] tmp = new string[2];
            tmp[0] = value2;
            this.currentAnswers.Add(key2, tmp);
        }
        else this.currentAnswers[key2][1] = value2;
        this.amountOfAnswers++;

        if(this.amountOfAnswers < this.currentPrompts.Length*2)
            return;

        Dictionary<string, string[]> promptAndAnswer = new Dictionary<string, string[]>();
        byte len = (byte)this.currentPrompts.Length;
        for(byte i = 0; i < len; i++)
            promptAndAnswer.Add(this.currentPrompts[i], currentAnswers[i]);

        GamescreenManager.singleton.StartVoting(promptAndAnswer);
    }

    public void UpdateRound(byte? roundNumber)
    {
        if(Data.ROUNDS_DATA == null)
            return;

        this.currentRound = (roundNumber == null) ? (byte)(this.currentRound+1) : (byte)roundNumber;
        this.amountOfAnswers = 0;

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
        List<byte[]> questionIDs = new List<byte[]>();
        List<string[]> prompts = new List<string[]>();
        for(byte i = 0; i < amountOfPlayers; i++)
        {   
            if(players[i].IsMasterClient)
            {
                prompts.Add(new string[2]);
                questionIDs.Add(new byte[2]);
                continue;
            }

            string[] tmp = new string[2];
			tmp[0] = currentPrompts[index];
			tmp[1] = currentPrompts[(index==promptsNeeded-1) ? 0 : index+1];
            prompts.Add(tmp);

            byte[] ids = new byte[2];
            ids[0] = (byte)index;
            ids[1] = (byte)((index==promptsNeeded-1) ? 0 : index+1);
            questionIDs.Add(ids);
			index++;
        }

        this.currentPrompts = currentPrompts.ToArray();
        this.currentAnswers = new Dictionary<byte, string[]>(); 

        for(byte i = 0; i < amountOfPlayers; i++)
        {
            if(players[i].IsMasterClient)
                continue;

            RPC.singleton.SendQuestions(questionIDs[i], prompts[i], players[i]);
        }
    }
}
