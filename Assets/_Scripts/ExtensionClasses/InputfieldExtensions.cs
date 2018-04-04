using UnityEngine.UI;

public static class InputfieldExtensions
{
	public static void Clear(this InputField field)
	{
		field.text = "";
	}
}
