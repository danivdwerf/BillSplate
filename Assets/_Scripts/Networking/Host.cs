using UnityEngine;

using System.Collections.Generic;

public class Host : Photon.PunBehaviour 
{
	public static Host singleton;

	private byte currentRound;
    private string[] currentPrompts;
    private Dictionary<int, int> scores;
    private Dictionary<int, string[]> currentAnswers;
    private byte amountOfAnswers;
    private List<AI> ais;

	private void Awake()
	{
		if(singleton != null && singleton != this)
			Destroy(this);
		singleton = this;
	}

    private void Start() 
    {
        this.scores = new Dictionary<int, int>();
        this.ais = new List<AI>();
    }

    public void AddPlayerToScore(int id)
    {
        this.scores.Add(id, 0);
    }

    public void CreateAI()
    {
        this.ais.Add(new AI());
        LobbyscreenManager.singleton.AddPlayer("Computer");
    }

    public void ReceiveAnswers(int[] questionIDs, string[] answers)
    {
        int key1 = questionIDs[0];
        string value1 = answers[0];
        if(!currentAnswers.ContainsKey(key1))
        {
            string[] tmp = new string[2];
            tmp[0] = value1;
            this.currentAnswers.Add(key1, tmp);
        }
        else this.currentAnswers[key1][1] = value1;
        this.amountOfAnswers++;

        int key2 = questionIDs[1];
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
        int len = this.currentPrompts.Length;
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
        int amountOfPlayers = players.Length-1;
        int amountOfAI = this.ais.Count;

		List<Prompt> promptsData = Data.ROUNDS_DATA.rounds[this.currentRound].prompts;
        int promptsLen = promptsData.Count;

        List<string> currentPrompts = new List<string>();
		int promptsNeeded = amountOfPlayers + amountOfAI;
		
        for(byte i = 0; i < promptsNeeded; i++)
        {
            int randomIndex = Random.Range(0, promptsLen);
            currentPrompts.Add(promptsData[randomIndex].prompt);
            promptsData.RemoveAt(randomIndex);
            promptsLen--;
        }

		int index = 0;
        List<int[]> questionIDs = new List<int[]>();
        List<string[]> prompts = new List<string[]>();
        for(byte i = 0; i < promptsNeeded; i++)
        {   
            if(i<amountOfPlayers && players[i].IsMasterClient)
            {
                prompts.Add(new string[2]);
                questionIDs.Add(new int[2]);
                continue;
            }

            string[] tmp = new string[2];
			tmp[0] = currentPrompts[index];
			tmp[1] = currentPrompts[(index==promptsNeeded-1) ? 0 : index+1];
            prompts.Add(tmp);

            int[] ids = new int[2];
            ids[0] = index;
            ids[1] = ((index==promptsNeeded-1) ? 0 : index+1);
            questionIDs.Add(ids);
			index++;
        }

        this.currentPrompts = currentPrompts.ToArray();
        this.currentAnswers = new Dictionary<int, string[]>(); 

        for(byte i = 0; i < amountOfPlayers; i++)
        {
            if(players[i].IsMasterClient)
                continue;
            RPC.singleton.SendQuestions(questionIDs[i], prompts[i], players[i]);
        }

        for(byte i = 0; i < amountOfAI; i++)
        {
            byte realIndex = (byte)((amountOfPlayers)+i);
            string[] answers = this.ais[i].GetAnswers();
            this.ReceiveAnswers(questionIDs[realIndex], answers);
        }
        GamescreenManager.singleton.StartWaitForAnswers();
    }
}
