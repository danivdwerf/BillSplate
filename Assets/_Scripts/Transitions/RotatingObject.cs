using UnityEngine;

namespace Transitions
{
	class RotatingObject : MonoBehaviour
	{
		private enum Axis{XAxis=(1<<0),YAxis=(1<<1),ZAxis=(1<<2),XandYAxis=(1<<0)|(1<<1),XandZAxis=(1<<0)|(1<<2),YandZAxis=(1<<1)|(1<<2),XYandZAxis=(1<<0)|(1<<1)|(1<<2)};
		[SerializeField]private Axis axis = Axis.YAxis;
		[SerializeField]private float rotationSpeed;
		[SerializeField]private float min = -20;
		[SerializeField] private float max = 20;


		private bool shouldRotate = true;
		public bool ShouldRotate{get{return this.shouldRotate;} set{this.shouldRotate = value;}}

		private void FixedUpdate()
		{
			if(!shouldRotate)
				return;

			Vector3 rotation = this.transform.localEulerAngles;
			float angle = -1;
			float xAxis = 0;
			float yAxis = 0;
			float zAxis = 0;

			if((axis&Axis.XAxis)!=0)
			{
				angle = rotation.x;
     			angle = (angle > 180) ? angle - 360 : angle;
				if(angle <= this.min || angle >= this.max)
					this.rotationSpeed *= -1;
				xAxis = rotationSpeed;
			}

			if((axis&Axis.YAxis)!=0)
			{
				angle = rotation.y;
     			angle = (angle > 180) ? angle - 360 : angle;
				if(angle <= this.min || angle >= this.max)
					this.rotationSpeed *= -1;
				yAxis = rotationSpeed;
			}
			
			if((axis&Axis.ZAxis)!=0)
			{
				angle = rotation.z;
     			angle = (angle > 180) ? angle - 360 : angle;
				if(angle <= this.min || angle >= this.max)
					this.rotationSpeed *= -1;
				zAxis = rotationSpeed;
			}

			Vector3 vec = new Vector3(xAxis, yAxis, zAxis);
			this.transform.localEulerAngles += vec*Time.deltaTime;
		}
	}
}