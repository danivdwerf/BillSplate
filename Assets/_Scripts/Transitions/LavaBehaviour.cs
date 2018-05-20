using UnityEngine;

public class LavaBehaviour : MonoBehaviour 
{
	private float moveSpeed;
	private float rotateSpeed;
	private Vector3 resetPos;
	public Vector3 ResetPos{set{this.resetPos = value;}}
	private bool isReady = false;

	private void OnEnable() 
	{
		if(!this.isReady)
			return;

		this.transform.localPosition = this.resetPos;
		StartCoroutine("UpdateBlob");
	}

	private void Awake()
	{
		this.moveSpeed = Random.Range(1.0f, 4.0f);
		this.rotateSpeed = Random.Range(0.5f, 1.5f);
		this.isReady = true;
	}

	private System.Collections.IEnumerator UpdateBlob()
	{
		while(true)
		{
			this.transform.localPosition += new Vector3(0.0f, this.moveSpeed, 0.0f);
			this.transform.localEulerAngles += new Vector3(0.0f, 0.0f, this.rotateSpeed);

			float yPos = this.transform.localPosition.y;
			if(yPos >= 275)
				this.transform.localPosition = this.resetPos;

			yield return new WaitForEndOfFrame();
		}
		yield return null;
	}

	private void OnDisable() 
	{
		StopCoroutine("UpdateBlob");
	}
}
