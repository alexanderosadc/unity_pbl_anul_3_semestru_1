using System;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class RoomsManager : MonoBehaviour
{ 
    [SerializeField] private List<GameObject> roomsGameObjects;

    [SerializeField] private Material emptyRoomMaterial;
    [SerializeField] private Material occupiedRoomMaterial;
    
    private void Awake()
    {
        PubSub.current.RoomLoaded += GetOccupiedRoomsFromServer;
    }

    private void GetOccupiedRoomsFromServer()
    {
        StartCoroutine(GetRequest("https://localhost:44378/api/3Dmodel/getRoomStatus"));
    }
    IEnumerator GetRequest(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();
            
            if (webRequest.isNetworkError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                JSONNode jsonNode = JSON.Parse(webRequest.downloadHandler.text);
                //Debug.Log(jsonNode[0]);
                if (jsonNode[0] == "All Empty")
                {
                    foreach (GameObject room in roomsGameObjects)
                    {
                        ColorRooms(room.transform, emptyRoomMaterial);
                    }
                }
                else
                {
                    foreach (var keyValue in jsonNode[0])
                    {
                        if (keyValue.Key == "roomName")
                        {
                            string roomNameFromJson = keyValue.Value;
                            
                            char roomNumberFromJson = roomNameFromJson[roomNameFromJson.Length - 1];
                            
                            foreach (GameObject room in roomsGameObjects)
                            {
                                char roomNumberGameObject = room.name[room.name.Length - 1];
                                if (roomNumberFromJson == roomNumberGameObject)
                                {
                                    ColorRooms(room.transform, emptyRoomMaterial);
                                }
                                else
                                {
                                    ColorRooms(room.transform, occupiedRoomMaterial);
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private void ColorRooms(Transform parent, Material roomMaterial)
    {
        foreach (Transform child in parent)
        {
            child.GetComponent<Renderer>().material = roomMaterial;
        }
    }
}
