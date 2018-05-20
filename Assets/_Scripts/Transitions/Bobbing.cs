using UnityEngine;
using System.Collections;

namespace Transitions
{
	public class Bobbing : MonoBehaviour 
	{
		private enum Axis{X=(1<<1), Y=(1<<2), XY=(1<<1) | (1<<2)};

		[SerializeField]private Axis axis = Axis.Y;
		[SerializeField]private AnimationCurve curve;
		[SerializeField]private float maxValue;
		[SerializeField]private float duration = 1.0f;
		[SerializeField]private bool bobOnEnable;

		private RectTransform rectTransform;

		private void OnEnable() 		
		{
			if(this.rectTransform == null)
				return;
			
			if(this.bobOnEnable)
				StartCoroutine("Bob");
		}

		private void Awake()
		{
			this.rectTransform = this.gameObject.GetComponent<RectTransform>();
			if(this.bobOnEnable)
				this.StartBobbing();
		}

		public void StartBobbing()
		{
			StartCoroutine(Bob());
		}

		public void StopBobbing()
		{
			StopCoroutine(Bob());	
		}

		private IEnumerator Bob()	
		{
			Vector2 from = this.rectTransform.anchoredPosition;
			Vector2 pos = new Vector2(0.0f, 0.0f);	

			float timer = 0.0f;

			while(true)
			{
				while(timer < this.duration)
				{
					float step = this.maxValue * this.curve.Evaluate(timer/this.duration);
					if((this.axis&Axis.X) != 0) pos.x = from.x + step;
					if((this.axis&Axis.Y) != 0) pos.y = from.y + step;
					this.rectTransform.anchoredPosition = pos;
					timer += Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}

				while(timer > 0.0)
				{
					float step = this.maxValue * this.curve.Evaluate(timer/this.duration);
					if((this.axis&Axis.X) != 0) pos.x = from.x + step;
					if((this.axis&Axis.Y) != 0) pos.y = from.y + step;
					this.rectTransform.anchoredPosition = pos;
					timer -= Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}
			}
		}
	}
}