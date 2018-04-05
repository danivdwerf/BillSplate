using UnityEngine;
using System.Collections;

namespace Transitions
{
	/// <summary>
	/// Bobbing effect
	/// </summary>
	public class Bobbing : MonoBehaviour 
	{
		/// <summary>
		/// Enum with the different Axis
		/// </summary>
		private enum Axis{X=(1<<0),Y=(1<<1),Z=(1<<2), XYZ=(1<<0) | (1<<1) | (1<<2) };

		/// <summary>
		/// The axis to use
		/// </summary>
		[SerializeField]private Axis axis = Axis.Y;
		/// <summary>
		/// An animationcurve for the bobbbing
		/// </summary>
		[SerializeField]private AnimationCurve curve;
		/// <summary>
		/// The max amount of of bobbing
		/// </summary>
		[SerializeField]private float maxBob;
		/// <summary>
		/// How fast the bobbing will go
		/// </summary>
		[SerializeField]private float time = 1.0f;
		/// <summary>
		/// Wheter the object should bob or not
		/// </summary>
		[SerializeField]private bool bobOnEnable;

		/// <summary>
		/// Start bobbing
		/// </summary>
		private void OnEnable() 		
		{
			if(bobOnEnable)
				StartCoroutine("Bob");
		}

		/// <summary>
		/// Start bobbing
		/// </summary>
		public void StartBobbing()
		{
			StartCoroutine("Bob");
		}

		/// <summary>
		/// Stop bobbing
		/// </summary>
		public void StopBobbing()
		{
			StopCoroutine("Bob");	
		}

		/// <summary>
		/// Bob the object up and down
		/// </summary>
		private IEnumerator Bob()	
		{
			float timer = 0.0f;
			float step = 0.0f;
			Vector3 pos = new Vector3(0.0f, 0.0f, 0.0f);

			while(true)
			{
				while(timer < time)
				{
					step = maxBob * curve.Evaluate(timer/time);
					if((axis&Axis.X)!=0)
						pos.x = step; 
					if((axis&Axis.Y)!=0)
						pos.y = -step;
					if((axis&Axis.Z)!=0)
						pos.z = step;
					this.transform.position = pos;		
					timer+=Time.deltaTime;
					yield return new WaitForEndOfFrame();
				}

				while(timer > 0.0f)
				{
					step = maxBob * curve.Evaluate(timer/time);
					if((axis&Axis.X)!=0)
						pos.x = step; 
					if((axis&Axis.Y)!=0)
						pos.y = -step;
					if((axis&Axis.Z)!=0)
						pos.z = step;
					this.transform.position = pos;	
					timer-=Time.deltaTime;	
					yield return new WaitForEndOfFrame();
				}
			}
		}
	}
}