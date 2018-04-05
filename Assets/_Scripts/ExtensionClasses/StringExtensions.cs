using System.Text.RegularExpressions;

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
		for(int i = 0; i < len; i++)
		{
			if(char.IsWhiteSpace(original[i]))
				whiteSpaceLen++;
		}
		return (whiteSpaceLen == len);
	}
}
