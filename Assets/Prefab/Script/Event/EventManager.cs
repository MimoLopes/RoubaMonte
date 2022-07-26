using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour
{
    public bool logEvent;
    private Dictionary<string, UnityEvent<object>> eventDictionary;
    private static EventManager eventManager;

    public static EventManager instance
    {
        get
        {
            if (!eventManager)
            {
                eventManager = FindObjectOfType(typeof(EventManager)) as EventManager;

                if (!eventManager)
                {
                    Debug.LogError("There needs to be one active EventManger script on a GameObject in your scene.");
                }
                else
                {
                    eventManager.Init();
                }
            }
            return eventManager;
        }
    }

    void Init()
    {
        if (eventDictionary == null)
        {
            eventDictionary = new Dictionary<string, UnityEvent<object>>();
        }
    }

    public static void StartListening(string eventName, UnityAction<object> listener)
    {

        if (instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.AddListener(listener);
        }
        else
        {
            thisEvent = new UnityEvent<object>();
            thisEvent.AddListener(listener);
            instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction<object> listener)
    {
        if (eventManager == null)
            return;
        
        if (instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName, object obj = null)
    {
        if (instance.eventDictionary.TryGetValue(eventName, out var thisEvent))
        {
            if (instance.logEvent)
                Debug.Log("#Event# TriggerEvent: " + eventName);

            thisEvent?.Invoke(obj);
        }
    }
}