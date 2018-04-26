using UnityEngine;

using System.Collections;

namespace Transitions
{
    public class MoveTo : MonoBehaviour 
    {
		[SerializeField]private Vector3 fromPosition;
        [SerializeField]private Vector3 toPosition;
        [SerializeField]private AnimationCurve curve;
        [SerializeField]private float duration = 2.0f;
		[SerializeField]private bool startOnEnable;

		private RectTransform rectTransform;

		private void OnEnable() 
		{
			if(this.rectTransform == null)
				return;

			if(!this.startOnEnable)
				return;
			
			this.Move(null);
		}

		private void Awake()
		{
			this.rectTransform = this.gameObject.GetComponent<RectTransform>();
		}
        
        public void Move(System.Action onMoveFinished)
		{
			StartCoroutine(move(onMoveFinished));
		}

		private IEnumerator move(System.Action onMoveFinished)
		{
			float timer = 0;
			Vector3 curr = this.fromPosition;
			Vector3 delta = this.toPosition-this.fromPosition;

			this.rectTransform.anchoredPosition = this.fromPosition;
			while(timer < duration)
			{
                float evalutation = curve.Evaluate(timer/duration);

                curr.x = this.fromPosition.x+(delta.x*evalutation);
                curr.y = this.fromPosition.y+(delta.y*evalutation);
                curr.z = this.fromPosition.z+(delta.z*evalutation);
                this.rectTransform.anchoredPosition = curr;

				timer+=Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}

			this.rectTransform.anchoredPosition = this.toPosition;
			if(onMoveFinished != null)
				onMoveFinished();
		}
    }
}