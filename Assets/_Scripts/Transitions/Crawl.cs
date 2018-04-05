using UnityEngine;

using System.Collections;

namespace Transitions
{
	/// <summary>
	/// Crawling effect
	/// </summary>
	public class Crawl : MonoBehaviour 
	{
		/// <summary>
		/// The possible axis to crawl
		/// </summary>
		private enum Axis{X=(1<<0),Y=(1<<1),Z=(1<<2)};

		/// <summary>
		/// The axis crawl on
		/// </summary>
		[SerializeField]private Axis axis = Axis.Y;
		/// <summary>
		/// The crawling step size
		/// </summary>
		[SerializeField]private float stepSize = 0.1f;
		/// <summary>
		/// The max amount of crawling
		/// </summary>
		[SerializeField]private float max = -1.0f;
		/// <summary>
		/// The position to go to when the max is reached
		/// </summary>
		[SerializeField]private Vector3 resetPos;

		/// <summary>
		/// Start crawling
		/// </summary>
		private void OnEnable() 
		{
			StartCoroutine("crawl");
		}

		/// <summary>
		/// Crawl object with the given step
		/// </summary>
		private IEnumerator crawl()
		{
			while(true)
			{
				bool useX = (axis&Axis.X)!=0;
				bool useY = (axis&Axis.Y)!=0;
				bool useZ = (axis&Axis.Z)!=0;
				float x = 0.0f;
				float y = 0.0f;
				float z = 0.0f;

				if(useX)x=stepSize;
				if(useY)y=stepSize;
				if(useZ)z=stepSize;

				this.transform.localPosition += new Vector3(x, y, z);

				if(useX)
				{	
					if(max > 0)
					{
						if(this.transform.localPosition.x >= this.max)
							this.transform.localPosition = new Vector3(resetPos.x, this.transform.localPosition.y, this.transform.localPosition.z);
					}
					else
					{
						if(this.transform.localPosition.x <= this.max)
							this.transform.localPosition =new Vector3(resetPos.x, this.transform.localPosition.y, this.transform.localPosition.z);
					}
				}
				
				if(useY)
				{
					if(max > 0)
					{
						if(this.transform.localPosition.y >= this.max)
							this.transform.localPosition = new Vector3(this.transform.localPosition.x, resetPos.y, this.transform.localPosition.z);
					}
					else
					{
						if(this.transform.localPosition.y <= this.max)
							this.transform.localPosition = new Vector3(this.transform.localPosition.x, resetPos.y, this.transform.localPosition.z);
					}
				}

				if(useZ)
				{
					if(max > 0)
					{
						if(this.transform.localPosition.z >= this.max)
							this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, resetPos.z);
					}
					else
					{
						if(this.transform.localPosition.z <= this.max)
							this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, resetPos.z);
					}
				}

				yield return new WaitForEndOfFrame();
			}
		}

		/// <summary>
		/// Stop crawling
		/// </summary>
		private void OnDisable() 
		{
			StopCoroutine("crawl");	
		}
	};
};
