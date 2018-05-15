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
    public List<PromptData> prompts = new List<PromptData>();
};

[System.Serializable]
public class PromptData
{
    public int id;
    public string prompt;
};

[System.Serializable]
public class AIanswers
{
    public string[] answers;
};