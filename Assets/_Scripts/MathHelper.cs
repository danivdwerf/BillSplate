using UnityEngine;
public static class MathHelper 
{
	public static float GetDegreesPerSegment(float segmentCount)
	{
		return 360 / segmentCount;
	}

	public static Vector3 PlaceOnCircle (Vector3 center, float radius, float angleIncrement)
	{
		Vector3 pos;
		pos.x = center.x+radius*Mathf.Sin(angleIncrement*Mathf.Deg2Rad);
		pos.y = center.y+radius*Mathf.Cos(angleIncrement*Mathf.Deg2Rad);
		pos.z = center.z;
		return pos;
	}
}
