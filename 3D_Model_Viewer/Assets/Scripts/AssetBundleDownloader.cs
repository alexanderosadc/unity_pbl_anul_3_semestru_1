using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
public class AssetBundleDownloader : MonoBehaviour
{
    private string uriGoogle = "https://google.com";
     public string uri = "http://localhost:5000";
    // public string assetName = "cube";

    private void Start()
    {
        StartCoroutine(nameof(WaitDataFromServer));
    }

    IEnumerator WaitDataFromServer()
    {
        using (UnityWebRequest webRequest = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                
                //var textFromServer = webRequest.downloadHandler.text;
                AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(webRequest);
                Debug.Log(assetBundle);
                GameObject gameObject = assetBundle.LoadAsset<GameObject>("cube");
                Instantiate(gameObject);



                // AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
            }     
        }
    }
}
