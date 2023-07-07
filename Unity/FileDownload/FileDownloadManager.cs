using System;
using UnityEngine;

namespace FileDownload
{
    public class FileDownloadManager : MonoBehaviour
    {
        public static FileDownloadManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void StartDownload(string url, string saveOPath)
        {
            FileDownloader downloader = new FileDownloader(url, saveOPath);
            downloader.OnCompleted += OnDownloaderCompleted;
            StartCoroutine(downloader.Start());
        }

        private void OnDownloaderCompleted(string url)
        {
            Debug.Log("下载完毕 " + url);
        }
    }
}