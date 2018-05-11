using System.Text.RegularExpressions;

public enum CharacterTypes
{
	LOWERCASE = (1<<0),
	UPPERCASE = (1<<1),
	NUMBERS = (1<<2),
	SPECIALS = (1<<3)
};

public static class StringExtensions  
{
	public static bool Validate(this string original, Regex pattern)
	{
		return pattern.IsMatch(original);
	}

	public static bool IsAllWhiteSpace(this string original)
	{
		int whiteSpaceLen = 0;
		int len = original.Length;
		for(byte i = 0; i < len; i++)
		{
			if(char.IsWhiteSpace(original[i]))
				whiteSpaceLen++;
		}
		return (whiteSpaceLen == len);
	}

	public static string Shuffle(this string original)
	{
		int len = original.Length-1;
		char[] chars = original.ToCharArray();
		System.Random rand = new System.Random();

		for(int i = len; i > 0; i--)
		{
			int index = rand.Next(i);
			char tmp = chars[index];
			chars[index] = chars[i];
			chars[i] = tmp;
		}

		original = new string(chars);
		return original;
	}

	public static string Random(byte length, CharacterTypes types)
	{
		string regex = string.Empty;
		if((types&CharacterTypes.LOWERCASE)!=0) regex+="abcdefghijklmnopqrstuvwxyz";
		if((types&CharacterTypes.UPPERCASE)!=0) regex+="ABCDEFGHIJKLMNOPQRSTUVWXYZ";
		if((types&CharacterTypes.NUMBERS)!=0) regex+="0123456789";
		if((types&CharacterTypes.SPECIALS)!=0) regex+="±§!@#$%^&*()_-+={[}]|\\\"':;?/>.<,~`";
		if(string.IsNullOrEmpty(regex)) return string.Empty;

		string tmp = string.Empty;
		regex = regex.Shuffle();
		for(byte i = 0; i < length; i++) tmp+=regex[UnityEngine.Random.Range(0, regex.Length)];
		return tmp;
	}
}
