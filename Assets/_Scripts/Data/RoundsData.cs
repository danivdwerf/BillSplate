using System.Collections.Generic;

[System.Serializable]
public class RoundsData
{
    public List<Round> rounds = new List<Round>();
};

[System.Serializable]
public class Round
{
    public int id;
    public string title;
    public int maxScore;
    public List<Prompt> prompts = new List<Prompt>();
};

[System.Serializable]
public class Prompt
{
    public int id;
    public string prompt;
    public string answerPrefix;
};

[System.Serializable]
public class AIanswers
{
    public string[] answers;
};