using UnityEngine;

public class AI 
{
	public string[] GetAnswers()
	{
		string[] tmp = new string[2];
		if(Data.AI_DATA == null)
		{
			tmp[0] = tmp[1] = "undefined";
			return tmp;
		}

		string[] data = Data.AI_DATA.answers;
		int len = data.Length;
		tmp[0] = data[Random.Range(0, len)];
		tmp[1] = data[Random.Range(0, len)];
		return tmp;
	}
}
