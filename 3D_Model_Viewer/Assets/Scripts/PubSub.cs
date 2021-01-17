using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PubSub : MonoBehaviour
{
    public static PubSub current;
    public event Action RoomLoaded;
    public event Action<GameObject> OnClickOnPlate;
    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }


    public virtual void OnOnClickOnPlate(GameObject obj)
    {
        OnClickOnPlate?.Invoke(obj);
    }

    public virtual void OnRoomLoaded()
    {
        RoomLoaded?.Invoke();
    }
}
