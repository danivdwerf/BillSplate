using UnityEngine;
using UnityEngine.Networking;

namespace Utilities
{
    public class HTTP : MonoBehaviour
    {
        public enum Method
        {
            GET = 0,
            POST = (1<<0),
            PUT = (1<<1)
        }
        public static HTTP instance;
        private void Awake ()
        {
            if (instance != null && instance != this)
                Destroy (this.gameObject);
            instance = this;
        }

        /// <summary>
        /// Sends an HTTP Request with the given data
        /// </summary>
        /// <param name="url">The url to send the request to</param>
        /// <param name="data">An WWWForm with data (GET requests require the data to be in the url i.e. http://www.mywebsite.com/folder/requesthandler.php?dat1=8&data2="hallo")</param>
        /// <param name="method">The method of the request</param>
        /// <param name="headers">The headers of the request</param>
        /// <param name="callback">A callback with information</param>
        public void SendRequest(string url, byte[] data, Method method, System.Collections.Hashtable headers, System.Action<System.Collections.Hashtable> callback = null)
        {
            StartCoroutine(Send(url, data, method, headers, callback));
        }

        private System.Collections.IEnumerator Send(string url, byte[] data, Method method, System.Collections.Hashtable headers, System.Action<System.Collections.Hashtable> callback)
        {
            UnityWebRequest www = default(UnityWebRequest);
            if(method==Method.GET)
                www = new UnityWebRequest(url, "GET");
            if(method==Method.POST)
            {
                www = new UnityWebRequest(url, "POST");
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
            }
            if(method==Method.PUT)
            {
                www = new UnityWebRequest(url, "PUT");
                www.uploadHandler = (UploadHandler) new UploadHandlerRaw(data);
            }

            www.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

            int headerLen = headers.Count;
            string[] headerKeys = new string[headerLen];
            headers.Keys.CopyTo(headerKeys, 0);
            for(int i = 0; i < headerLen; i++)
            {
                string key = headerKeys[i];
                string value = (string)headers[key];
                www.SetRequestHeader(key, value);
            }
            yield return www.SendWebRequest();
            if(callback != null)
            {
                System.Collections.Hashtable response = new System.Collections.Hashtable();
                response["response-code"] = www.responseCode;
                response["response-text"] = www.downloadHandler.text;
                callback(response);
            }
            yield break;
        }
    }
}