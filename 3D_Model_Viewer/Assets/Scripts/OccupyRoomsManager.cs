using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class OccupyRoomsManager : MonoBehaviour
{
    public List<TextMeshPro> roomsText;
    public string uri = "https://localhost:44378/getModel";
    void Start()
    {
        UnityWebRequest request = UnityWebRequest.Get(uri);
        StartCoroutine(nameof(WaitDataFromServer), request);
    }

    IEnumerator WaitDataFromServer(UnityWebRequest request)
    {
        yield return request;
        if (string.IsNullOrEmpty(request.error))
        {
            
        }
    }
}
