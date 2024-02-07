using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private Dictionary<Outcome, bool> worldEvent = new Dictionary<Outcome, bool>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Instantiate all world events false initially
        // TODO get world events from save file
        foreach(Outcome outcome in Enum.GetValues(typeof(Outcome)))
        {
            worldEvent[outcome] = false; 
        }
    }

    private void Update()
    {
        foreach(var pair in worldEvent)
        {
            Debug.Log($"Key: {pair.Key}, Value: {pair.Value}");
        }
    }

    public void SetWorldEvent(Outcome outcome, bool status)
    {
        if(outcome != Outcome.NULL)
        {
            worldEvent[outcome] = status;
        }
    }
}
