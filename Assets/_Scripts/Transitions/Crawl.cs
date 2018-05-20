using UnityEngine;

using System.Collections;

namespace Transitions
{
	public class Crawl : MonoBehaviour 
	{
		private enum Axis{X=(1<<0), Y=(1<<1)};

		[SerializeField]private Axis axis = Axis.Y;
		[SerializeField]private float stepSize = 0.1f;
		[SerializeField]private float max = -1.0f;

		[SerializeField]private Vector2 resetPos;
		public Vector2 ResetPos{set{this.resetPos = value;}}

		private void OnEnable() 
		{	
			StartCoroutine("crawl");
		}

		private IEnumerator crawl()
		{
			while(true)
			{
				bool useX = (axis&Axis.X) != 0;
				bool useY = (axis&Axis.Y) != 0;
				float x = 0.0f;
				float y = 0.0f;

				if(useX) x = stepSize;
				if(useY) y = stepSize;

				this.transform.localPosition += new Vector3(x, y, 0.0f);

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
							this.transform.localPosition = new Vector3(resetPos.x, this.transform.localPosition.y, this.transform.localPosition.z);
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

				yield return new WaitForEndOfFrame();
			}
		}
		private void OnDisable() 
		{
			StopCoroutine("crawl");	
		}
	};
};
