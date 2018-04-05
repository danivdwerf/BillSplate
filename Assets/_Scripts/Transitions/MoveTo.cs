using UnityEngine;

using System.Collections;

namespace Transitions
{
    public class MoveTo : MonoBehaviour 
    {
        [SerializeField] private Vector3 desPosition;
        [SerializeField] private AnimationCurve curve;
        [SerializeField] private float duration = 2.0f;
        
        public void Move(System.Action onMoveFinished)
		{
			StartCoroutine(move(onMoveFinished));
		}
		private IEnumerator move(System.Action onMoveFinished)
		{
			float timer = 0;
			Vector3 position = new Vector3();

			while(timer < duration)
			{
                float evalutation = curve.Evaluate(timer/duration);
                position.x = desPosition.x*evalutation;
                position.y = desPosition.y*evalutation;
                position.z = desPosition.z*evalutation;
                this.transform.localPosition = position;

				timer+=Time.deltaTime;
				yield return new WaitForEndOfFrame();
			}
			this.transform.localPosition = new Vector3(this.desPosition.x, this.desPosition.y, this.desPosition.z);
			if(onMoveFinished != null)
				onMoveFinished();
		}
    }
}