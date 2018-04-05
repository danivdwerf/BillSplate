using UnityEngine;
using UnityEngine.UI;

using System;
using System.Collections;

namespace Transitions
{
	public class ScaleIn : MonoBehaviour 
	{
		[SerializeField]private float scaleTime;
		[SerializeField]private float maxScale = 1.0f;
		[SerializeField]private AnimationCurve scaleCurve;
		[SerializeField]private bool scaleOnEnable;
		private void OnEnable() 
		{
			if(scaleOnEnable)
				StartCoroutine(scale(null));
		}

		public void Scale(Action onScaleDone)
		{
			StartCoroutine(scale(onScaleDone));
		}
		private IEnumerator scale(Action onScaleDone)
		{
			float timer = 0;
			float scale = 0;
			
			this.transform.localScale = new Vector3(scale, scale, scale);
			while(timer < scaleTime)
			{
				scale = maxScale*scaleCurve.Evaluate(timer/scaleTime);
				this.transform.localScale = new Vector3(scale, scale, scale);
				timer+=Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			this.transform.localScale = new Vector3(scale, scale, scale);
			if(onScaleDone != null)
				onScaleDone();
		}
	}
}