using UnityEngine;

using System;
using System.Collections;

namespace Utilities
{
	public class JSON : MonoBehaviour
	{
		public static JSON instance;
		private void Awake ()
		{
			if (instance != null && instance != this)
				Destroy (this.gameObject);
			instance = this;
		}

		public void ToClass<T>(string json, ref T dataHolder)
		{
			Type type = dataHolder.GetType();
			dataHolder = (T)JsonUtility.FromJson(json, type);
		}

		public string ToJSON<T>(T data, bool prettify = false)
		{
			return JsonUtility.ToJson(data, prettify);
		}

		public void loadJSON(string url, Action<string>callback)
		{
			StartCoroutine(load(url, callback));
		}

		private IEnumerator load(string url, Action<string> callback)
		{
			if(string.IsNullOrEmpty(url))
				yield break;

			WWW www = new WWW(url);
			yield return www;


			if(www.error != null)
			{
				if(callback != null)
					callback(www.error);
				yield break;
			}

			if(!www.isDone)
				yield break;

			if(callback == null)
				yield break;
			
			callback(www.text);
		}
	}
}