using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Proyecto26;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Networking;

public class RoomsManager : MonoBehaviour
{ 
    private List<GameObject> roomsGameObjects;

    [SerializeField] private Material emptyRoomMaterial;
    [SerializeField] private Material occupiedRoomMaterial;

    private void Start()
    {
        //PubSub.current.RoomLoaded += GetOccupiedRoomsFromServer;
        
    }

    private void GetAllRoomObjects()
    {
        roomsGameObjects = GameObject.FindGameObjectsWithTag("Room").ToList();
        GetRequest("https://localhost:44378/api/3Dmodel/getRoomStatus");
        
    }
    public void GetOccupiedRoomsFromServer()
    {
        GetAllRoomObjects();
       
    }
    private void GetRequest(string uri)
    {
        RestClient.Get(new RequestHelper {
            Uri = uri,
            DownloadHandler = new DownloadHandlerBuffer()
        }).Then(res =>
        {
            Debug.Log(res.Text);
            JSONNode jsonNode = JSON.Parse(res.Text);
            if(jsonNode[0] != "All Empty")
            {
                List<char> roomNumbers = new List<char>();
                for (int i = 0; i < jsonNode.Count; i++)
                {
                    foreach (var keyValue in jsonNode[i])
                    {
                        if (keyValue.Key == "roomName")
                        {
                            string roomNameFromJson = keyValue.Value;
                            
                            char roomNumberFromJson = roomNameFromJson[roomNameFromJson.Length - 1];
                                
                            roomNumbers.Add(roomNumberFromJson);
                            foreach (GameObject room in roomsGameObjects)
                            {
                                char roomNumberGameObject = room.name[room.name.Length - 1];
                                Debug.Log(roomNumberFromJson);
                                if (roomNumberFromJson == roomNumberGameObject)
                                {
                                    ColorRooms(room.transform, occupiedRoomMaterial);
                                }
                            }
                        }
                    }
                }
            }
            
        }).Catch(err => {
            //EditorUtility.DisplayDialog ("Error", err.Message, "Ok");
        });
    }

    private void ColorRooms(Transform parent, Material roomMaterial)
    {
        foreach (Transform child in parent)
        {
            child.GetComponent<Renderer>().material = roomMaterial;
        }
    }
}
