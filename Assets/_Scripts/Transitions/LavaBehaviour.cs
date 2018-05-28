using UnityEngine;

public class LavaBehaviour : MonoBehaviour 
{
	private float moveSpeed;
	private float rotateSpeed;

	private float maxX;
	public float MaxX{set{this.maxX = value;}}

	private float startY;
	public float StartY{set{this.startY = value;}}

	private bool isReady = false;

	private void OnEnable() 
	{
		if(!this.isReady)
			return;

		this.transform.localPosition = new Vector3(Random.Range(-this.maxX, this.maxX), this.startY, -1);
		this.rotateSpeed = Random.Range(-1.5f, 1.5f);
		this.moveSpeed = Random.Range(0.5f, 1.5f);
		StartCoroutine("UpdateBlob");
	}

	private void Awake()
	{
		this.isReady = true;
	}

	private System.Collections.IEnumerator UpdateBlob()
	{
		while(true)
		{
			this.transform.localPosition += new Vector3(0.0f, this.moveSpeed, 0.0f);
			this.transform.localEulerAngles += new Vector3(0.0f, 0.0f, this.rotateSpeed);

			float yPos = this.transform.localPosition.y;
			if(yPos > Screen.height)
			{
				float x = Random.Range(-this.maxX, this.maxX);
				this.transform.localPosition = new Vector3(x, this.startY, -1);
				this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, Random.Range(0, 359));
			}

			yield return new WaitForEndOfFrame();
		}
		yield return null;
	}

	private void OnDisable() 
	{
		StopCoroutine("UpdateBlob");
	}
}
