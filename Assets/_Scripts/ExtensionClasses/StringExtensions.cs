using System.Text.RegularExpressions;

public static class StringExtensions  
{
	public static bool Validate(this string original, string regex)
	{
		Regex pattern = new Regex(regex);
		return pattern.IsMatch(original);
	}
}
