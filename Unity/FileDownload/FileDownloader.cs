using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace FileDownload
{
    public class FileDownloader
    {
        public string Url { get; private set; }
        public string SavePath { get; private set; }
        public float ElapsedTime { get; set; }

        public event Action<string> OnCompleted;

        private UnityWebRequest unityWebRequest;

        public FileDownloader(string url, string savePath)
        {
            Url = url;
            SavePath = savePath;
        }

        public IEnumerator Start()
        {
            unityWebRequest = new UnityWebRequest(Url);
            unityWebRequest.downloadHandler = new DownloadHandlerFile(SavePath);
            Debug.Log("发送请求" + Url);
            yield return unityWebRequest.SendWebRequest();
            Debug.Log("返回请求");
            if (unityWebRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("出错了\n" + unityWebRequest.error);
            }

            OnCompleted?.Invoke(Url);
        }

        public void Stop()
        {
            Url = string.Empty;
            SavePath = string.Empty;
            ElapsedTime = 0;
            unityWebRequest?.Dispose();
            unityWebRequest = null;
            OnCompleted = null;
        }
    }
}