using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using System.Security.Cryptography;
using Proyecto26;
using UnityEditor;


public class AssetBundleDownloader : MonoBehaviour
{
    [SerializeField] private RoomsManager _roomsManager;
    private string uriGoogle = "https://google.com";
    public string uri = "";
    // public string assetName = "cube";

    private void Awake()
    {
        //PubSub.current.OnRoomLoaded += ;
    }

    private void Start()
    {
        /*if (File.Exists(Application.persistentDataPath + "/3DModel"))
        {
            LoadFromLocal();
        }
        else
        { */
            Debug.Log("RoomDownloading");
            Download3DModel();
       // }
        
        //StartCoroutine(nameof(WaitDataFromServer));
    }

    private void LoadFromLocal()
    {
        byte[] data = File.ReadAllBytes(Application.persistentDataPath + "/3DModel");
        AssetBundle assetBundle = AssetBundle.LoadFromMemory(data);
        GameObject gameObject = assetBundle.LoadAsset<GameObject>("office_1");
        Instantiate(gameObject);
        _roomsManager.GetOccupiedRoomsFromServer();
        //PubSub.current.OnRoomLoaded();
    }

    private void Download3DModel()
    {
        RestClient.Get(new RequestHelper {
            Uri = uri,
            DownloadHandler = new DownloadHandlerBuffer()
        }).Then(res =>
        {
            String text = res.Text;
            byte[] byteStra = StringToByteArray(text);
            File.WriteAllBytes(Application.persistentDataPath + "/3DModel", byteStra);
            //byte[] byteStr = Encoding.UTF8.GetBytes(text);
            AssetBundle assetBundle = AssetBundle.LoadFromMemory(byteStra);
            
            GameObject gameObject = assetBundle.LoadAsset<GameObject>("office_1");
            Instantiate(gameObject);
            _roomsManager.GetOccupiedRoomsFromServer();
            //PubSub.current.OnRoomLoaded();
        }).Catch(err => {
            //EditorUtility.DisplayDialog ("Error", err.Message, "Ok");
        });
    }

    public static byte[] StringToByteArray(String hex)
    {
        hex = hex.Replace("-", "");
        int NumberChars = hex.Length;
        byte[] bytes = new byte[NumberChars / 2];
        for (int i = 0; i < NumberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        return bytes;
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
                GameObject gameObject = assetBundle.LoadAsset<GameObject>("office_1");
                Instantiate(gameObject);



                // AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(webRequest);
            }     
        }
    }
}
