using UnityEngine;

using System.Collections.Generic;

public struct Question
{
    public int index{get;set;}
    public Prompt[] prompts{get;set;}
    public Answer[] answers{get;set;}
};

public struct Prompt
{
    public int id{get;set;}
    public string text{get;set;}
};

[System.Serializable]
public struct Answer
{
    public string text{get;set;}
    public int playerID{get;set;}
    public int votes{get;set;}
};

public class Host : Photon.PunBehaviour 
{
	public static Host singleton;

	private int currentRound;
    private Answer[] currentVotables;
    private Question[] currentPrompts;
    private Dictionary<int, int> scores;
    private Dictionary<int, string[]> currentAnswers;
    private int amountOfAnswers;
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

    public void Vote(int index)
    {
        this.currentVotables[index].votes++;
    }

    public void SetVotables(Answer answer1, Answer answer2)
    {
        this.currentVotables = new Answer[2];
        this.currentVotables[0] = answer1;
        this.currentVotables[1] = answer2;
    }

    public Answer GetMostVoted()
    {
        Answer answer1 = this.currentVotables[0];
        Answer answer2 = this.currentVotables[1];

        if(answer1.votes > answer2.votes)
            return answer1;
        else
            return answer2;
    }

    public void ReceiveAnswers(string[] answertext, int playerID)
    {
        int? index = null;
        int len = this.currentPrompts.Length;
        for(int i = 0; i < len; i++)
        {
            Question curr = this.currentPrompts[i];
            if(curr.answers[0].playerID != playerID)
                continue;
            
            index = i;
            break;
        }

        if(index == null)
            return;
        
        this.currentPrompts[(int)index].answers[0].text = answertext[0];
        this.currentPrompts[(int)index].answers[1].text = answertext[1];
        this.amountOfAnswers += 2;

        if(this.amountOfAnswers < this.currentPrompts.Length*2)
            return;

        GamescreenManager.singleton.StartVoting(this.currentPrompts);
    }

    public void UpdateRound(int? roundNumber)
    {
        if(Data.ROUNDS_DATA == null)
            return;

        this.currentRound = (roundNumber == null) ? this.currentRound+1 : (int)roundNumber;
        this.amountOfAnswers = 0;
        
        //Collect players
		PhotonPlayer[] players = PhotonNetwork.playerList;
        int amountOfPlayers = players.Length-1;
        int amountOfAI = this.ais.Count;

        //Collect prompts
        int promptsNeeded = amountOfPlayers + amountOfAI;
        string[] currentPrompts = this.GetRandomPrompts(promptsNeeded);
        Question[] currentQuestions = new Question[promptsNeeded];

        //Assign prompt ID's
		int index = 0;
        for(int i = 0; i < promptsNeeded; i++)
        {
            Question question = new Question();
            question.index = i;

            if(i < amountOfPlayers && players[i].IsMasterClient)
            {
                currentQuestions[i] = question;
                continue;
            }
            
            Prompt[] prompts = new Prompt[2];

            Prompt prompt1 = new Prompt();
            prompt1.id = index;
            prompt1.text = currentPrompts[index];
            prompts[0] = prompt1;

            Prompt prompt2 = new Prompt();
            prompt2.id = ((index==promptsNeeded-1) ? 0 : index+1);
            prompt2.text = currentPrompts[(index==promptsNeeded-1) ? 0 : index+1];
            prompts[1] = prompt2;

            question.prompts = prompts;

            Answer[] answers = new Answer[2];
            answers[0] = new Answer();
            answers[1] = new Answer();
            question.answers = answers;

            currentQuestions[i] = question;
			index++;
        }

        this.currentPrompts = currentQuestions;

        for(int i = 0; i < amountOfPlayers; i++)
        {
            if(players[i].IsMasterClient)
                continue;

            this.currentPrompts[i].answers[0].playerID = players[i].ID;
            this.currentPrompts[i].answers[1].playerID = players[i].ID;
            
            string[] prompts = new string[2];
            prompts[0] = currentQuestions[i].prompts[0].text;
            prompts[1] = currentQuestions[i].prompts[1].text;

            RPC.singleton.SendQuestions(prompts, players[i]);
        }

        for(int i = 0; i < amountOfAI; i++)
        {
            int realIndex = amountOfPlayers + i;
            int aiIndex = (-2 - i);
            this.currentPrompts[realIndex].answers[0].playerID = aiIndex;
            this.currentPrompts[realIndex].answers[1].playerID = aiIndex;

            string[] answers = this.ais[i].GetAnswers();
            this.ReceiveAnswers(answers, aiIndex);
        }
        GamescreenManager.singleton.StartWaitForAnswers();
    }

    private string[] GetRandomPrompts(int amountNeeded)
    {
        List<PromptData> promptsData = Data.ROUNDS_DATA.rounds[this.currentRound].prompts;
        string[] currentPrompts = new string[amountNeeded];
        int promptsLen = promptsData.Count;
		
        for(int i = 0; i < amountNeeded; i++)
        {
            int randomIndex = Random.Range(0, promptsLen);
            currentPrompts[i] = promptsData[randomIndex].prompt;
            promptsData.RemoveAt(randomIndex);
            promptsLen--;
        }
        return currentPrompts;
    }
}
